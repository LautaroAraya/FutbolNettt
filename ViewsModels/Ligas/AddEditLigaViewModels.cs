using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using FutbolNet.Class;
using FutbolNet.Modelos;
using FutbolNet.Service;

namespace FutbolNet.ViewModels
{
    public class AddEditLigaViewModel : ObservableObject
    {
        private readonly GenericService<Liga> ligaService = new GenericService<Liga>();

        private Liga liga;
        public Liga Liga
        {
            get => liga;
            set
            {
                liga = value ?? new Liga(); // Asigna una nueva Liga si el valor es null
                OnPropertyChanged();
            }
        }

        public AddEditLigaViewModel()
        {
            Liga = new Liga(); // Asegura que Liga esté inicializada al inicio
            GuardarCommand = new AsyncRelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        public IAsyncRelayCommand GuardarCommand { get; }
        public IRelayCommand CancelarCommand { get; }

        private async Task Guardar()
        {
            if (Liga == null)
            {
                Liga = new Liga(); // Crea una nueva instancia de Liga si es null
            }

            try
            {
                // Verifica si es una liga nueva o existente
                if (Liga.Id == 0)
                {
                    await ligaService.AddAsync(Liga); // Agrega la liga nueva
                }
                else
                {
                    await ligaService.UpdateAsync(Liga); // Actualiza la liga existente
                }

                WeakReferenceMessenger.Default.Send(new MyMessage("VolverALigas"));
            }
            catch (Exception ex)
            {
                // Manejo de errores, puedes también mostrar un mensaje al usuario si es necesario
                Console.WriteLine($"Error al guardar liga: {ex.Message}");
                // Opcional: Mostrar alerta en la UI sobre el error
            }
        }

        private void Cancelar()
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("VolverALigas"));
        }
    }
}
