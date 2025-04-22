namespace ST10442835PRGPOEPart1
{
    // Handles all display-related functionality for the chatbot
    class Display
    {
        // Displays the logo from a text file
        public void showFile()
        {
            string filePath = "Logo.txt";

            try
            {
                string fileContent = File.ReadAllText(filePath);
                Console.WriteLine(fileContent);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Could not find dile: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        // Prints the chatbot's response with a typing effect
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

        // Gets and validates user input
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

