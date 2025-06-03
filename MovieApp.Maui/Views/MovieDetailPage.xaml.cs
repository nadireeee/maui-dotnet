using System;
using Microsoft.Maui.Controls;
using MovieApp.Maui.ViewModels;

namespace MovieApp.Maui.Views;

public partial class MovieDetailPage : ContentPage, IQueryAttributable
{
    private readonly MovieDetailViewModel _viewModel;

    public MovieDetailPage(MovieDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("movieId", out var movieIdObj) && movieIdObj is string movieIdStr && int.TryParse(movieIdStr, out var movieId))
        {
            await _viewModel.LoadMovieAsync(movieId);
        }
    }
} 