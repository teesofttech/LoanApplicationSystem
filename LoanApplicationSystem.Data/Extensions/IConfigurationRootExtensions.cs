﻿using Microsoft.Extensions.Configuration;

namespace LoanApplicationSystem.Data.Extensions;
public static class IConfigurationRootExtensions
{
    public static IConfigurationBuilder AddBasePath(this IConfigurationBuilder builder)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var startupProjectPath = Path.Combine(currentDirectory, "..", "LoanApplicationSystem.UI");
        var basePathConfiguration = Directory.Exists(startupProjectPath) ? startupProjectPath : currentDirectory;

        return builder.SetBasePath(basePathConfiguration);
    }
}
