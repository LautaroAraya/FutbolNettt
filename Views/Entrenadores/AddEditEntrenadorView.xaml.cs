using FutbolNet.Modelos;
using FutbolNet.ViewModels;

namespace FutbolNet.Views.Entrenadores;

public partial class AddEditEntrenadorView : ContentPage
{
    private AddEditEntrenadorViewModel viewModel;
    public AddEditEntrenadorView()
    {
        InitializeComponent();
        viewModel = this.BindingContext as AddEditEntrenadorViewModel;
    }

    public AddEditEntrenadorView(Entrenador entrenador)
    {
        InitializeComponent();
        viewModel = this.BindingContext as AddEditEntrenadorViewModel;
        if (viewModel != null)
        {
            viewModel.Entrenador = entrenador;
        }
    }
}