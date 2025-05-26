using System;
using System.Collections.Generic;

namespace ST10442835PRGPOEPart2
{
    class ChatBot
    {
        private readonly Voice voice = new Voice();
        private readonly Display display = new Display();
        private string userName;
        private string userInterest = "";
        private string lastDiscussedTopic = "";
        private Dictionary<string, List<string>> keywordResponses;
        private List<string> phishingTips;
        private Dictionary<string, string> sentimentResponses;
        private Random rand = new Random();

        //Delegate declaration: handles displaying bot responses
        private delegate void ResponseHandler(string message);

        public ChatBot()
        {
            InitializeResponses();
        }

        public void Run()
        {
            Start();

            //Assign the delegate to the PrintResponse method
            ResponseHandler responseHandler = DisplayBotResponse;

            while (true)
            {
                string userInput = Display.GetValidUserInput();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    responseHandler("Please enter something so I can help.");
                    continue;
                }

                if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                    userInput.Equals("quit", StringComparison.OrdinalIgnoreCase))
                    break;

                string response = GetBotResponse(userInput);

                // Clear console and re-display the logo
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                display.showFile();
                Console.ResetColor();

                //Use delegate to display the chatbot response
                responseHandler(response);
            }

            Console.WriteLine("\nThank you for using the Cybersecurity Awareness Bot. Stay safe online!");
        }

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
            Console.WriteLine("You can ask me about password safety, phishing, scams, or privacy tips.");
            Console.WriteLine("Enter 'Exit' or 'Quit' to end the program.");
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.ResetColor();
        }

        private void InitializeResponses()
        {
            //Cybersecurity keyword responses
            keywordResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "password safety", new List<string> {
                    "Make sure to use strong, unique passwords for each account.",
                    "Avoid using personal details like birthdays or names in your passwords.",
                    "Use a password manager to store your passwords securely." }
                },
                { "scam", new List<string> {
                    "Online scams can trick you into giving up personal information.",
                    "Always verify the sender of unexpected emails or messages.",
                    "Be cautious of deals that sound too good to be true—they often are." }
                },
                { "privacy", new List<string> {
                    "Adjust your social media privacy settings to control who sees your info.",
                    "Avoid oversharing personal details online.",
                    "Use encrypted messaging apps to protect your conversations." }
                }
            };

            //Phishing tips
            phishingTips = new List<string>
            {
                "Be cautious of emails asking for personal information.",
                "Scammers often disguise themselves as trusted organisations.",
                "Hover over links to see where they lead before clicking.",
                "Don’t open attachments from unknown senders."
            };

            //Sentiment-based responses
            sentimentResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "worried", "It's completely understandable to feel that way. Let me share some tips to help you feel more secure." },
                { "curious", "I'm glad you're curious! Cybersecurity awareness is the first step to staying safe." },
                { "frustrated", "Sorry you're feeling frustrated. Let’s tackle your concerns together." },
                { "mad", "Sorry you're feeling mad. Let’s tackle your concerns together." }
            };
        }

        private string GetBotResponse(string userInput)
        {
            userInput = userInput.ToLower();

            //Sentiment detection
            foreach (var sentiment in sentimentResponses.Keys)
            {
                if (userInput.Contains(sentiment))
                    return sentimentResponses[sentiment];
            }

            //Remember user interest
            if (userInput.Contains("interested in"))
            {
                int index = userInput.IndexOf("interested in") + "interested in".Length;
                userInterest = NormalizeTopic(userInput.Substring(index).Trim('.', ' ', '?'));
                return $"Great! I'll remember that you're interested in {userInterest}. It's a crucial part of staying safe online.";
            }

            //Recall user interest
            if (userInput.Contains("remind me") || userInput.Contains("what was my interest"))
            {
                if (string.IsNullOrEmpty(userInterest))
                {
                    return GetRandomResponse(new List<string>
                    {
                        "You haven't told me your favourite topic yet!",
                        "I don’t remember you telling me a topic.",
                        "We haven't discussed your interests yet."
                    });
                }
                else
                {
                    return GetRandomResponse(new List<string>
                    {
                        $"You said you're interested in {userInterest}. Would you like to know more about it?",
                        $"Previously, you mentioned {userInterest}. Want to dive deeper into that?",
                        $"You're curious about {userInterest}, right? I can tell you more if you'd like."
                    });
                }
            }

            //Follow-up based on memory
            if ((userInput.Contains("yes") || userInput.Contains("sure") || userInput.Contains("ok")) && !string.IsNullOrEmpty(userInterest))
            {
                return $"Here’s more about {userInterest}: {GetTopicTip(userInterest)}";
            }

            //Follow-up based on last topic
            if ((userInput.Contains("more") || userInput.Contains("explain")) && !string.IsNullOrEmpty(lastDiscussedTopic))
            {
                return $"Here's more about {lastDiscussedTopic}: {GetTopicTip(lastDiscussedTopic)}";
            }

            //Keyword recognition
            foreach (var keyword in keywordResponses.Keys)
            {
                if (userInput.Contains(keyword))
                {
                    lastDiscussedTopic = keyword;
                    return GetRandomResponse(keywordResponses[keyword]);
                }
            }

            //Phishing keyword
            if (userInput.Contains("phishing"))
            {
                lastDiscussedTopic = "phishing";
                return GetRandomResponse(phishingTips);
            }

            //Friendly replies
            if (userInput.Contains("how are you"))
                return $"I'm doing well, {userName}! Ready to help with your cybersecurity questions.";

            if (userInput.Contains("what can you do") || userInput.Contains("purpose"))
                return "My purpose is to educate about cybersecurity best practices and help keep you safe online.";

            //Fallback response
            return "I'm not sure I understand. Could you try rephrasing or ask about passwords, scams, phishing, or privacy?";
        }

        private string GetTopicTip(string topic)
        {
            topic = NormalizeTopic(topic);

            foreach (var key in keywordResponses.Keys)
            {
                if (topic.Contains(key))
                    return GetRandomResponse(keywordResponses[key]);
            }

            if (topic.Contains("phishing"))
                return GetRandomResponse(phishingTips);

            return "I don’t have more details on that specific topic right now, but I'm always learning!";
        }

        private string NormalizeTopic(string topic)
        {
            topic = topic.ToLower().Trim();
            if (topic.EndsWith("s"))
                topic = topic.Substring(0, topic.Length - 1);
            return topic;
        }

        private string GetRandomResponse(List<string> responses)
        {
            return responses[rand.Next(responses.Count)];
        }

        //Method that matches the ResponseHandler delegate
        private void DisplayBotResponse(string message)
        {
            Display.PrintResponse(message);
        }
    }
}
