using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace ST10442835_PROG6221_POE
{
    internal class PlayAudio
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
