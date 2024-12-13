using FutbolNet.Modelos;
using FutbolNet.ViewModels;

namespace FutbolNet.Views.Equipos;

public partial class AddEditEquipoView : ContentPage
{
    private AddEditEquipoViewModel viewModel;

    public AddEditEquipoView()
	{
		InitializeComponent();
        viewModel = this.BindingContext as AddEditEquipoViewModel;
    }

    public AddEditEquipoView(Equipo equipo)
    {
        InitializeComponent();
        viewModel = this.BindingContext as AddEditEquipoViewModel;
        if (viewModel != null)
        {
            viewModel.Equipo = equipo;
        }
    }
}