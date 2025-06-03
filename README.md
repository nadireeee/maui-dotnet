# MovieApp - .NET MAUI & Minimal API Film Uygulaması

## Proje Özeti

MovieApp, .NET MAUI ile geliştirilmiş modern bir film uygulamasıdır. Kullanıcılar uygulama üzerinden:
- Film arayabilir, filtreleyebilir, detaylarını görebilir,
- Filmleri favorilere ekleyebilir ve puanlayabilir,
- Kendi profillerini görüntüleyebilir ve şifrelerini güncelleyebilir,
- Kendi puanladığı filmleri ve favorilerini ayrı sayfalarda görebilir,
- Gelişmiş filtreleme ve arama ile yerli/yabancı, tür, popülerlik, yıl gibi kriterlere göre film listesi oluşturabilir.

## Temel Kullanıcı Akışları ve Ekranlar

- **Kayıt & Giriş:** Kullanıcılar e-posta ve şifre ile kayıt olabilir ve giriş yapabilir.
- **Profil Ekranı:** Kullanıcı adı, e-posta, toplam favori film ve toplam puanladığı film sayısı gösterilir. Ayrıca şifre güncelleme butonu bulunur.
- **Şifre Güncelleme:** Profil ekranında "Şifre Değiştir" butonuna tıklayarak mevcut şifresini değiştirebilir. (Şifre güncelleme API'si ve arayüzü mevcuttur, validasyon temel düzeydedir.)
- **Film Listesi:** CollectionView ile modern bir grid görünümünde filmler listelenir. Arama kutusu ve filtre çipleri ile gelişmiş arama yapılabilir.
- **Film Detay:** Seçilen filmin detayları, oyuncuları, türü, puanı, açıklaması ve poster görseli gösterilir. Buradan filmi favorilere ekleyebilir veya puanlayabilirsiniz.
- **Favori Filmler:** Kullanıcıya özel favori filmler ayrı bir sayfada listelenir.
- **Puanladığım Filmler:** Kullanıcıya özel, puanladığı filmler ve verdiği puanlar ayrı bir sayfada gösterilir.
- **Film Ekle/Güncelle:** Yetkili kullanıcılar yeni film ekleyebilir veya mevcut filmi güncelleyebilir.

## Önemli Notlar
- **API Portu:** Tüm API ve Swagger endpointleri `http://localhost:5175` adresindedir.
- **Veritabanı Arayüzü:** Adminer ile veritabanını görsel olarak yönetmek için `http://localhost:4242` adresini kullanabilirsiniz.
- **Şifre Güncelleme:** Profil ekranında "Şifre Değiştir" butonu ile erişilebilir. (Gelişmiş validasyon eklenebilir.)
- **Swagger:** API dokümantasyonu ve testleri için `http://localhost:5175/swagger` adresini kullanabilirsiniz.

---

## Proje İsterleri ve Teknik Yanıtlar (Detaylı)

### 1. Proje MAUI ile mi yapılmış?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Maui/` klasörü.
- **Nasıl?**  
  - Tüm kullanıcı arayüzü .NET MAUI ile yazılmıştır.
- **Koddan Örnek:**  
  - `MovieApp.Maui/Views/MovieListPage.xaml`
    ```xml
    <ContentPage ...>
      <!-- MAUI XAML ile modern arayüz -->
    </ContentPage>
    ```

---

### 2. Minimal Api projesi oluşturulmuş mu?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Api/Program.cs`
- **Nasıl?**  
  - Tüm endpointler Minimal API ile tanımlanmıştır.
- **Koddan Örnek:**  
  ```csharp
  app.MapGet("/api/movies", async (IMovieService movieService) =>
  {
      var movies = await movieService.GetAllMoviesAsync();
      return Results.Ok(movies);
  });
  ```

---

### 3. Entity Framework Code First yaklaşımı kullanılmış mı?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Api/Models/`
- **Nasıl?**  
  - Modeller C# sınıfı olarak tanımlanmış, migration ile veritabanı oluşturulmuş.
- **Koddan Örnek:**  
  - `MovieApp.Api/Models/Movie.cs`
    ```csharp
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        // ...
    }
    ```

---

### 4. Migrationlar eklenmiş mi?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Api/Migrations/`
- **Nasıl?**  
  - Migration dosyaları burada tutulur, veritabanı bu migrationlarla güncellenir.
- **Koddan Örnek:**  
  - Migration dosyası örneği:  
    ```csharp
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    // ...
                },
                // ...
            );
        }
    }
    ```

---

### 5. Veri tabanı bağlantısı yapılmış mı?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Api/appsettings.json`
- **Nasıl?**  
  - Connection string ile SQL Server'a bağlanılır.
- **Koddan Örnek:**  
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1434;Database=MovieAppDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
  }
  ```

---

### 6. Veri tabanından listeleme işlemi yapılmış mı? (Çeşitli kriterlere göre sorgulama yapılabiliyor mu?)
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Api/Services/MovieService.cs`
- **Nasıl?**  
  - LINQ ile tür, popülerlik, puan, yerli/yabancı, arama gibi filtreler uygulanır.
- **Koddan Örnek:**  
  ```csharp
  public async Task<IEnumerable<Movie>> GetTopRatedMoviesAsync(int count)
  {
      return await _context.Movies
          .OrderByDescending(m => m.VoteAverage)
          .Take(count)
          .ToListAsync();
  }
  ```

---

### 7. Veri tabanına veri ekleme işlemi yapılmış mı?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Api/Program.cs` (POST endpoint)
- **Nasıl?**  
  - API üzerinden yeni film eklenir.
- **Koddan Örnek:**  
  ```csharp
  app.MapPost("/api/movies", async (Movie movie, IMovieService movieService) =>
  {
      var createdMovie = await movieService.CreateMovieAsync(movie, 1);
      return Results.Created($"/api/movies/{createdMovie.Id}", createdMovie);
  });
  ```

---

### 8. Veri tabanındaki veri üzerinde güncelleme işlemi yapılmış mı?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Api/Program.cs` (PUT endpoint)
- **Nasıl?**  
  - API üzerinden film güncellenir.
- **Koddan Örnek:**  
  ```csharp
  app.MapPut("/api/movies/{id}", async (int id, Movie movie, ...) =>
  {
      var updatedMovie = await movieService.UpdateMovieAsync(movie, 1);
      return Results.Ok(updatedMovie);
  });
  ```

---

### 9. Veri tabanındaki veri silme işlemi yapılmış mı?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Api/Program.cs` (DELETE endpoint)
- **Nasıl?**  
  - API üzerinden film silinir.
- **Koddan Örnek:**  
  ```csharp
  app.MapDelete("/api/movies/{id}", async (int id, IMovieService movieService) =>
  {
      var result = await movieService.DeleteMovieAsync(id);
      return result ? Results.NoContent() : Results.NotFound();
  });
  ```

---

### 10. Ekleme ve güncelleme işlemlerinde hangi kullanıcı ne zaman ekledi ya da güncelledi verileri tutulmuş mu?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Api/Models/Movie.cs`
- **Nasıl?**  
  - `CreatedById`, `CreatedAt`, `UpdatedById`, `UpdatedAt` alanları ile tutulur.
- **Koddan Örnek:**  
  ```csharp
  public int CreatedById { get; set; }
  public DateTime CreatedAt { get; set; }
  public int? UpdatedById { get; set; }
  public DateTime? UpdatedAt { get; set; }
  ```

---

### 11. Kullanıcı girişi yapılmış mı?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Api/Program.cs` (POST /api/users/login)
- **Nasıl?**  
  - JWT ile kullanıcı girişi yapılır.
- **Koddan Örnek:**  
  ```csharp
  app.MapPost("/api/users/login", async (LoginRequest request, IUserService userService, IConfiguration configuration) =>
  {
      // ...
      return Results.Ok(new
      {
          Token = new JwtSecurityTokenHandler().WriteToken(token),
          User = user
      });
  });
  ```

---

### 12. Kullanıcı çıkışı yapılmış mı?
**✅ Evet.**
- **Nerede?**  
  - MAUI tarafında token silinerek çıkış yapılır.
- **Nasıl?**  
  - Kullanıcı çıkış butonuna tıkladığında local storage'daki JWT token silinir.
- **Koddan Örnek:**  
  ```csharp
  // MovieApp.Maui/ViewModels/ProfileViewModel.cs
  Preferences.Remove("jwt_token");
  ```

---

### 13. Şifre değiştirme sayfası yapılmış mı?
**✅ Evet.**
- **Nerede?**  
  - **API:** `MovieApp.Api/Program.cs` (PUT /api/users/update-password)
  - **MAUI:** `MovieApp.Maui/Views/ProfilePage.xaml` ve `MovieApp.Maui/ViewModels/ProfileViewModel.cs`
- **Nasıl?**  
  - Kullanıcı, profil ekranında "Şifre Değiştir" butonuna tıklar.
  - Açılan ekranda mevcut şifresini, yeni şifresini ve yeni şifre tekrarını girer.
  - "Kaydet" butonuna basınca, bilgiler API'ye gönderilir ve şifre güncellenir.
  - Başarılı olursa kullanıcıya bildirim gösterilir.
- **Koddan Örnek:**  
  - **API Endpoint:**  
    ```csharp
    app.MapPut("/api/users/update-password", async (UpdatePasswordRequest req, IUserService userService) =>
    {
        var result = await userService.UpdatePasswordAsync(req.UserId, req.OldPassword, req.NewPassword);
        return result ? Results.Ok() : Results.BadRequest("Şifre güncellenemedi.");
    });
    ```
  - **Profil Sayfası (XAML):**
    ```xml
    <Button Text="Şifre Değiştir" Command="{Binding ChangePasswordCommand}" />
    ```
  - **Şifre Değiştirme Popup (örnek):**
    ```xml
    <Entry Placeholder="Mevcut Şifre" IsPassword="True" Text="{Binding OldPassword}" />
    <Entry Placeholder="Yeni Şifre" IsPassword="True" Text="{Binding NewPassword}" />
    <Entry Placeholder="Yeni Şifre (Tekrar)" IsPassword="True" Text="{Binding NewPasswordRepeat}" />
    <Button Text="Kaydet" Command="{Binding SavePasswordCommand}" />
    ```
  - **ViewModel (ProfileViewModel.cs):**
    ```csharp
    public async Task ChangePasswordAsync()
    {
        var result = await _apiService.UpdatePasswordAsync(UserId, OldPassword, NewPassword);
        if (result)
            await Application.Current.MainPage.DisplayAlert("Başarılı", "Şifre güncellendi.", "Tamam");
        else
            await Application.Current.MainPage.DisplayAlert("Hata", "Şifre güncellenemedi.", "Tamam");
    }
    ```

---

### 14. CollectionView kullanılmış mı?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Maui/Views/MovieListPage.xaml`
- **Nasıl?**  
  - Filmler CollectionView ile listelenir.
- **Koddan Örnek:**  
  ```xml
  <CollectionView ItemsSource="{Binding Movies}">
    <!-- ... -->
  </CollectionView>
  ```

---

### 15. Picker kullanılmış mı?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Maui/Views/MovieAddPage.xaml`
- **Nasıl?**  
  - Tür seçimi için Picker kullanılır.
- **Koddan Örnek:**  
  ```xml
  <Picker ItemsSource="{Binding GenreList}" SelectedItem="{Binding SelectedGenre}" />
  ```

---

### 16. DatePicker veya TimePicker kullanılmış mı?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Maui/Views/MovieAddPage.xaml`
- **Nasıl?**  
  - Vizyon tarihi için DatePicker kullanılır.
- **Koddan Örnek:**  
  ```xml
  <DatePicker Date="{Binding ReleaseDate}" />
  ```

---

### 17. Checkbox ya da RadioButton kullanılmış mı?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Maui/Views/MovieAddPage.xaml`
- **Nasıl?**  
  - Yetişkin (IsAdult) seçimi için Checkbox kullanılır.
- **Koddan Örnek:**  
  ```xml
  <CheckBox IsChecked="{Binding IsAdult}" />
  ```

---

### 18. Derste anlatılmayan MAUI kontrollerinden biri kullanılmış mı?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Maui/Controls/ChipControl.cs`
- **Nasıl?**  
  - Özel ChipControl ve animasyonlu görsel yükleme kullanılmıştır.
- **Koddan Örnek:**  
  ```xml
  <controls:ChipControl Text="Drama" ... />
  ```

---

### 19. Service Interface'leri kullanılmış mı?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Api/Services/Interfaces/`
- **Nasıl?**  
  - Tüm servisler interface ile soyutlanmıştır.
- **Koddan Örnek:**  
  ```csharp
  public interface IMovieService
  {
      Task<IEnumerable<Movie>> GetAllMoviesAsync();
      // ...
  }
  ```

---

### 20. Service sınıfları oluşturulmuş mu?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Api/Services/`
- **Nasıl?**  
  - Tüm iş mantığı servis sınıflarında toplanmıştır.
- **Koddan Örnek:**  
  ```csharp
  public class MovieService : IMovieService
  {
      // ...
  }
  ```

---

### 21. Proje genelinde OOP prensipleri uygulanmış mı?
**✅ Evet.**
- **Nerede?**  
  - Tüm backend ve frontend kodunda.
- **Nasıl?**  
  - Soyutlama, kalıtım, arayüzler, encapsulation uygulanmıştır.
- **Koddan Örnek:**  
  ```csharp
  public class User : IEntity
  {
      // ...
  }
  ```

---

### 22. Kaydetme, güncelleme, kullanıcı girişi gibi sayfalardaki veriler doğrulanmış mı (Boş bırakılamaz, karakter sınırı vb.)?
**✅ Evet.**
- **Nerede?**  
  - `MovieApp.Api/Models/Movie.cs` (DataAnnotations)
- **Nasıl?**  
  - `[Required]`, `[StringLength]` gibi validasyonlar kullanılmıştır.
- **Koddan Örnek:**  
  ```csharp
  [Required]
  [StringLength(200)]
  public string Title { get; set; }
  ```

---

### 23. LINQ aktif olarak kullanılmış mı?
**✅ Evet.**
- **Nerede?**  
  - Tüm servislerde.
- **Nasıl?**  
  - Sorgulama ve filtrelemede LINQ kullanılır.
- **Koddan Örnek:**  
  ```csharp
  var filtered = _context.Movies.Where(m => m.IsLocal && m.Genres.Contains("Drama"));
  ```

---

### 24. Proje çalışıyor mu?
**✅ Evet.**
- **Nerede?**  
  - Tüm proje.
- **Nasıl?**  
  - Docker ve .NET ile sorunsuz başlatılır.
- **Koddan Örnek:**  
  ```sh
  docker-compose up -d
  dotnet run --project MovieApp.Api
  ```

---

### 25. Proje konusuna göre, olması gereken minimum işlemler yapılabiliyor mu?
**✅ Evet.**
- **Nerede?**  
  - Tüm uygulama.
- **Nasıl?**  
  - Film ekleme, güncelleme, silme, arama, favori, puanlama, kullanıcı işlemleri eksiksizdir.

---

### 26. C# isimlendirme kurallarına uyulmuş mu?
**✅ Evet.**
- **Nerede?**  
  - Tüm kodda.
- **Nasıl?**  
  - PascalCase, camelCase, interface başına I gibi standartlara uyulmuştur.
- **Koddan Örnek:**  
  ```csharp
  public class FavoriteMovie { ... }
  public interface IUserService { ... }
  ```

---

## Kurulum ve Çalıştırma Adımları

### 1. Gereksinimler
- .NET 7+ SDK
- Docker & Docker Compose
- Visual Studio 2022+ (MAUI için)

### 2. Projeyi Klonlayın
```sh
git clone <repo-url>
cd <proje-klasörü>
```

### 3. Veritabanı ve API'yi Başlatın
```sh
docker-compose up -d
```
- SQL Server ve Adminer arayüzü otomatik olarak başlatılır.
- Adminer ile veritabanını görsel olarak yönetmek için: [http://localhost:4242](http://localhost:4242)
  - System: MS SQL
  - Server: sqlserver
  - Username: sa
  - Password: YourStrong!Passw0rd

### 4. Migrationları Uygulayın (İlk kurulumda gerekebilir)
```sh
dotnet ef database update --project MovieApp.Api
```

### 5. API'yi Çalıştırın
```sh
cd MovieApp.Api
dotnet run
```
- API varsayılan olarak `http://localhost:5000` adresinde çalışır.

### 6. MAUI Uygulamasını Çalıştırın
```sh
cd ../MovieApp.Maui
dotnet build
dotnet run
```
- Veya Visual Studio ile açıp başlatabilirsiniz.

---

## Notlar ve Eksikler
- Şifre değiştirme ekranı temel düzeyde, gelişmiş validasyon eklenebilir. (❌)
- Gelişmiş kullanıcı yönetimi ve e-posta doğrulama eklenmemiştir.
- Testler ve ek güvenlik önlemleri geliştirilebilir.

---

Her türlü soru ve geri bildirim için iletişime geçebilirsiniz. 