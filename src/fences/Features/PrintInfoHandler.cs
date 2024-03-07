using System.ComponentModel;
using System.Globalization;
using MediatR;
using Spectre.Console;
using Spectre.Console.Cli;


public sealed class PrintInfoRequest : CommandSettings, IRequest<int>
{
    [CommandOption("--verbose")]
    [Description("Display verbose information")]
    [DefaultValue(false)]
    public bool Verbose { get; set; }
}

class PrintInfoHandler : IRequestHandler<PrintInfoRequest, int>
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

        if (request.Verbose)
        {
            AnsiConsole.WriteLine("full version: " + _appInfo.FullVersion);
            AnsiConsole.WriteLine("build time: " + _appInfo.BuildTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        }

        return Task.FromResult(0);
    }
}
