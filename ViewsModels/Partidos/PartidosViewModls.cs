using CommunityToolkit.Mvvm.Messaging;
using FutbolNet.Class;
using FutbolNet.Modelos;
using FutbolNet.Service;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FutbolNet.ViewModels
{
    public class PartidosViewModel : NotificationObject
    {
        private readonly GenericService<Partido> partidoService = new GenericService<Partido>();
        private readonly GenericService<Equipo> equipoService = new GenericService<Equipo>();

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

        private ObservableCollection<PartidosEquipos> partidos;
        public ObservableCollection<PartidosEquipos> Partidos
        {
            get => partidos;
            set
            {
                if (partidos != value)
                {
                    partidos = value;
                    OnPropertyChanged();
                }
            }
        }

        private PartidosEquipos partidoCurrent;
        public PartidosEquipos PartidoCurrent
        {
            get => partidoCurrent;
            set
            {
                if (partidoCurrent != value)
                {
                    partidoCurrent = value;
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

        public PartidosViewModel()
        {
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command(Editar, PermitirEditar);
            EliminarCommand = new Command(Eliminar, PermitirEliminar);

            Task.Run(async () => await ObtenerPartidos());

            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                if (m.Value == "VolverAPartidos")
                {
                    Task.Run(async () => await RefreshPartidos());
                }
            });
        }

        private async Task RefreshPartidos()
        {
            IsRefreshing = true;
            await ObtenerPartidos();
            IsRefreshing = false;
        }

        private bool PermitirEliminar(object arg) => PartidoCurrent != null;

        private async void Eliminar(object obj)
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert(
                "Eliminar un partido",
                $"¿Está seguro que desea eliminar el partido entre {PartidoCurrent.EquipoLocal?.Nombre} y {PartidoCurrent.EquipoVisitante?.Nombre}?",
                "Sí",
                "No");

            if (respuesta)
            {
                ActivityStart = true;
                await partidoService.DeleteAsync(PartidoCurrent.Partido.Id);
                await ObtenerPartidos();
                ActivityStart = false;
            }
        }

        private bool PermitirEditar(object arg) => PartidoCurrent != null;

        private void Editar(object obj)
        {
            if (PartidoCurrent != null)
            {
                PartidoCurrent.Partido.EquipoLocalId = PartidoCurrent.EquipoLocal?.Id ?? 0;
                PartidoCurrent.Partido.EquipoVisitanteId = PartidoCurrent.EquipoVisitante?.Id ?? 0;
            }
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditPartidoView") { Partido = PartidoCurrent.Partido });
        }

        private async void Agregar(object obj)
        {
            if (partidoCurrent != null)
            {
                if (PartidoCurrent.EquipoLocal?.Id == 0 && !string.IsNullOrEmpty(PartidoCurrent.EquipoLocal?.Nombre))
                {
                    var equipoLocal = await equipoService.GetAllAsync();
                    PartidoCurrent.EquipoLocal.Id = equipoLocal.FirstOrDefault(e => e.Nombre == PartidoCurrent.EquipoLocal.Nombre)?.Id ?? 0;
                }

                if (PartidoCurrent.EquipoVisitante?.Id == 0 && !string.IsNullOrEmpty(PartidoCurrent.EquipoVisitante?.Nombre))
                {
                    var equipoVisitante = await equipoService.GetAllAsync();
                    PartidoCurrent.EquipoVisitante.Id = equipoVisitante.FirstOrDefault(e => e.Nombre == PartidoCurrent.EquipoVisitante.Nombre)?.Id ?? 0;
                }

                PartidoCurrent.Partido.EquipoLocalId = PartidoCurrent.EquipoLocal?.Id ?? 0;
                PartidoCurrent.Partido.EquipoVisitanteId = PartidoCurrent.EquipoVisitante?.Id ?? 0;
            }
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditPartidoView"));
        }

        public async Task ObtenerPartidos(int? equipoLocalId = null, int? equipoVisitanteId = null)
        {
            ActivityStart = true;
            var partidosLista = await partidoService.GetAllAsync();

            // Filtrar los partidos según los IDs de equipo local y visitante
            if (equipoLocalId.HasValue)
            {
                partidosLista = partidosLista.Where(p => p.EquipoLocalId == equipoLocalId.Value).ToList();
            }

            if (equipoVisitanteId.HasValue)
            {
                partidosLista = partidosLista.Where(p => p.EquipoVisitanteId == equipoVisitanteId.Value).ToList();
            }

            var partidosConDetalles = new ObservableCollection<PartidosEquipos>();

            foreach (var partido in partidosLista)
            {
                var equipoLocal = await equipoService.GetByIdAsync(partido.EquipoLocalId);
                var equipoVisitante = await equipoService.GetByIdAsync(partido.EquipoVisitanteId);

                var detalle = new PartidosEquipos
                {
                    Partido = partido,
                    EquipoLocal = equipoLocal,
                    EquipoVisitante = equipoVisitante
                };

                partidosConDetalles.Add(detalle);
            }

            Partidos = partidosConDetalles;
            ActivityStart = false;
        }
    }

    // Clase auxiliar para vincular los datos del partido con los equipos
    public class PartidosEquipos
    {
        public Partido Partido { get; set; }
        public Equipo EquipoLocal { get; set; }
        public Equipo EquipoVisitante { get; set; }

        // Formatear la fecha para que se vea como dd/MM/yyyy
        public string PartidoFechaFormatted
        {
            get
            {
                return Partido.Fecha.ToString("dd/MM/yyyy");
            }
        }
    }
}
