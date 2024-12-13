using FutbolNet.Modelos;
using FutbolNet.ViewModels;

namespace FutbolNet.Views.Partidos;
public partial class AddEditPartidoView : ContentPage
{
    private AddEditPartidoViewModel viewModel;
    public AddEditPartidoView()
	{
		InitializeComponent();
        viewModel = this.BindingContext as AddEditPartidoViewModel;
    }
    public AddEditPartidoView(Partido partido)
    {
        InitializeComponent();
        viewModel = this.BindingContext as AddEditPartidoViewModel;
        if (viewModel != null)
        {
            viewModel.Partido = partido;
        }
    }
}