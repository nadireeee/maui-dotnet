using System;
using Microsoft.Maui.Controls;
using MovieApp.Maui.ViewModels;

namespace MovieApp.Maui.Views;

public partial class FavoriteMoviesPage : ContentPage
{
    private readonly FavoriteMoviesViewModel _viewModel;

    public FavoriteMoviesPage(FavoriteMoviesViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    public static class ServiceHelper
    {
        public static T GetService<T>() => (T)App.Current.Handler.MauiContext.Services.GetService(typeof(T));
    }

    public FavoriteMoviesPage() : this(ServiceHelper.GetService<FavoriteMoviesViewModel>())
    {
    }

    private void OnCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        _viewModel.UpdateHasSelectedMovies();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadFavoriteMoviesCommand.Execute(null);
    }
} 