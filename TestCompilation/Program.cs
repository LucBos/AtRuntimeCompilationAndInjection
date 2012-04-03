using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCompilation
{
    class Program
    {
        static List<ICommand> Commands = new List<ICommand>() {new EvaluateExpressionCommand(), new InsertExpressionCommand()}; 

        static void Main(string[] args)
        {
            while (true)
            {
                string commandAggregate =
                    Commands.Select(
                        command => string.Format("{0} = {1}   ", command.Activator.ToString(), command.Description))
                        .Aggregate((s, s1) => s1 + s);

                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Usage: {0}, press escape to exit.", commandAggregate);
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine();

                var usage = Console.ReadKey();
                Console.WriteLine();

                if (usage.Key.Equals(ConsoleKey.Escape))
                    return;

                ExecuteCommandFromUsage(usage);
            }
        }

        private static void ExecuteCommandFromUsage(ConsoleKeyInfo usage)
        {
            var respondingCommand = Commands.FirstOrDefault(command => command.Activator.Equals(usage.Key));
            if (respondingCommand == null)
            {
                Console.WriteLine("Usage not correct");
            }
            else
            {
                Console.Clear();
                respondingCommand.Handle();
            }
        }
    }
}
