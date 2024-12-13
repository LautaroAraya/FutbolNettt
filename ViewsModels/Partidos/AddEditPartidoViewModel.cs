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
    public class AddEditPartidoViewModel : ObservableObject
    {
        // Servicios
        private readonly GenericService<Partido> partidoService = new GenericService<Partido>();
        private readonly GenericService<Equipo> equipoService = new GenericService<Equipo>();

        // Propiedades de Partido y Equipos
        private Partido partido;
        public Partido Partido
        {
            get => partido;
            set
            {
                partido = value ?? new Partido();
                OnPropertyChanged();
            }
        }

        private Equipo equipoLocal;
        public Equipo EquipoLocal
        {
            get => equipoLocal;
            set
            {
                equipoLocal = value ?? new Equipo();
                OnPropertyChanged();
            }
        }

        private Equipo equipoVisitante;
        public Equipo EquipoVisitante
        {
            get => equipoVisitante;
            set
            {
                equipoVisitante = value ?? new Equipo();
                OnPropertyChanged();
            }
        }

        // Propiedades de lista de Equipos
        private ObservableCollection<Equipo> equipos;
        public ObservableCollection<Equipo> Equipos
        {
            get => equipos;
            set
            {
                equipos = value;
                OnPropertyChanged();
            }
        }

        // Comandos
        public IAsyncRelayCommand GuardarCommand { get; }
        public IRelayCommand CancelarCommand { get; }

        // Constructor para creación de nuevo partido
        public AddEditPartidoViewModel()
        {
            Partido = new Partido
            {
                Fecha = DateTime.Now // Asigna la fecha actual (sin hora)
            };
            EquipoLocal = new Equipo();
            EquipoVisitante = new Equipo();

            ObtenerEquipos(); // Cargar lista de equipos

            GuardarCommand = new AsyncRelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        // Constructor para editar partido existente
        public AddEditPartidoViewModel(int partidoId)
        {
            GuardarCommand = new AsyncRelayCommand(Guardar);
            CancelarCommand = new RelayCommand(Cancelar);

            ObtenerEquipos(); // Cargar lista de equipos
            CargarDatosPartido(partidoId); // Cargar los datos cuando se pasa el partidoId
        }

        // Método para cargar la lista de equipos
        private async void ObtenerEquipos()
        {
            try
            {
                var equiposLista = await equipoService.GetAllAsync();
                Equipos = new ObservableCollection<Equipo>(equiposLista);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar la lista de equipos: {ex.Message}");
            }
        }

        // Método para cargar los datos del partido
        private async Task CargarDatosPartido(int partidoId)
        {
            if (partidoId > 0)
            {
                try
                {
                    Partido = await partidoService.GetByIdAsync(partidoId);

                    if (Partido == null)
                    {
                        Console.WriteLine($"No se encontró el partido con Id {partidoId}");
                        Partido = new Partido { Fecha = DateTime.Now }; // Asigna la fecha actual si no se encuentra el partido
                    }
                    else
                    {
                        EquipoLocal = await equipoService.GetByIdAsync(Partido.EquipoLocalId) ?? new Equipo();
                        EquipoVisitante = await equipoService.GetByIdAsync(Partido.EquipoVisitanteId) ?? new Equipo();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar los datos del partido: {ex.Message}");
                }
            }
        }

        // Método para guardar el partido
        private async Task Guardar()
        {
            if (Partido == null || EquipoLocal == null || EquipoVisitante == null)
            {
                Console.WriteLine("Faltan datos para completar el partido.");
                return;
            }

            if (EquipoLocal.Id == 0 || EquipoVisitante.Id == 0)
            {
                Console.WriteLine("El equipo local o visitante no tiene un Id válido.");
                return;
            }

            try
            {
                Partido.EquipoLocalId = EquipoLocal.Id;
                Partido.EquipoVisitanteId = EquipoVisitante.Id;
                Partido.Fecha = Partido.Fecha.Date; // Solo la fecha, sin la hora

                if (Partido.Id == 0)
                {
                    // El partido es nuevo, lo agregamos
                    await partidoService.AddAsync(Partido);
                }
                else
                {
                    // El partido ya existe, lo actualizamos
                    await partidoService.UpdateAsync(Partido);
                }

                // Enviar mensaje para actualizar la lista de partidos
                WeakReferenceMessenger.Default.Send(new MyMessage("VolverAPartidos"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el partido: {ex.Message}");
            }
        }

        // Método para cancelar la operación
        private void Cancelar()
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("VolverAPartidos"));
        }
    }
}
