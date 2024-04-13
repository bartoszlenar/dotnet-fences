namespace Fences.Commands;

using System;
using System.Globalization;
using Fences.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class AboutCommand(AppInfo appInfo) : AsyncCommand
{
    private readonly AppInfo _appInfo = appInfo ?? throw new ArgumentNullException(nameof(appInfo));

    public override Task<int> ExecuteAsync(CommandContext context)
    {
        AnsiConsole.Write(new FigletText(this._appInfo.Name).LeftJustified());

        var table = new Table
        {
            Border = TableBorder.None,
        };

        table.AddColumn("key");
        table.AddColumn("value");
        table.Columns[0].PadRight(3);
        table.HideHeaders();

        table.AddRow("name:", this._appInfo.Name);
        table.AddRow("version:", this._appInfo.Version);
        table.AddRow("commit id:", this._appInfo.Commit);
        table.AddRow("build time:", this._appInfo.BuildTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        table.AddRow("project url:", this._appInfo.ProjectUrl);
        table.AddRow("author:", this._appInfo.Author);

        AnsiConsole.Write(table);

        return Task.FromResult(0);
    }
}
