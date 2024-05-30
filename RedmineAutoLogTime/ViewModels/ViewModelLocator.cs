using Microsoft.Extensions.DependencyInjection;
using WpfDependencyInjectionSample.ViewModels;

namespace RedmineAutoLogTime.ViewModels;

public class ViewModelLocator
{
    public static MainViewModel MainWindow => ServiceLocator.Current.GetService<MainViewModel>()!;
}