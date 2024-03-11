using System.Globalization;
using fences.Helpers;
using fences.Helpers.Commands;
using MediatR;
using Spectre.Console;
using Spectre.Console.Cli;

public sealed class PrintAboutRequest : FeatureRequest
{
}

class PrintAboutFeature : IFeatureHandler<PrintAboutRequest>
{

    private readonly AppInfo _appInfo;

    public PrintAboutFeature(AppInfo appInfo)
    {
        _appInfo = appInfo;
    }

    public Task<int> Handle(PrintAboutRequest request, CancellationToken cancellationToken)
    {
        AnsiConsole.Write(new FigletText(_appInfo.Name).LeftJustified());

        var table = new Table();
        table.Border = TableBorder.None;

        table.AddColumn("key");
        table.AddColumn("value");
        table.Columns[0].PadRight(3);
        table.HideHeaders();

        table.AddRow("name:", _appInfo.Name);
        table.AddRow("version:", _appInfo.Version);
        table.AddRow("commit id:", _appInfo.Commit);
        table.AddRow("build time:", _appInfo.BuildTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        table.AddRow("project url:", _appInfo.ProjectUrl);
        table.AddRow("author:", _appInfo.Author);

        AnsiConsole.Write(table);

        return Task.FromResult(0);
    }
}

public static class PrintAboutFeatureExtensions
{
    public static ICommandConfigurator AddPrintAboutFeature(this IConfigurator configurator, string name, IMediator mediator)
    {
        return configurator
            .AddFeature<PrintAboutRequest>(mediator, "about")
            .WithDescription("Prints detailed info about the application.");
    }
}
