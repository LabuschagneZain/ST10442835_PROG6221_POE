namespace ST10442835PRGPOEPart1
{
    class Display
    {
        public void showFile()
        {
            string filePath = "Logo.txt";
            string output = "";

            try
            {
                string fileContent = File.ReadAllText(filePath);
                Console.WriteLine(fileContent);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File not found: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static void PrintResponse(string response)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nBot: ");

            foreach (char c in response)
            {
                Console.Write(c);
                Thread.Sleep(30);
            }

            Console.ResetColor();
            Console.WriteLine();
        }

        public static void PrintSectionHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n=== {title.ToUpper()} ===");
            Console.ResetColor();
        }

        public static string GetValidUserInput()
        {
            while (true)
            {
                Console.Write("\nYou: ");
                string input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                Console.WriteLine("Please enter a valid question or message.");
            }
        }
    }
}

