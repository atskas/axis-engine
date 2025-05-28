using UntitledEngine.Core;

namespace UntitledEngine.Executable
{
    class Program
    {
        private static Engine _engine;

        static void Main(string[] args)
        {
            _engine = new Engine(450, 450, "Engine");
            _engine.Run();
        }
    }
}