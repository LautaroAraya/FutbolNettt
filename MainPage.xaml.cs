using CommunityToolkit.Mvvm.Messaging;
using FutbolNet.Class;
using FutbolNet.ViewModels;
using FutbolNet.Views.Entrenadores;
using FutbolNet.Views.Equipos;
using FutbolNet.Views.Jugadores;
using FutbolNet.Views.Ligas;
using FutbolNet.Views.Partidos;

namespace FutbolNet
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            //código para preparar la recepción de mensajes y la llamada al método RecibirMensaje
            WeakReferenceMessenger.Default.Register<MyMessage>(this, (r, m) =>
            {
                AlRecibirMensaje(m);
            });
        }

        private async void AlRecibirMensaje(MyMessage m)
        {
            if (m.Value == "AbrirAddEditJugadorView")
            {
                await Navigation.PushAsync(new AddEditJugadorView(m.Jugador));
            }
            else if (m.Value == "AbrirJugadores")
            {
                await Navigation.PushAsync(new JugadorView());
            }
            else if (m.Value == "VolverAJugadores")
            {
                await Navigation.PopAsync();
            }
            if (m.Value == "AbrirAddEditLigaView")
            {
                await Navigation.PushAsync(new AddEditLigaView(m.Liga));
            }
            else if (m.Value == "AbrirLigas")
            {
                await Navigation.PushAsync(new LigaView());
            }
            else if (m.Value == "VolverALigas")
            {
                await Navigation.PopAsync();
            }

            if(m.Value == "AbrirAddEditJugadorView")
            {
                await Navigation.PushAsync(new AddEditJugadorView(m.Jugador));
            }
            else if (m.Value == "AbrirJugadores")
            {
                await Navigation.PushAsync(new JugadorView());
            }
            else if (m.Value == "VolverAJugadores")
            {
                await Navigation.PopAsync();
            }

            if(m.Value == "AbrirAddEditEntrenadorView")
            {
                await Navigation.PushAsync(new AddEditEntrenadorView(m.Entrenador));
            }
            else if (m.Value == "AbrirEntrenadores")
            {
                await Navigation.PushAsync(new EntrenadorView());
            }
            else if (m.Value == "VolverAEntrenadores")
            {
                await Navigation.PopAsync();
            }
            else if(m.Value== "VolverAInicio")
            {
                await Navigation.PopToRootAsync();
            }

            if(m.Value == "AbrirAddEditEquipoView")
            {
                await Navigation.PushAsync(new AddEditEquipoView(m.Equipo));
            }
            else if (m.Value == "AbrirEquipos")
            {
                await Navigation.PushAsync(new EquipoView());
            }
            else if (m.Value == "VolverAEquipos")
            {
                await Navigation.PopAsync();
            }

            if (m.Value == "AbrirAddEditPartidoView")
            {
                await Navigation.PushAsync(new AddEditPartidoView(m.Partido));
            }
            else if (m.Value == "AbrirPartidos")
            {
                await Navigation.PushAsync(new PartidoView());
            }
            else if (m.Value == "VolverAPartidos")
            {
                await Navigation.PopAsync();
            }
        }

        private async void JugadorBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new JugadorView());


        }

        private async void LigaBtn_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LigaView());

        }

        private async void EntrenadorBtn_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EntrenadorView());
        }

        private async void EquipoBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EquipoView());
        }

        private async void PartidoBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PartidoView());
        }
    }
}


