using Microsoft.Maui.Controls;

namespace MovieApp.Maui.Behaviors;

public class PageTransitionBehavior : Behavior<Page>
{
    protected override void OnAttachedTo(Page page)
    {
        base.OnAttachedTo(page);
        page.Appearing += OnPageAppearing;
    }

    protected override void OnDetachingFrom(Page page)
    {
        base.OnDetachingFrom(page);
        page.Appearing -= OnPageAppearing;
    }

    private async void OnPageAppearing(object sender, EventArgs e)
    {
        if (sender is Page page)
        {
            // Sayfa görünür olduğunda animasyon başlat
            await page.FadeTo(0, 0);
            await page.FadeTo(1, 300, Easing.CubicOut);
        }
    }
} 