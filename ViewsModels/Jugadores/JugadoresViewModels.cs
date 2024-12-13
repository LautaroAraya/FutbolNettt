using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FutbolNet.Class;
using FutbolNet.Modelos;
using FutbolNet.Service;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FutbolNet.ViewModels
{
    public class JugadoresViewModel : NotificationObject
    {
        GenericService<Jugador> jugadorService = new GenericService<Jugador>();

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

        private ObservableCollection<Jugador> jugadores;
        public ObservableCollection<Jugador> Jugadores
        {
            get { return jugadores; }
            set
            {
                jugadores = value;
                OnPropertyChanged();
            }
        }

        private Jugador jugadorCurrent;
        public Jugador JugadorCurrent
        {
            get { return jugadorCurrent; }
            set
            {
                jugadorCurrent = value;
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

        public JugadoresViewModel()
        {
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command(Editar, PermitirEditar);
            EliminarCommand = new Command(Eliminar, PermitirEliminar);

            Task.Run(async () => await ObtenerJugadores());

            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                AlRecibirMensaje(m);
            });
        }

        private async void AlRecibirMensaje(MyMessage m)
        {
            if (m.Value == "VolverAJugadores")
            {
                await RefreshJugadores(this);
            }
        }

        private async Task RefreshJugadores(object obj)
        {
            IsRefreshing = true;
            await ObtenerJugadores();
            IsRefreshing = false;
        }

        private bool PermitirEliminar(object arg)
        {
            return JugadorCurrent != null;
        }

        private async void Eliminar(object obj)
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert(
                "Eliminar un jugador",
                $"Está seguro que desea eliminar al jugador {JugadorCurrent.Nombre} del equipo {JugadorCurrent.Equipo}?",
                "Si",
                "No"
            );
            if (respuesta)
            {
                ActivityStart = true;
                await jugadorService.DeleteAsync(JugadorCurrent.Id);
                await ObtenerJugadores();
            }
        }

        private bool PermitirEditar(object arg)
        {
            return JugadorCurrent != null;
        }

        private void Editar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditJugadorView") { Jugador = JugadorCurrent });
        }

        private void Agregar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditJugadorView"));
        }

        public async Task ObtenerJugadores()
        {
            ActivityStart = true;
            var jugadores = await jugadorService.GetAllAsync();
            Jugadores = new ObservableCollection<Jugador>(jugadores);
            ActivityStart = false;
        }
    }
}
