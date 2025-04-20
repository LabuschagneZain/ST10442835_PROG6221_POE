namespace ST10442835PRGPOEPart1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ChatBot bot = new ChatBot();
                bot.Run();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error: {ex.Message}");
                Console.ResetColor();
            }
            finally
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }
    }


}
