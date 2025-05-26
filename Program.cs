namespace ST10442835PRGPOEPart2
{
    internal class Program
    {
        // Entry point for the application
        static void Main(string[] args)
        {
            try
            {
                //Launch primary chatbot functionality  
                ChatBot bot = new ChatBot();
                bot.Run(); // Enters main conversation loop
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error: {ex.Message}");
                Console.ResetColor();
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\nPress any key to exit...");
                Console.ResetColor();
                Console.ReadKey();
            }
        }
    }


}
