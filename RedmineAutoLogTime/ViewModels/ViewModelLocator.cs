using Microsoft.Extensions.DependencyInjection;

namespace RedmineAutoLogTime.ViewModels;

public class ViewModelLocator
{
    public static MainViewModel MainWindow => ServiceLocator.Current.GetService<MainViewModel>()!;
}