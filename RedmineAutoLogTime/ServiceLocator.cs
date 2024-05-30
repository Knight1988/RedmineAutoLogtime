using Microsoft.Extensions.DependencyInjection;

namespace RedmineAutoLogTime;

public static class ServiceLocator
{
    public static ServiceProvider Current { get; set; }
}