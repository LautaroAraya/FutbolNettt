using FutbolNet.Modelos;
using FutbolNet.ViewModels;

namespace FutbolNet.Views.Ligas;

public partial class AddEditLigaView : ContentPage
{
	private AddEditLigaViewModel viewModel;
    public AddEditLigaView()
	{
		InitializeComponent();
        viewModel = this.BindingContext as AddEditLigaViewModel;
    }

    public AddEditLigaView(Liga liga)
    {
        InitializeComponent();
        viewModel = this.BindingContext as AddEditLigaViewModel;
        if (viewModel != null)
        {
            viewModel.Liga = liga;
        }
    }
}