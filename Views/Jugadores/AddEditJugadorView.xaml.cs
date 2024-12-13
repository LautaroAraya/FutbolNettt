using FutbolNet.ViewModels;
using FutbolNet.Modelos;

namespace FutbolNet.Views.Jugadores;

public partial class AddEditJugadorView : ContentPage
{
    private AddEditJugadorViewModel viewModel;

    public AddEditJugadorView()
    {
        InitializeComponent();
        viewModel = this.BindingContext as AddEditJugadorViewModel;
    }

    public AddEditJugadorView(Jugador jugador)
    {
        InitializeComponent();
        viewModel = this.BindingContext as AddEditJugadorViewModel;
        if (viewModel != null)
        {
            viewModel.Jugador = jugador;
        }
    }
}
