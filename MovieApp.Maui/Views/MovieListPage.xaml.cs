using System;
using Microsoft.Maui.Controls;
using MovieApp.Maui.ViewModels;

namespace MovieApp.Maui.Views;

public partial class MovieListPage : ContentPage
{
    public MovieListPage(MovieListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnProfileBarClicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet("Profil", "İptal", null, "Favori Filmlerim");
        if (action == "Favori Filmlerim")
        {
            await Shell.Current.GoToAsync("///FavoriteMoviesPage");
        }
    }
} 