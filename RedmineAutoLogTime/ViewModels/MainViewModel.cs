using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WpfDependencyInjectionSample.ViewModels;

public class MainViewModel : ObservableObject
{
    private string _text = null!;

    public string Text
    {
        get => _text;
        set
        {
            if (value == _text) return;
            _text = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand StartCommand { get; }
    
    public MainViewModel()
    {
        StartCommand = new RelayCommand(Start);
    }

    private void Start()
    {
        MessageBox.Show(Text);
    }
}