namespace OthelloPersonal
{
    class Program
    {
        static void Main(string[] args)
        {
            bool running = true;
            

            while (running)
            {


                running = ShowMenu(); // returns false if Exit chosen
            }

        }

        static bool ShowMenu()
        {
            string[] options = { "   ", "Exit" };
            int index = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("Use ↑ ↓ to navigate, Enter to select:\n");

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == index)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    Console.WriteLine(options[i]);

                    Console.ResetColor();
                }

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                    index = (index == 0) ? options.Length - 1 : index - 1;
                else if (key == ConsoleKey.DownArrow)
                    index = (index + 1) % options.Length;

            } while (key != ConsoleKey.Enter);

            // If "Restart" is chosen, return true, otherwise false
            return index == 0;
        }
    }
}

