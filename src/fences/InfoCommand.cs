using System.Reflection;
using Spectre.Console;
using Spectre.Console.Cli;

namespace fences;

class InfoCommand : Command
{

    public override int Execute(CommandContext context)
    {
        AnsiConsole.Write(new FigletText(typeof(Program).Assembly.GetName().Name!).LeftJustified());

        return 0;
    }
}
