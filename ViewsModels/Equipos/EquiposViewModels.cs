using CommunityToolkit.Mvvm.Messaging;
using FutbolNet.Class;
using FutbolNet.Modelos;
using FutbolNet.Service;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FutbolNet.ViewModels
{
    public class EquiposViewModel : NotificationObject
    {
        private readonly GenericService<Equipo> equipoService = new GenericService<Equipo>();
        private readonly GenericService<Entrenador> entrenadorService = new GenericService<Entrenador>();

        private bool activityStart;
        public bool ActivityStart
        {
            get => activityStart;
            set
            {
                if (activityStart != value)
                {
                    activityStart = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<EntrenadoresEquiposPartidos> equipos;
        public ObservableCollection<EntrenadoresEquiposPartidos> Equipos
        {
            get => equipos;
            set
            {
                if (equipos != value)
                {
                    equipos = value;
                    OnPropertyChanged();
                }
            }
        }

        private EntrenadoresEquiposPartidos equipoCurrent;
        public EntrenadoresEquiposPartidos EquipoCurrent
        {
            get => equipoCurrent;
            set
            {
                if (equipoCurrent != value)
                {
                    equipoCurrent = value;
                    OnPropertyChanged();
                    EditarCommand.ChangeCanExecute();
                    EliminarCommand.ChangeCanExecute();
                }
            }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    OnPropertyChanged();
                }
            }
        }

        public Command AgregarCommand { get; }
        public Command EditarCommand { get; }
        public Command EliminarCommand { get; }

        public EquiposViewModel()
        {
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command(Editar, PermitirEditar);
            EliminarCommand = new Command(Eliminar, PermitirEliminar);

            Task.Run(async () => await ObtenerEquipos());

            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                if (m.Value == "VolverAEquipos")
                {
                    Task.Run(async () => await RefreshEquipos());
                }
            });
        }

        private async Task RefreshEquipos()
        {
            IsRefreshing = true;
            await ObtenerEquipos();
            IsRefreshing = false;
        }

        private bool PermitirEliminar(object arg) => EquipoCurrent != null;

        private async void Eliminar(object obj)
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert(
                "Eliminar un equipo",
                $"¿Está seguro que desea eliminar el equipo {EquipoCurrent.Equipo.Nombre}?",
                "Sí",
                "No");

            if (respuesta)
            {
                ActivityStart = true;
                await equipoService.DeleteAsync(EquipoCurrent.Equipo.Id);
                await ObtenerEquipos();
                ActivityStart = false;
            }
        }

        private bool PermitirEditar(object arg) => EquipoCurrent != null;

        private void Editar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditEquipoView") { Equipo = EquipoCurrent.Equipo });
        }

        private void Agregar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditEquipoView"));
        }

        public async Task ObtenerEquipos()
        {
            ActivityStart = true;
            var equiposLista = await equipoService.GetAllAsync();
            var equiposConDetalles = new ObservableCollection<EntrenadoresEquiposPartidos>();

            foreach (var equipo in equiposLista)
            {
                var entrenador = await entrenadorService.GetByIdAsync(equipo.EntrenadorId);

                var detalle = new EntrenadoresEquiposPartidos
                {
                    Equipo = equipo,
                    Entrenador = entrenador
                };

                equiposConDetalles.Add(detalle);
            }

            Equipos = equiposConDetalles;
            ActivityStart = false;
        }
    }

    public class EntrenadoresEquiposPartidos
    {
        public Equipo Equipo { get; set; }
        public Entrenador Entrenador { get; set; }
    }
}