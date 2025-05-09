using System;

namespace UntitledEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine engine = new Engine(800, 600, "Engine");
            engine.Run();
        }
    }
}