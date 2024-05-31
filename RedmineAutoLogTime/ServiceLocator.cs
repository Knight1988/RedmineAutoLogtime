using System;
using Microsoft.Extensions.DependencyInjection;

namespace RedmineAutoLogTime;

public static class ServiceLocator
{
    public static IServiceProvider Current { get; set; }
}