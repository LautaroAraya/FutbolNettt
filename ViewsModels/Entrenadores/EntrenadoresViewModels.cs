using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FutbolNet.Class;
using FutbolNet.Modelos;
using FutbolNet.Service;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FutbolNet.ViewModels
{
    public class EntrenadoresViewModel : NotificationObject
    {
        GenericService<Entrenador> entrenadorService = new GenericService<Entrenador>();

        private bool activityStart;
        public bool ActivityStart
        {
            get { return activityStart; }
            set
            {
                activityStart = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Entrenador> entrenadores;
        public ObservableCollection<Entrenador> Entrenadores
        {
            get { return entrenadores; }
            set
            {
                entrenadores = value;
                OnPropertyChanged();
            }
        }

        private Entrenador entrenadorCurrent;
        public Entrenador EntrenadorCurrent
        {
            get { return entrenadorCurrent; }
            set
            {
                entrenadorCurrent = value;
                OnPropertyChanged();
                EditarCommand.ChangeCanExecute();
                EliminarCommand.ChangeCanExecute();
            }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public Command AgregarCommand { get; }
        public Command EditarCommand { get; }
        public Command EliminarCommand { get; }

        public EntrenadoresViewModel()
        {
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command(Editar, PermitirEditar);
            EliminarCommand = new Command(Eliminar, PermitirEliminar);

            Task.Run(async () => await ObtenerEntrenadores());

            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                AlRecibirMensaje(m);
            });
        }

        private async void AlRecibirMensaje(MyMessage m)
        {
            if (m.Value == "VolverAEntrenadores")
            {
                await RefreshEntrenadores(this);
            }
        }

        private async Task RefreshEntrenadores(object obj)
        {
            IsRefreshing = true;
            await ObtenerEntrenadores();
            IsRefreshing = false;
        }

        private bool PermitirEliminar(object arg)
        {
            return EntrenadorCurrent != null;
        }

        private async void Eliminar(object obj)
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert(
                "Eliminar un entrenador",
                $"¿Está seguro que desea eliminar al entrenador {EntrenadorCurrent.Nombre}?",
                "Sí",
                "No"
            );
            if (respuesta)
            {
                ActivityStart = true;
                await entrenadorService.DeleteAsync(EntrenadorCurrent.Id);
                await ObtenerEntrenadores();
            }
        }

        private bool PermitirEditar(object arg)
        {
            return EntrenadorCurrent != null;
        }

        private void Editar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditEntrenadorView") { Entrenador = EntrenadorCurrent });
        }

        private void Agregar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditEntrenadorView"));
        }

        public async Task ObtenerEntrenadores()
        {
            ActivityStart = true;
            var entrenadores = await entrenadorService.GetAllAsync();
            Entrenadores = new ObservableCollection<Entrenador>(entrenadores);
            ActivityStart = false;
        }
    }
}
