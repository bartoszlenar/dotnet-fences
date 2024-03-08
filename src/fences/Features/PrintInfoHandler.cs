using System.ComponentModel;
using System.Globalization;
using fences.Helpers;
using fences.Helpers.Commands;
using MediatR;
using Spectre.Console;
using Spectre.Console.Cli;


public sealed class PrintInfoRequest : FeatureRequest
{
    [CommandOption("--verbose")]
    [Description("Display verbose information")]
    [DefaultValue(false)]
    public bool Verbose { get; set; }
}

class PrintInfoHandler : IFeatureHandler
{
    private readonly AppInfo _appInfo;

    public PrintInfoHandler(AppInfo appInfo)
    {
        _appInfo = appInfo;
    }


    public Task<int> Handle(PrintInfoRequest request, CancellationToken cancellationToken)
    {
        AnsiConsole.Write(new FigletText(_appInfo.Name).LeftJustified());
        AnsiConsole.WriteLine("version: " + _appInfo.Version);
        AnsiConsole.WriteLine("commit: " + _appInfo.Commit);
        AnsiConsole.WriteLine("repository url: " + _appInfo.RepositoryUrl);
        AnsiConsole.WriteLine("build time: " + _appInfo.BuildTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

        return Task.FromResult(0);
    }
}
