using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using FutbolNet.Class;
using FutbolNet.Modelos;
using FutbolNet.Service;

namespace FutbolNet.ViewModels
{
    public class AddEditEntrenadorViewModel : ObservableObject
    {
        private readonly GenericService<Entrenador> entrenadorService = new GenericService<Entrenador>();

        private Entrenador entrenador;
        public Entrenador Entrenador
        {
            get => entrenador;
            set
            {
                entrenador = value ?? new Entrenador(); // Asigna un nuevo Entrenador si el valor es null
                OnPropertyChanged();
            }
        }

        public AddEditEntrenadorViewModel()
        {
            Entrenador = new Entrenador(); // Asegura que Entrenador esté inicializado al inicio
            GuardarCommand = new AsyncRelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        public IAsyncRelayCommand GuardarCommand { get; }
        public IRelayCommand CancelarCommand { get; }

        private async Task Guardar()
        {
            if (Entrenador == null)
            {
                Entrenador = new Entrenador(); // Crea una nueva instancia de Entrenador si es null
            }

            try
            {
                // Verifica si es un entrenador nuevo o existente
                if (Entrenador.Id == 0)
                {
                    await entrenadorService.AddAsync(Entrenador); // Agrega el entrenador nuevo
                }
                else
                {
                    await entrenadorService.UpdateAsync(Entrenador); // Actualiza el entrenador existente
                }

                WeakReferenceMessenger.Default.Send(new MyMessage("VolverAEntrenadores"));
            }
            catch (Exception ex)
            {
                // Manejo de errores, puedes también mostrar un mensaje al usuario si es necesario
                Console.WriteLine($"Error al guardar entrenador: {ex.Message}");
                // Opcional: Mostrar alerta en la UI sobre el error
            }
        }

        private void Cancelar()
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("VolverAEntrenadores"));
        }
    }
}
