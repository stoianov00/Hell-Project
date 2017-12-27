using System;

public class StartUp
{
    public static void Main()
    {
        InputReader reader = new ConsoleReader();
        IOutputWriter writer = new ConsoleWriter();
        HeroManager manager = new HeroManager();

        Engine engine = new Engine(reader, writer, manager);
        engine.Run();
        
    }
}