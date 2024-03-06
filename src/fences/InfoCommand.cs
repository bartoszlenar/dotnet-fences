using System.Reflection;
using Spectre.Console;
using Spectre.Console.Cli;

namespace fences;

class InfoCommand : Command
{
    public override int Execute(CommandContext context)
    {
        var programInfo = ProgramInfo.FromAssembly(Assembly.GetExecutingAssembly());

        AnsiConsole.Write(new FigletText(programInfo.Name).LeftJustified());
        AnsiConsole.WriteLine("version: " + programInfo.Version);
        AnsiConsole.WriteLine("commit: " + programInfo.Commit);

        return 0;
    }
}
