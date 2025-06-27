using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ST10442835_PROG6221_POE
{
    public partial class MainWindow : Window
    {
        private ChatBot bot;

        public MainWindow()
        {
            InitializeComponent();

            // Load logo and display at the top of the ScrollViewer
            string logoText = Display.LoadLogo();
            AddLogoMessage(logoText);

            PlayAudio voice = new PlayAudio();
            voice.PlaySound();

            bot = new ChatBot();
            AddMessage("Welcome to the Cybersecurity Awareness Bot!");
            AddMessage("Please enter your name to get started:");
        }

        private void AddLogoMessage(string logo)
        {
            TextBlock logoBlock = new TextBlock
            {
                Text = logo,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 14,
                Foreground = Brushes.DarkGreen,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(5),
                TextAlignment = TextAlignment.Center,       // Center-align the text inside the TextBlock
                HorizontalAlignment = HorizontalAlignment.Center // Center the TextBlock itself in its container
            };

            ChatPanel.Children.Insert(0, logoBlock); // Add logo at top of chat panel
        }


        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = UserInputBox.Text.Trim();
            if (!string.IsNullOrEmpty(userInput))
            {
                AddUserMessage(userInput);

                string response = bot.GetBotResponse(userInput);
                AddBotMessage(response);

                UserInputBox.Text = string.Empty;
            }
        }

        private void AddUserMessage(string message)
        {
            TextBlock msg = new TextBlock
            {
                Text = "You: " + message,
                Margin = new Thickness(5),
                Foreground = Brushes.Black,
                TextWrapping = TextWrapping.Wrap,       // **Enable wrapping**
                MaxWidth = 400                          // optional: limit width for better readability
            };
            ChatPanel.Children.Add(msg);
            ChatScrollViewer.ScrollToEnd();
        }

        private void AddBotMessage(string message)
        {
            TextBlock msg = new TextBlock
            {
                Text = "Bot: " + message,
                Margin = new Thickness(5),
                Foreground = Brushes.DarkGreen,
                TextWrapping = TextWrapping.Wrap,       // **Enable wrapping**
                MaxWidth = 400                          // optional: limit width
            };
            ChatPanel.Children.Add(msg);
            ChatScrollViewer.ScrollToEnd();
        }

        private void AddMessage(string message)
        {
            TextBlock msg = new TextBlock
            {
                Text = message,
                Margin = new Thickness(5),
                Foreground = Brushes.DarkGreen,
                TextWrapping = TextWrapping.Wrap,       // **Enable wrapping**
                MaxWidth = 400                          // optional: limit width
            };
            ChatPanel.Children.Add(msg);
            ChatScrollViewer.ScrollToEnd();
        }
    }
}
