using FutbolNet.Modelos;
using FutbolNet.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using FutbolNet.Class;

namespace FutbolNet.ViewModels
{
    public class AddEditEquipoViewModel : ObservableObject
    {
        // Servicios
        private readonly GenericService<Equipo> equipoService = new GenericService<Equipo>();
        private readonly GenericService<Entrenador> entrenadorService = new GenericService<Entrenador>();

        // Propiedades de Equipo y Entrenador
        private Equipo equipo;
        public Equipo Equipo
        {
            get => equipo;
            set
            {
                equipo = value ?? new Equipo();
                OnPropertyChanged();
            }
        }

        private Entrenador entrenador;
        public Entrenador Entrenador
        {
            get => entrenador;
            set
            {
                entrenador = value ?? new Entrenador();
                OnPropertyChanged();
            }
        }

        // Propiedades de lista de Entrenadores
        private ObservableCollection<Entrenador> entrenadores;
        public ObservableCollection<Entrenador> Entrenadores
        {
            get => entrenadores;
            set
            {
                entrenadores = value;
                OnPropertyChanged();
            }
        }

        // Comandos
        public IAsyncRelayCommand GuardarCommand { get; }
        public IRelayCommand CancelarCommand { get; }

        // Constructor para creación de nuevo equipo
        public AddEditEquipoViewModel()
        {
            Equipo = new Equipo();
            Entrenador = new Entrenador();

            ObtenerEntrenadores(); // Cargar lista de entrenadores

            GuardarCommand = new AsyncRelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        // Constructor para editar equipo existente
        public AddEditEquipoViewModel(int equipoId)
        {
            GuardarCommand = new AsyncRelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);

            ObtenerEntrenadores(); // Cargar lista de entrenadores
            CargarDatosEquipo(equipoId); // Carga los datos cuando se pasa el equipoId
        }

        // Método para cargar la lista de entrenadores
        private async void ObtenerEntrenadores()
        {
            try
            {
                var entrenadoresLista = await entrenadorService.GetAllAsync();
                Entrenadores = new ObservableCollection<Entrenador>(entrenadoresLista);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar la lista de entrenadores: {ex.Message}");
            }
        }

        // Método para cargar los datos del equipo
        private async Task CargarDatosEquipo(int equipoId)
        {
            if (equipoId > 0)
            {
                try
                {
                    Equipo = await equipoService.GetByIdAsync(equipoId);

                    if (Equipo == null)
                    {
                        Console.WriteLine($"No se encontró el equipo con Id {equipoId}");
                    }
                    else
                    {
                        Entrenador = await entrenadorService.GetByIdAsync(Equipo.EntrenadorId);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar los datos del equipo: {ex.Message}");
                }
            }
        }

        // Método para guardar el equipo
        private async Task Guardar()
        {
            if (Equipo == null || Entrenador == null)
            {
                Console.WriteLine("Faltan datos para completar el equipo.");
                return;
            }

            try
            {
                Equipo.EntrenadorId = Entrenador.Id;

                if (Equipo.Id == 0)
                {
                    // El equipo es nuevo, lo agregamos
                    await equipoService.AddAsync(Equipo);
                }
                else
                {
                    // El equipo ya existe, lo actualizamos
                    await equipoService.UpdateAsync(Equipo);
                }

                // Enviar mensaje para actualizar la lista de equipos
                WeakReferenceMessenger.Default.Send(new MyMessage("VolverAEquipos"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el equipo: {ex.Message}");
            }
        }

        // Método para cancelar la operación
        private void Cancelar()
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("VolverAEquipos"));
        }
    }
}
