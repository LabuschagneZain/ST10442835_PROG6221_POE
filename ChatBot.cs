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
            Console.WriteLine("You can ask me about password safety, phishing, scams, vpn, malware or privacy tips.");
            Console.WriteLine("Enter 'Exit' or 'Quit' to end the program.");
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.ResetColor();
        }

        private void InitializeResponses()
        {
            // Keyword responses
            keywordResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
{
    { "password safety", new List<string> {
        "Make sure to use strong, unique passwords for each account.",
        "Avoid using personal details like birthdays or names in your passwords.",
        "Use a password manager to store your passwords securely.",
        "Enable two-factor authentication (2FA) wherever possible.",
        "Change your passwords regularly, especially if a breach occurs."
    }},
    { "scam", new List<string> {
        "Online scams can trick you into giving up personal information.",
        "Always verify the sender of unexpected emails or messages.",
        "Be cautious of deals that sound too good to be true—they often are.",
        "Watch out for phishing emails that create a sense of urgency.",
        "Never click suspicious links or download unexpected attachments."
    }},
    { "privacy", new List<string> {
        "Adjust your social media privacy settings to control who sees your info.",
        "Avoid oversharing personal details online.",
        "Use encrypted messaging apps to protect your conversations.",
        "Review app permissions and revoke those that are unnecessary.",
        "Browse in incognito mode or use a VPN for extra privacy."
    }},
    { "malware", new List<string> {
        "Malware can infect your device through suspicious downloads or email attachments.",
        "Keep your antivirus software updated to catch new threats.",
        "Avoid downloading software from untrusted websites.",
        "Scan USB drives before accessing their contents.",
        "Don’t ignore software updates—they often fix security vulnerabilities."
    }},
    { "phishing", new List<string> {
        "Phishing attempts often pretend to be legitimate companies.",
        "Always double-check email addresses and URLs before clicking.",
        "If you're unsure, contact the company directly using official channels.",
        "Never give out personal info through links in emails or texts.",
        "Report suspicious emails to your IT department or email provider."
    }},
   
    { "vpn", new List<string> {
        "VPNs encrypt your internet traffic, keeping it safe from snoopers.",
        "Use a VPN when browsing on public Wi-Fi networks.",
        "Choose a reputable VPN provider that doesn’t log your data.",
        "A VPN can also help you bypass geo-restricted content securely.",
        "Make sure your VPN is active before logging into sensitive accounts."
    }}
};


            //Phishing tips
            phishingTips = new List<string>
            {
                "Be cautious of emails asking for personal information.",
                "Scammers often disguise themselves as trusted organisations.",
                "Hover over links to see where they lead before clicking.",
                "Don’t open attachments from unknown senders."
            };

            // Sentiment-based responses
            sentimentResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
{
             { "worried", "It's completely understandable to feel that way. Let me share some tips to help you feel more secure." },
             { "curious", "I'm glad you're curious! Cybersecurity awareness is the first step to staying safe." },
             { "frustrated", "Sorry you're feeling frustrated. Let’s tackle your concerns together." },
             { "mad", "Sorry you're feeling mad. Let’s work through your concerns together and find a solution." },
             { "anxious", "You're not alone in feeling anxious. I'm here to help break things down and support you." },
             { "confused", "That’s okay—cybersecurity can be tricky. Let me explain things in a simpler way." },
             { "scared", "It’s okay to be scared. I’ll help you understand the risks and how to stay safe." },
             { "angry", "I understand your anger. Let’s figure out what’s wrong and work through it." },
             { "hopeful", "That's great to hear! Let’s build on that and explore ways to stay secure." },
             { "excited", "Awesome! It’s great to see your enthusiasm. Let's dive into some cool cybersecurity topics!" },
             { "overwhelmed", "It can definitely feel like a lot. Let's take it step by step together." },
             { "bored", "Let’s spice things up! I can show you some interesting cybersecurity facts or challenges." },
             { "happy", "That’s wonderful to hear! Let’s keep that positive energy going while we learn more." },
             { "sad", "I’m sorry you’re feeling down. Maybe learning something new can lift your mood a bit." },
             { "grateful", "I'm glad I could help! Your appreciation means a lot." }
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
            return "I'm not sure I understand. Could you try rephrasing or ask about passwords, scams, phishing, vpn, malware or privacy?";
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
