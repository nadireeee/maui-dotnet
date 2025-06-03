using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MovieApp.Maui.Theme;

public static class AppTheme
{
    // Colors
    public static class Colors
    {
        public static Color Primary = Color.FromArgb("#B388FF");
        public static Color Secondary = Color.FromArgb("#512BD4");
        public static Color Background = Color.FromArgb("#181828");
        public static Color Surface = Color.FromArgb("#23233a");
        public static Color SurfaceVariant = Color.FromArgb("#2D2D44");
        public static Color Error = Color.FromArgb("#FF4081");
        public static Color Text = Microsoft.Maui.Graphics.Colors.White;
        public static Color TextSecondary = Color.FromArgb("#808080");
        public static Color Transparent = Microsoft.Maui.Graphics.Colors.Transparent;
    }

    // Dimensions
    public static class Dimensions
    {
        public const double CornerRadius = 28;
        public const double CornerRadiusSmall = 15;
        public const double Padding = 32;
        public const double Spacing = 24;
    }

    // Styles
    public static class Styles
    {
        public static Style NeonButton => new Style(typeof(Button))
        {
            Setters =
            {
                new Setter { Property = Button.BackgroundColorProperty, Value = Colors.SurfaceVariant },
                new Setter { Property = Button.TextColorProperty, Value = Colors.Primary },
                new Setter { Property = Button.CornerRadiusProperty, Value = 25 },
                new Setter { Property = Button.PaddingProperty, Value = new Thickness(20, 10) },
                new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold },
                new Setter 
                { 
                    Property = Button.ShadowProperty, 
                    Value = new Shadow
                    {
                        Brush = Colors.Primary,
                        Offset = new Point(0, 0),
                        Radius = 10,
                        Opacity = 0.5f
                    }
                }
            }
        };

        public static Style NeonButtonSecondary => new Style(typeof(Button))
        {
            Setters =
            {
                new Setter { Property = Button.BackgroundColorProperty, Value = Colors.Transparent },
                new Setter { Property = Button.TextColorProperty, Value = Colors.TextSecondary },
                new Setter { Property = Button.CornerRadiusProperty, Value = 25 },
                new Setter { Property = Button.PaddingProperty, Value = new Thickness(20, 10) },
                new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.None }
            }
        };

        public static Style NeonEntry => new Style(typeof(Entry))
        {
            Setters =
            {
                new Setter { Property = Entry.TextColorProperty, Value = Colors.Text },
                new Setter { Property = Entry.PlaceholderColorProperty, Value = Colors.TextSecondary },
                new Setter { Property = Entry.BackgroundColorProperty, Value = Colors.Transparent }
            }
        };

        public static Style TitleLabel => new Style(typeof(Label))
        {
            Setters =
            {
                new Setter { Property = Label.TextColorProperty, Value = Colors.Primary },
                new Setter { Property = Label.FontSizeProperty, Value = 32 },
                new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold },
                new Setter { Property = Label.HorizontalOptionsProperty, Value = LayoutOptions.Center }
            }
        };

        public static Style ErrorLabel => new Style(typeof(Label))
        {
            Setters =
            {
                new Setter { Property = Label.TextColorProperty, Value = Colors.Error },
                new Setter { Property = Label.FontSizeProperty, Value = 15 },
                new Setter { Property = Label.HorizontalOptionsProperty, Value = LayoutOptions.Center }
            }
        };
    }
} 