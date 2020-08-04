using System.Drawing;
using Connector.Commands;
using Connector.Printer;

namespace Connector.Commands.CommandsRealization
{
    public class TestCommand : CommandBase
    {
        public TestCommand() :base()
        {
            Id = "testCommand";
        }

        public override void Invoke(string[] args)
        {
            if (args.Length >= 3)
                Print.Log($"{args[0]}///{args[1]}//{args[2]}", Color.Yellow);

            var numbers = this.ParseFloat(args);

            for (var i = 0; i < numbers.Count; i++)
            {
                Print.Log($"Вытянутое число из аргумента {numbers[i]}", Color.Green);
            }
        }
    }
}