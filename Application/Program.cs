using System;
using System.Threading.Tasks;

namespace UntitledEngine
{
    class Program
    {
        private static Engine _engine;
        
        static async Task Main(string[] args)
        {
            // Create and run the engine
            _engine = new Engine(450, 450, "Engine");
            _engine.Run();
        }
    }
}
