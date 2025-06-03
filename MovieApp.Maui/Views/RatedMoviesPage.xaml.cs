using MovieApp.Maui.ViewModels;
using MovieApp.Maui.Services;

namespace MovieApp.Maui.Views;

public partial class RatedMoviesPage : ContentPage
{
    private readonly RatedMoviesViewModel _viewModel;

    public RatedMoviesPage(RatedMoviesViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    // Eğer DI yoksa, aşağıdaki constructor kullanılabilir:
    public RatedMoviesPage() : this(new RatedMoviesViewModel(new ApiService())) { }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadRatedMoviesCommand.Execute(null);
    }
} 