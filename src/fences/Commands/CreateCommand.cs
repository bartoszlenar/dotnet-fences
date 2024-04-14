namespace Fences.Commands;

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class CreateCommand : AsyncCommand<CreateCommand.Settings>
{
    public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.Write("Creating fence: " + Path.Combine(Environment.CurrentDirectory, settings.Path));

        return Task.FromResult(0);
    }

    public sealed class Settings : GlobalSettings
    {
        [CommandArgument(0, "[path]")]
        [Description("The path to create the fence in. Default: environment's current directory.")]
        [DefaultValue(".")]
        public string Path { get; init; } = ".";

        [CommandOption("-r|--root")]
        [Description("If set, the fence will be created as a solution's root fence.")]
        [DefaultValue(false)]
        public bool IsRoot { get; init; }

        [CommandOption("-n|--namespace <NAMESPACE>")]
        [Description("The namespace for this fence. Default: namespaced derived from the path (and fences in parent directories).")]
        [DefaultValue(false)]
        public string? Namespace { get; init; }
    }
}
