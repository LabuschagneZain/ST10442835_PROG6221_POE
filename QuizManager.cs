using ST10442835_PROG6221_POE.Models;

namespace ST10442835_PROG6221_POE.Managers
{
    public class QuizManager
    {
        public List<QuizQuestion> Questions { get; private set; }
        public int CurrentQuestionIndex { get; private set; } = 0;
        public int Score { get; private set; } = 0;

        public QuizManager()
        {
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            Questions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    QuestionText = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                    CorrectOptionIndex = 2,
                    Explanation = "Reporting phishing emails helps prevent scams."
                },
                new QuizQuestion
                {
                    QuestionText = "True or False: It is safe to use the same password for all accounts.",
                    Options = new List<string> { "True", "False" },
                    CorrectOptionIndex = 1,
                    Explanation = "Reusing passwords increases the risk if one account is compromised."
                },
                new QuizQuestion
                {
                    QuestionText = "Which of the following is the strongest password?",
                    Options = new List<string> { "password123", "MyDog2021", "Qz$7!pL#9", "johnsmith" },
                    CorrectOptionIndex = 2,
                    Explanation = "Strong passwords use a mix of letters, numbers, and symbols, and aren't based on personal info."
                },
                new QuizQuestion
{                   QuestionText = "True or False: Clicking on pop-up ads can expose your device to malware.",
                    Options = new List<string> { "True", "False" },
                    CorrectOptionIndex = 0,
                    Explanation = "True. Many pop-ups contain malicious code or redirect to unsafe websites."
                },
                new QuizQuestion
                {
                    QuestionText = "Which of these is a sign of a phishing website?",
                    Options = new List<string> { "The URL begins with https", "It has a padlock icon", "It asks for your login info on a strange URL", "It loads slowly" },
                    CorrectOptionIndex = 2,
                    Explanation = "Phishing sites often mimic real sites but use fake or strange URLs to steal your credentials."
                },
                new QuizQuestion
                {
                    QuestionText = "True or False: You should install software updates as soon as they become available.",
                    Options = new List<string> { "True", "False" },
                    CorrectOptionIndex = 0,
                    Explanation = "True. Updates often contain important security patches to fix vulnerabilities."
                },
                new QuizQuestion
                {
                    QuestionText = "Which of these is the safest way to connect to the internet in a public place?",
                    Options = new List<string> { "Free public Wi-Fi", "Using a VPN on public Wi-Fi", "Bluetooth tethering", "Public Ethernet port" },
                    CorrectOptionIndex = 1,
                    Explanation = "Using a VPN encrypts your data, protecting you from eavesdroppers on public networks."
                },
                new QuizQuestion
                {
                    QuestionText = "What is the primary purpose of two-factor authentication (2FA)?",
                    Options = new List<string> { "Make logging in faster", "Backup your data", "Prevent password reuse", "Add extra security when logging in" },
                    CorrectOptionIndex = 3,
                    Explanation = "2FA adds an extra step to verify your identity, increasing login security."
                },
                new QuizQuestion
                {
                    QuestionText = "True or False: Only websites can be affected by malware, not smartphones.",
                    Options = new List<string> { "True", "False" },
                    CorrectOptionIndex = 1,
                    Explanation = "False. Smartphones can also be infected by malware through apps, links, or downloads."
                },
                new QuizQuestion
                {
                    QuestionText = "Which of these actions could help you avoid social engineering attacks?",
                    Options = new List<string> { "Sharing your passwords with trusted coworkers", "Ignoring strange requests and verifying with the source", "Clicking all links in emails", "Accepting friend requests from unknown users" },
                    CorrectOptionIndex = 1,
                    Explanation = "Always verify unusual requests through trusted channels to avoid being tricked by social engineering tactics."
                }
            };
        }

        public QuizQuestion GetCurrentQuestion()
        {
            return Questions[CurrentQuestionIndex];
        }
        //What users answer is
        public string SubmitAnswer(int selectedIndex)
        {
            var question = GetCurrentQuestion();
            bool isCorrect = selectedIndex == question.CorrectOptionIndex;

            if (isCorrect)
                Score++;

            string result = isCorrect ? "Correct!" : $"Incorrect. The correct answer is: {question.Options[question.CorrectOptionIndex]}";
            result += $"\nExplanation: {question.Explanation}";

            CurrentQuestionIndex++;
            return result;
        }

        public bool IsQuizOver()
        {
            return CurrentQuestionIndex >= Questions.Count;
        }

        public string GetFinalScore()
        {
            return $"Your Score: {Score}/{Questions.Count}\n" +
                   (Score >= 8 ? "Great job! You're a cybersecurity pro!" :
                    Score >= 5 ? "Good effort! Keep learning to stay safe online!" : "");
        }
    }
}
