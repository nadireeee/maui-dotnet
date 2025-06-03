using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MovieApp.Maui.Models;
using MovieApp.Maui.Services;

namespace MovieApp.Maui.ViewModels;

public class MovieListViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private ObservableCollection<Movie> _movies = new();
    private bool _isBusy;
    private string _searchText = string.Empty;
    private string _errorMessage = string.Empty;
    private Movie? _selectedMovie;
    private bool _isRefreshing;
    private bool _isAllSelected = true;
    private bool _isTopRatedSelected;
    private bool _isNewestSelected;
    private bool _isPopularSelected;
    private bool _isDramaSelected;
    private bool _isActionSelected;
    private ObservableCollection<Movie> _allMovies = new();
    private string _selectedFilter = "All";
    private List<string> _filterOptions = new() { "All", "Top Rated", "Newest", "Popular", "Drama" };
    private bool _isFilterActive;
    private bool _isSearching;
    public List<string> FilterOptions => _filterOptions;
    public event PropertyChangedEventHandler? PropertyChanged;

    public MovieListViewModel(ApiService apiService)
    {
        _apiService = apiService;
        MovieSelectedCommand = new Command<Movie>(OnMovieSelected);
        LoadMoviesCommand = new Command(async () => await LoadMoviesAsync());
        SearchCommand = new Command(async () => await SearchMoviesAsync());
        FilterCommand = new Command<string>(OnFilterSelected);
        ShowFilterCommand = new Command(OnShowFilter);
        RefreshCommand = new Command(async () => await RefreshAsync());
        ClearSearchCommand = new Command(OnClearSearch);
        LoadMoviesCommand.Execute(null);
    }

    public ObservableCollection<Movie> Movies
    {
        get => _movies;
        set
        {
            if (_movies != value)
            {
                _movies = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (_isBusy != value)
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (_errorMessage != value)
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }
    }

    public Movie? SelectedMovie
    {
        get => _selectedMovie;
        set
        {
            if (_selectedMovie != value)
            {
                _selectedMovie = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set
        {
            if (_isRefreshing != value)
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsAllSelected
    {
        get => _isAllSelected;
        set
        {
            if (_isAllSelected != value)
            {
                _isAllSelected = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsTopRatedSelected
    {
        get => _isTopRatedSelected;
        set
        {
            if (_isTopRatedSelected != value)
            {
                _isTopRatedSelected = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsNewestSelected
    {
        get => _isNewestSelected;
        set
        {
            if (_isNewestSelected != value)
            {
                _isNewestSelected = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsPopularSelected
    {
        get => _isPopularSelected;
        set
        {
            if (_isPopularSelected != value)
            {
                _isPopularSelected = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsDramaSelected
    {
        get => _isDramaSelected;
        set
        {
            if (_isDramaSelected != value)
            {
                _isDramaSelected = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsActionSelected
    {
        get => _isActionSelected;
        set
        {
            if (_isActionSelected != value)
            {
                _isActionSelected = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand MovieSelectedCommand { get; }
    public ICommand LoadMoviesCommand { get; }
    public Command SearchCommand { get; }
    public Command FilterCommand { get; }
    public ICommand ShowFilterCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ClearSearchCommand { get; }

    public string SelectedFilter
    {
        get => _selectedFilter;
        set
        {
            if (_selectedFilter != value)
            {
                _selectedFilter = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }
    }

    public bool IsFilterActive
    {
        get => _isFilterActive;
        set
        {
            if (_isFilterActive != value)
            {
                _isFilterActive = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsSearching
    {
        get => _isSearching;
        set
        {
            if (_isSearching != value)
            {
                _isSearching = value;
                OnPropertyChanged();
            }
        }
    }

    private async Task LoadMoviesAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            System.Diagnostics.Debug.WriteLine($"[MovieListViewModel] API'ye film listesi isteği atılıyor: {SelectedFilter}");
            List<Movie> movies = new();
            switch (SelectedFilter)
            {
                case "Top Rated":
                    movies = await _apiService.GetTopRatedMoviesAsync(10);
                    break;
                case "Newest":
                    movies = await _apiService.GetNewestMoviesAsync(10);
                    break;
                case "Popular":
                    movies = await _apiService.GetPopularMoviesAsync(10);
                    break;
                case "Drama":
                    movies = await _apiService.GetMoviesByGenreAsync(SelectedFilter);
                    break;
                default:
                    movies = await _apiService.GetMoviesAsync();
                    break;
            }
            System.Diagnostics.Debug.WriteLine($"[MovieListViewModel] API'den {movies.Count} film geldi.");
            _allMovies = new ObservableCollection<Movie>(movies);
            Movies = new ObservableCollection<Movie>(movies);
        }
        catch (Exception ex)
        {
            ErrorMessage = "Filmler yüklenirken bir hata oluştu.";
            System.Diagnostics.Debug.WriteLine($"[MovieListViewModel] Film listesi yüklenirken hata: {ex.Message}\n{ex.StackTrace}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SearchMoviesAsync()
    {
        if (IsBusy || string.IsNullOrWhiteSpace(SearchText))
            return;

        try
        {
            IsBusy = true;
            IsSearching = true;
            ErrorMessage = string.Empty;
            System.Diagnostics.Debug.WriteLine($"[MovieListViewModel] Arama başlatıldı: '{SearchText}'");
            var movies = await _apiService.GetMoviesAsync();
            var filteredMovies = movies.Where(m =>
                (m.Title ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                (m.Overview ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                (m.Director ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                (m.Cast ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                (m.Genres ?? "").Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
            System.Diagnostics.Debug.WriteLine($"[MovieListViewModel] Arama sonucu: {filteredMovies.Count} film bulundu.");
            Movies = new ObservableCollection<Movie>(filteredMovies);
        }
        catch (Exception ex)
        {
            ErrorMessage = "Filmler aranırken bir hata oluştu.";
            System.Diagnostics.Debug.WriteLine($"[MovieListViewModel] Arama sırasında hata: {ex.Message}\n{ex.StackTrace}");
        }
        finally
        {
            IsBusy = false;
            IsSearching = false;
        }
    }

    private async void OnMovieSelected(Movie? movie)
    {
        if (movie == null)
        {
            System.Diagnostics.Debug.WriteLine("[MovieListViewModel] Film seçimi null, navigasyon yapılmayacak.");
            return;
        }
        System.Diagnostics.Debug.WriteLine($"[MovieListViewModel] Film seçildi: {movie.Title} (ID: {movie.Id})");
        var route = $"///MovieDetailPage?movieId={movie.Id}";
        System.Diagnostics.Debug.WriteLine($"[MovieListViewModel] Navigasyon başlatılıyor: {route}");
        await Shell.Current.GoToAsync(route);
    }

    private void OnFilterSelected(string filter)
    {
        // Tüm seçimleri sıfırla
        IsAllSelected = false;
        IsTopRatedSelected = false;
        IsNewestSelected = false;
        IsPopularSelected = false;
        IsDramaSelected = false;

        // Seçilen filtreyi işaretle
        switch (filter)
        {
            case "All":
                IsAllSelected = true;
                break;
            case "Top Rated":
                IsTopRatedSelected = true;
                break;
            case "Newest":
                IsNewestSelected = true;
                break;
            case "Popular":
                IsPopularSelected = true;
                break;
            case "Drama":
                IsDramaSelected = true;
                break;
        }

        SelectedFilter = filter;
        LoadMoviesCommand.Execute(null);
    }

    private void OnShowFilter()
    {
        IsFilterActive = !IsFilterActive;
        // TODO: Filtre menüsü implementasyonu
    }

    private async Task RefreshAsync()
    {
        await LoadMoviesAsync();
        IsRefreshing = false;
    }

    private void ApplyFilter()
    {
        System.Diagnostics.Debug.WriteLine($"[MovieListViewModel] Filtre uygulanıyor: {SelectedFilter}");
        if (_allMovies == null || !_allMovies.Any())
        {
            Movies = new ObservableCollection<Movie>();
            return;
        }
        IEnumerable<Movie> filtered = _allMovies;
        switch (SelectedFilter)
        {
            case "All":
                filtered = _allMovies;
                break;
            case "Action":
                filtered = _allMovies.Where(m => m.Genres.Contains("Action", StringComparison.OrdinalIgnoreCase) || m.Genres.Contains("Aksiyon", StringComparison.OrdinalIgnoreCase));
                break;
            case "Comedy":
                filtered = _allMovies.Where(m => m.Genres.Contains("Comedy", StringComparison.OrdinalIgnoreCase) || m.Genres.Contains("Komedi", StringComparison.OrdinalIgnoreCase));
                break;
            case "Drama":
                filtered = _allMovies.Where(m => m.Genres.Contains("Drama", StringComparison.OrdinalIgnoreCase) || m.Genres.Contains("Dram", StringComparison.OrdinalIgnoreCase));
                break;
            case "Horror":
                filtered = _allMovies.Where(m => m.Genres.Contains("Horror", StringComparison.OrdinalIgnoreCase) || m.Genres.Contains("Korku", StringComparison.OrdinalIgnoreCase));
                break;
            case "Romance":
                filtered = _allMovies.Where(m => m.Genres.Contains("Romance", StringComparison.OrdinalIgnoreCase) || m.Genres.Contains("Romantik", StringComparison.OrdinalIgnoreCase));
                break;
            case "Newest":
                filtered = _allMovies.OrderByDescending(m => m.ReleaseDate).Take(20);
                break;
            case "Popular":
                filtered = _allMovies.OrderByDescending(m => m.Popularity).Take(20);
                break;
            case "TopRated":
                filtered = _allMovies.OrderByDescending(m => m.VoteAverage).Take(20);
                break;
        }
        Movies = new ObservableCollection<Movie>(filtered);
    }

    private void OnClearSearch()
    {
        SearchText = string.Empty;
        LoadMoviesCommand.Execute(null);
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
} 