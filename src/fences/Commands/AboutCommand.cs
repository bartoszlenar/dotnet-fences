namespace fences.Commands;

using System;
using System.Globalization;
using fences.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class AboutCommand(AppInfo appInfo) : AsyncCommand
{
    private readonly AppInfo appInfo = appInfo ?? throw new ArgumentNullException(nameof(appInfo));

    public override Task<int> ExecuteAsync(CommandContext context)
    {
        AnsiConsole.Write(new FigletText(this.appInfo.Name).LeftJustified());

        var table = new Table
        {
            Border = TableBorder.None,
        };

        table.AddColumn("key");
        table.AddColumn("value");
        table.Columns[0].PadRight(3);
        table.HideHeaders();

        table.AddRow("name:", this.appInfo.Name);
        table.AddRow("version:", this.appInfo.Version);
        table.AddRow("commit id:", this.appInfo.Commit);
        table.AddRow("build time:", this.appInfo.BuildTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        table.AddRow("project url:", this.appInfo.ProjectUrl);
        table.AddRow("author:", this.appInfo.Author);

        AnsiConsole.Write(table);

        return Task.FromResult(0);
    }
}
