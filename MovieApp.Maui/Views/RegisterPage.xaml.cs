using MovieApp.Maui.ViewModels;

namespace MovieApp.Maui.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
} 