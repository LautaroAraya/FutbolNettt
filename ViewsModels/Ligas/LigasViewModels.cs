using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using FutbolNet.Class;
using FutbolNet.Modelos;
using FutbolNet.Service;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FutbolNet.ViewModels
{
    public class LigasViewModel : NotificationObject
    {
        GenericService<Liga> ligaService = new GenericService<Liga>();

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

        private ObservableCollection<Liga> ligas;
        public ObservableCollection<Liga> Ligas
        {
            get { return ligas; }
            set
            {
                ligas = value;
                OnPropertyChanged();
            }
        }

        private Liga ligaCurrent;
        public Liga LigaCurrent
        {
            get { return ligaCurrent; }
            set
            {
                ligaCurrent = value;
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

        public LigasViewModel()
        {
            AgregarCommand = new Command(Agregar);
            EditarCommand = new Command(Editar, PermitirEditar);
            EliminarCommand = new Command(Eliminar, PermitirEliminar);

            Task.Run(async () => await ObtenerLigas());

            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                AlRecibirMensaje(m);
            });
        }

        private async void AlRecibirMensaje(MyMessage m)
        {
            if (m.Value == "VolverALigas")
            {
                await RefreshLigas(this);
            }
        }

        private async Task RefreshLigas(object obj)
        {
            IsRefreshing = true;
            await ObtenerLigas();
            IsRefreshing = false;
        }

        private bool PermitirEliminar(object arg)
        {
            return LigaCurrent != null;
        }

        private async void Eliminar(object obj)
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert(
                "Eliminar una liga",
                $"Está seguro que desea eliminar la liga {LigaCurrent.Nombre}?",
                "Si",
                "No"
            );
            if (respuesta)
            {
                ActivityStart = true;
                await ligaService.DeleteAsync(LigaCurrent.Id);
                await ObtenerLigas();
            }
        }

        private bool PermitirEditar(object arg)
        {
            return LigaCurrent != null;
        }

        private void Editar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditLigaView") { Liga = LigaCurrent });
        }

        private void Agregar(object obj)
        {
            WeakReferenceMessenger.Default.Send(new MyMessage("AbrirAddEditLigaView"));
        }

        public async Task ObtenerLigas()
        {
            ActivityStart = true;
            var ligas = await ligaService.GetAllAsync();
            Ligas = new ObservableCollection<Liga>(ligas);
            ActivityStart = false;
        }
    }
}
