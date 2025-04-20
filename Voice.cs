using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;


namespace ST10442835PRGPOEPart1
{
    class Voice
    {
        public void PlaySound()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("ChatBotRecording.wav");
                player.Play();
        
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't play welcome sound: " + ex.Message);
            }
        }
    }
}
