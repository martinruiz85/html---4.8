using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using UtilETWeb;

//https://codigofuentenet.wordpress.com/2012/09/15/reconocimiento-de-voz-y-texto-en-c/
//http://sg.com.mx/revista/29/mas-alla-del-teclado-usando-voz-tacto-y-escritura#.V3K8ONJ95dg
//http://www.cepstral.com/es/personal/download
//http://stackoverflow.com/questions/34776593/speechsynthesizer-selectvoice-fails-with-no-matching-voice-is-installed-or-th
//http://10rem.net/blog/2009/12/16/using-speech-synthesis-in-net-4-and-windows-7
namespace html
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            // Initialize a new instance of the SpeechSynthesizer.
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {

                // Output information about all of the installed voices. 
                Console.WriteLine("Installed voices -");
                foreach (InstalledVoice voice in synth.GetInstalledVoices())
                {
                    VoiceInfo info = voice.VoiceInfo;
                    string AudioFormats = "";
                    foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                    {
                        AudioFormats += String.Format("{0}\n",
                        fmt.EncodingFormat.ToString());
                    }

                    Console.WriteLine(" Name:          " + info.Name);
                    Console.WriteLine(" Culture:       " + info.Culture);
                    Console.WriteLine(" Age:           " + info.Age);
                    Console.WriteLine(" Gender:        " + info.Gender);
                    Console.WriteLine(" Description:   " + info.Description);
                    Console.WriteLine(" ID:            " + info.Id);
                    Console.WriteLine(" Enabled:       " + voice.Enabled);
                    if (info.SupportedAudioFormats.Count != 0)
                    {
                        Console.WriteLine(" Audio formats: " + AudioFormats);
                    }
                    else
                    {
                        Console.WriteLine(" No supported audio formats found");
                    }

                    string AdditionalInfo = "";
                    foreach (string key in info.AdditionalInfo.Keys)
                    {
                        AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
                    }

                    Console.WriteLine(" Additional Info - " + AdditionalInfo);
                    Console.WriteLine();
                }
            }
            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();



            //// Initialize a new instance of the SpeechSynthesizer.
            //SpeechSynthesizer synth = new SpeechSynthesizer();

            //// Configure the audio output. 
            //synth.SetOutputToDefaultAudioDevice();
            ////synth.SelectVoice("Cepstral Alejandra");
            //// Speak a string.
            ////synth.Speak("This example demonstrates a basic use of Speech Synthesizer");
            //synth.Speak("se han generado 1234 recursos");

            ////Console.WriteLine();
            ////Console.WriteLine("Press any key to exit...");
            ////Console.ReadKey();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
            
        }
    }
}
