using System;
using System.Collections.Generic;

namespace Praxis.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.Engine engine = null;
            string initialSetupInput = null;

            while (engine == null)
            {
                initialSetupInput = System.Console.ReadLine();

                if (initialSetupInput.Equals("uci"))
                {
                    engine = new Engine.Engine(Engine.Engine.Protocols.UCI);
                }
                else
                {
                    System.Console.WriteLine("Not Supported Exception: Engine does not support an interface of type '" + initialSetupInput + "'.");
                }
            }

            List<string> engineOutputs = engine.ProcessInputFromInterface(initialSetupInput);
            SendEngineOutputToInterface(engineOutputs);

            while (engine.IsActive)
            {
                string interfaceInput = System.Console.ReadLine();

                engineOutputs = engine.ProcessInputFromInterface(interfaceInput);
                SendEngineOutputToInterface(engineOutputs);
            }

            Environment.Exit(0);
        }

        private static void SendEngineOutputToInterface(List<string> engineOutputs)
        {
            if (engineOutputs != null)
            {
                foreach (string engineOutput in engineOutputs)
                {
                    System.Console.WriteLine(engineOutput);
                }
            }
        }
    }
}
