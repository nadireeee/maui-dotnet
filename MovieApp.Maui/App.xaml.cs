namespace MovieApp.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        // İkisini aynı anda kullanmana gerek yok, sadece biri yeterli
        // MainPage = new AppShell(); // ← Bu satırı silebiliriz, çünkü CreateWindow override ediyoruz
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window
        {
            Title = "Film Uygulaması",
            Page = new AppShell()
        };
    }
}
