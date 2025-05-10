using System;
using System.Threading.Tasks;

namespace UntitledEngine
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create and run the engine
            Engine engine = new Engine(800, 600, "Engine");
            engine.Run();
        }
    }
}
