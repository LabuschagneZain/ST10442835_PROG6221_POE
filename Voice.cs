using System.Media;


namespace ST10442835PRGPOEPart2
{
    // Handles playing audio files for the chatbot
    class Voice
    {
        // Plays the pre-recorded sound file
        public void PlaySound()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("ChatBotRecording.wav");
                player.Play();
        
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't play cyberaware sound: " + ex.Message);
            }
        }
    }
}
