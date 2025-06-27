using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ST10442835_PROG6221_POE.Managers;
using ST10442835_PROG6221_POE.Models;

namespace ST10442835_PROG6221_POE
{
    class ChatBot
    {
        private bool waitingForName = true;
        private TaskManager taskManager = new TaskManager();
        private QuizManager quizManager = null;
        private bool inQuizMode = false;

        private readonly PlayAudio voice = new PlayAudio();
        private string userName = "User"; // default fallback
        private string userInterest = "";
        private string lastDiscussedTopic = "";
        private Dictionary<string, List<string>> keywordResponses;
        private List<string> phishingTips;
        private Dictionary<string, string> sentimentResponses;
        private List<string> activityLog = new List<string>(); //Activity Log
        private Random rand = new Random();

        private bool waitingForReminder = false;
        private int lastAddedTaskIndex = -1;
        int count = 2;

        public ChatBot()
        {
            InitializeResponses();
            voice.PlaySound();
        }

        public void SetUserName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                userName = name;
        }

        public string GetBotResponse(string userInput)
        {
            //Ask name
            if (waitingForName)
            {
                string name = userInput.Trim();
                SetUserName(name);
                waitingForName = false;
                activityLog.Add($"{DateTime.Now:HH:mm} - User set name: {userName}");
                return $"Hello {userName}! I'm here to help you stay safe online.\nYou can ask me about password safety, phishing, scams, VPNs, malware, or privacy tips.\nType 'play game' to start the quiz!";
            }

            userInput = userInput.Trim();
            string userInputLower = userInput.ToLower();
            //Activity log
            if (userInputLower.Contains("activity log") || userInputLower.Contains("what have you done for me"))
            {
                if (activityLog.Count == 0)
                    return "There are no recorded actions yet.";

                var recent = activityLog.TakeLast(5).ToList();
                return "Here's a summary of recent actions:\n" + string.Join("\n", recent.Select((log, i) => $"{i + 1}. {log}"));
            }

            if (inQuizMode)
                return HandleQuizInput(userInput);
            //Reminder
            if (waitingForReminder)
            {
                int days = ParseReminderDays(userInputLower);
                if (days > 0 && lastAddedTaskIndex >= 0 && lastAddedTaskIndex < taskManager.Tasks.Count)
                {
                    var task = taskManager.Tasks[lastAddedTaskIndex];
                    task.ReminderDate = DateTime.Now.AddDays(days);
                    activityLog.Add($"{DateTime.Now:HH:mm} - Reminder set for task: \"{task.Title}\" on {task.ReminderDate.Value:dd MMM yyyy}");
                    waitingForReminder = false;
                    lastAddedTaskIndex = -1;
                    return $"Reminder set for task '{task.Title}' in {days} days.";
                }
                else
                {
                    return "Sorry, I didn't understand the reminder time. Please say something like 'in 3 days'.";
                }
            }
            //Game
            if (userInputLower.Contains("play game") || userInputLower.Contains("start game") || userInputLower.Contains("play quiz"))
            {
                quizManager = new QuizManager();
                inQuizMode = true;
                activityLog.Add($"{DateTime.Now:HH:mm} - Quiz started by user.");
                return $"Starting Cybersecurity Quiz! You will be asked 10 questions \n\n{FormatQuestion(quizManager.GetCurrentQuestion())}";
            }

            foreach (var sentiment in sentimentResponses.Keys)
            {
                if (userInputLower.Contains(sentiment))
                    return sentimentResponses[sentiment];
            }
            //Interested in
            if (userInputLower.Contains("interested in"))
            {
                int index = userInputLower.IndexOf("interested in") + "interested in".Length;
                userInterest = NormalizeTopic(userInputLower.Substring(index).Trim('.', ' ', '?'));
                activityLog.Add($"{DateTime.Now:HH:mm} - User expressed interest in: {userInterest}");
                return $"Great! I'll remember that you're interested in {userInterest}. It's a crucial part of staying safe online.";
            }
            //Ask what user interest was
            if (userInputLower.Contains("my interest") || userInputLower.Contains("what was my interest"))
            {
                if (string.IsNullOrEmpty(userInterest))
                    return "You haven't told me your favourite topic yet!";

                return $"You said you're interested in {userInterest}. Want to know more?";
            }
            //If they want to know more about there intrest
            if ((userInputLower.Contains("yes") || userInputLower.Contains("sure")) && !string.IsNullOrEmpty(userInterest))
            {
                return $"Here’s more about {userInterest}: {GetTopicTip(userInterest)}";
            }
            //If they want to know more about the previous topic
            if ((userInputLower.Contains("more") || userInputLower.Contains("explain")) && !string.IsNullOrEmpty(lastDiscussedTopic))
            {
                return $"Here's more about {lastDiscussedTopic}: {GetTopicTip(lastDiscussedTopic)}";
            }

            foreach (var keyword in keywordResponses.Keys)
            {
                if (userInputLower.Contains(keyword))
                {
                    lastDiscussedTopic = keyword;
                    return GetRandomResponse(keywordResponses[keyword]);
                }
            }

            if (userInputLower.Contains("phishing"))
            {
                lastDiscussedTopic = "phishing";
                return GetRandomResponse(phishingTips);
            }
            //Add task
            if (userInputLower.StartsWith("add task") || userInputLower.StartsWith("remind me to") || userInputLower.StartsWith("set reminder"))
            {
                var (title, reminderDays) = ExtractTaskAndReminder(userInputLower);
                var reminderDate = reminderDays > 0 ? DateTime.Now.AddDays(reminderDays) : (DateTime?)null;
                taskManager.AddTask(title, $"Task created from chatbot: {title}", reminderDate);
                lastAddedTaskIndex = taskManager.Tasks.Count - 1;
                activityLog.Add($"{DateTime.Now:HH:mm} - Task added: \"{title}\"{(reminderDays > 0 ? $" with reminder in {reminderDays} days" : "")}");

                if (reminderDays > 0) // User gave days
                {
                    waitingForReminder = false;
                    return $"Task added: \"{title}\" with a reminder set for {reminderDate:dd MMM yyyy}.";
                }
                else //User did not give days
                {
                    waitingForReminder = true;
                    return $"Task added: \"{title}\". Would you like to set a reminder for this task? (e.g., 'in 3 days')";
                }
            }
            //Request to view tasks
            if (userInputLower.Contains("view tasks") || userInputLower.Contains("my tasks"))
            {
                var tasks = taskManager.Tasks;
                if (tasks.Count == 0)
                    return "You currently have no tasks.";

                var taskList = tasks.Select((t, i) =>
                    $"{i + 1}. {t.Title}" + (t.ReminderDate.HasValue ? $" (Reminder: {t.ReminderDate.Value:dd MMM yyyy})" : ""));
                return "Here are your current tasks:\n" + string.Join("\n", taskList);
            }

            if (userInputLower.Contains("how are you"))
                return $"I'm doing well, {userName}! Ready to help with your cybersecurity questions.";

            if (userInputLower.Contains("what can you do") || userInputLower.Contains("purpose"))
                return "My purpose is to educate about cybersecurity best practices and help keep you safe online.";

            return "I'm not sure I understand. Could you try rephrasing or ask about passwords, scams, phishing, vpn, malware or privacy?";
        }
        //Answer user gave for game
        private string HandleQuizInput(string userInput)
        {
            int selectedIndex = -1;
            userInput = userInput.Trim().ToUpper();

            if (int.TryParse(userInput, out int num))
                selectedIndex = num - 1;
            else if (!string.IsNullOrEmpty(userInput))
            {
                char c = userInput[0];
                if (c >= 'A' && c <= 'D')
                    selectedIndex = c - 'A';
            }

            var question = quizManager.GetCurrentQuestion();
            if (selectedIndex < 0 || selectedIndex >= question.Options.Count)
                return "Please answer by typing the number or letter of your choice.";

            string feedback = quizManager.SubmitAnswer(selectedIndex);

            

            if (quizManager.IsQuizOver())
            {
                inQuizMode = false;
                activityLog.Add($"{DateTime.Now:HH:mm} - Quiz completed. Score: {quizManager.GetFinalScore()}");
                feedback += "\n\n" + quizManager.GetFinalScore();
                feedback += "\n\nYou can ask me questions or say 'play game' to play the quiz again.";
            }
            else
            {
                feedback += $"\n\nQuestion {count}:\n" + FormatQuestion(quizManager.GetCurrentQuestion());
                count ++;
            }

            return feedback;
        }

        private string FormatQuestion(QuizQuestion question)
        {
            string formatted = question.QuestionText + "\n";
            for (int i = 0; i < question.Options.Count; i++)
            {
                char letter = (char)('A' + i);
                formatted += $"{letter}) {question.Options[i]}\n";
            }
            return formatted.TrimEnd();
        }
        //If user said tomorrow
        private (string taskTitle, int reminderDays) ExtractTaskAndReminder(string input)
        {
            string title = input;
            int days = 0;

            if (input.StartsWith("add task"))
                title = input.Substring("add task".Length).Trim();
            else if (input.StartsWith("remind me to"))
                title = input.Substring("remind me to".Length).Trim();
            else if (input.StartsWith("set reminder"))
                title = input.Substring("set reminder".Length).Trim();

            var match = Regex.Match(title, @"(.+?)\s+in\s+(\d+)\s+days?");
            if (match.Success)
            {
                title = match.Groups[1].Value.Trim();
                if (int.TryParse(match.Groups[2].Value, out int parsedDays))
                    days = parsedDays;
            }
            else if (title.EndsWith("tomorrow"))
            {
                title = title.Replace("tomorrow", "").Trim();
                days = 1;
            }

            return (title, days);
        }
        //If tomorrow days = 1
        private int ParseReminderDays(string input)
        {
            var match = Regex.Match(input, @"in\s+(\d+)\s+days?");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int days))
                return days;
            if (input.Contains("tomorrow"))
                return 1;
            return 0;
        }

        private void InitializeResponses()
        {
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

            phishingTips = new List<string>
            {
                "Be cautious of emails asking for personal information.",
                "Scammers often disguise themselves as trusted organisations.",
                "Hover over links to see where they lead before clicking.",
                "Don’t open attachments from unknown senders."
            };

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
        //Remove s
        private string NormalizeTopic(string topic)
        {
            topic = topic.ToLower().Trim();
            if (topic.EndsWith("s"))
                topic = topic.Substring(0, topic.Length - 1);
            return topic;
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

        private string GetRandomResponse(List<string> responses)
        {
            return responses[rand.Next(responses.Count)];
        }
    }
}
