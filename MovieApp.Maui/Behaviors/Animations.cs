using Microsoft.Maui.Controls;

namespace MovieApp.Maui.Behaviors;

public class FadeAnimation : Behavior<VisualElement>
{
    public static readonly BindableProperty DurationProperty =
        BindableProperty.Create(nameof(Duration), typeof(int), typeof(FadeAnimation), 500);

    public int Duration
    {
        get => (int)GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    protected override void OnAttachedTo(VisualElement bindable)
    {
        base.OnAttachedTo(bindable);
        bindable.Opacity = 0;
        bindable.FadeTo(1, (uint)Duration);
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        base.OnDetachingFrom(bindable);
    }
}

public class ScaleAnimation : Behavior<VisualElement>
{
    public static readonly BindableProperty DurationProperty =
        BindableProperty.Create(nameof(Duration), typeof(int), typeof(ScaleAnimation), 500);

    public static readonly BindableProperty ScaleProperty =
        BindableProperty.Create(nameof(Scale), typeof(double), typeof(ScaleAnimation), 1.0);

    public int Duration
    {
        get => (int)GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    public double Scale
    {
        get => (double)GetValue(ScaleProperty);
        set => SetValue(ScaleProperty, value);
    }

    protected override void OnAttachedTo(VisualElement bindable)
    {
        base.OnAttachedTo(bindable);
        bindable.Scale = 1;
        bindable.ScaleTo(Scale, (uint)Duration);
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        base.OnDetachingFrom(bindable);
    }
}

public class ShakeAnimation : Behavior<VisualElement>
{
    public static readonly BindableProperty DurationProperty =
        BindableProperty.Create(nameof(Duration), typeof(int), typeof(ShakeAnimation), 500);

    public int Duration
    {
        get => (int)GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    protected override void OnAttachedTo(VisualElement bindable)
    {
        base.OnAttachedTo(bindable);
        Shake(bindable);
    }

    private async void Shake(VisualElement element)
    {
        await element.TranslateTo(-15, 0, 50);
        await element.TranslateTo(15, 0, 50);
        await element.TranslateTo(-10, 0, 50);
        await element.TranslateTo(10, 0, 50);
        await element.TranslateTo(-5, 0, 50);
        await element.TranslateTo(5, 0, 50);
        await element.TranslateTo(0, 0, 50);
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        base.OnDetachingFrom(bindable);
    }
} 