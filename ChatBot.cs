namespace ST10442835PRGPOEPart1
{
    // Main chatbot class that handles conversation flow and responses
    class ChatBot
    {
        private readonly Voice voice = new Voice();
        private readonly Display display = new Display();
        private string userName;

        // Main method to run the chatbot
        public void Run()
        {
            Start();

            // Main conversation loop
            while (true)
            {
                string userInput = Display.GetValidUserInput();

                if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                    userInput.Equals("quit", StringComparison.OrdinalIgnoreCase))
                    break;

                string response = GetBotResponse(userInput);
                Display.PrintResponse(response);
            }

            Console.WriteLine("\nThank you for using the Cybersecurity Awareness Bot. Stay safe online!");
        }

        // Initializes the chatbot and welcomes the user
        private void Start()
        {
            voice.PlaySound();
           

            Console.ForegroundColor = ConsoleColor.Green;
            display.showFile();
            Console.WriteLine("\nWelcome to the Cybersecurity Awareness Bot!");
            Console.WriteLine("----------------------------------------------");
            Console.ResetColor();

            Console.Write("\nPlease enter your name: ");
            userName = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nHello {userName}! I'm here to help you stay safe online.");
            Console.WriteLine("You can ask me about password safety, phishing, or safe browsing practices.");
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.ResetColor();
        }

        // Generates appropriate responses based on user input
        private string GetBotResponse(string userInput)
        {
            userInput = userInput.ToLower();

            if (userInput.Contains("how are you"))
                return $"I'm doing well, {userName}! Ready to help with your cybersecurity questions.";

            if (userInput.Contains("purpose") || userInput.Contains("what can you do"))
                return "My purpose is to educate about cybersecurity best practices.";

            if (userInput.Contains("password"))
                return "Strong passwords should be at least 12 characters long and include a mix of letters, numbers, and symbols.";

            if (userInput.Contains("phishing"))
                return "Phishing attacks often come via email. Never click links or download attachments from suspicious senders.";

            if (userInput.Contains("browsing"))
                return "For safe browsing, always check for HTTPS in the URL and avoid public Wi-Fi for sensitive transactions.";

            return "I didn't quite understand that. Could you ask about password safety, phishing, or safe browsing?";
        }
    }
}
