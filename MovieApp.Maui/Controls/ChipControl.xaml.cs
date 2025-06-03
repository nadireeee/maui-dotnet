using System.Windows.Input;

namespace MovieApp.Maui.Controls;

public partial class ChipControl : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(ChipControl), string.Empty);

    public static readonly BindableProperty IsSelectedProperty =
        BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(ChipControl), false,
            propertyChanged: OnIsSelectedChanged);

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ChipControl), null);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ChipControl), null);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public ChipControl()
    {
        InitializeComponent();
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnTapped;
        GestureRecognizers.Add(tapGesture);
    }

    private void OnTapped(object sender, EventArgs e)
    {
        if (Command?.CanExecute(CommandParameter) == true)
        {
            Command.Execute(CommandParameter);
        }
    }

    private static void OnIsSelectedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ChipControl chip)
        {
            chip.UpdateVisualState();
        }
    }

    private void UpdateVisualState()
    {
        if (IsSelected)
        {
            ChipFrame.BackgroundColor = Color.FromArgb("#7B2FF2");
            ChipText.TextColor = Colors.White;
        }
        else
        {
            ChipFrame.BackgroundColor = Color.FromArgb("#232526");
            ChipText.TextColor = Colors.White;
        }
    }

    protected override void OnPropertyChanged(string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == TextProperty.PropertyName)
        {
            ChipText.Text = Text;
        }
    }
} 