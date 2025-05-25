using System;
using System.Threading.Tasks;

namespace UntitledEngine
{
    class Program
    {
        public static Engine Engine { get; private set; }

        static async Task Main(string[] args)
        {
            // Create and run the engine
            Engine = new Engine(450, 450, "Engine");
            Engine.Run();
        }
    }
}
