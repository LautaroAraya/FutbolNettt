using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using FutbolNet.Class;
using FutbolNet.Modelos;
using FutbolNet.Service;

namespace FutbolNet.ViewModels
{
    public class AddEditJugadorViewModel : ObservableObject
    {
        private readonly GenericService<Jugador> jugadorService = new GenericService<Jugador>();

        private Jugador jugador;
        public Jugador Jugador
        {
            get => jugador;
            set
            {
                jugador = value ?? new Jugador(); // Asigna un nuevo Jugador si el valor es null
                OnPropertyChanged();
            }
        }

        public AddEditJugadorViewModel()
        {
            Jugador = new Jugador(); // Asegura que Jugador esté inicializado al inicio
            GuardarCommand = new AsyncRelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        public IAsyncRelayCommand GuardarCommand { get; }
        public IRelayCommand CancelarCommand { get; }

        private async Task Guardar()
        {
            if (Jugador == null)
            {
                Jugador = new Jugador(); // Crea una nueva instancia de Jugador si es null
            }

            try
            {
                // Verifica si es un jugador nuevo o existente
                if (Jugador.Id == 0)
                {
                    await jugadorService.AddAsync(Jugador); // Agrega el jugador nuevo
                }
                else
                {
                    await jugadorService.UpdateAsync(Jugador); // Actualiza el jugador existente
                }

                WeakReferenceMessenger.Default.Send(new MyMessage("VolverAJugadores"));
            }
            catch (Exception ex)
            {
                // Manejo de errores, puedes también mostrar un mensaje al usuario si es necesario
                Console.WriteLine($"Error al guardar jugador: {ex.Message}");
                // Opcional: Mostrar alerta en la UI sobre el error
            }
        }

        private void Cancelar()
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("VolverAJugadores"));
        }
    }
}
