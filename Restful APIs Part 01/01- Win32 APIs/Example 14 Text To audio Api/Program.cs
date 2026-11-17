using System;
using System.Speech.Synthesis;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the eBook Reader with Speak Aloud feature!");

        string sampleText = "brahim Elkhattab";

        while (true)
        {
            Console.WriteLine("\nOptions:");
            Console.WriteLine("1: Speak Aloud");
            Console.WriteLine("2: Change Voice");
            Console.WriteLine("3: Exit");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    SpeakAloud(sampleText);
                    break;
                case "2":
                    ChangeVoice();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void SpeakAloud(string text)
    {
        using (SpeechSynthesizer synth = new SpeechSynthesizer())
        {
            synth.SetOutputToDefaultAudioDevice();
            synth.Speak(text);
        }
    }

    static void ChangeVoice()
    {
        using (SpeechSynthesizer synth = new SpeechSynthesizer())
        {
            Console.WriteLine("Available voices:");
            int index = 1;
            foreach (var voice in synth.GetInstalledVoices())
            {
                Console.WriteLine($"{index}: {voice.VoiceInfo.Name}");
                index++;
            }

            Console.WriteLine("Enter the number of the voice you want to use:");
            if (int.TryParse(Console.ReadLine(), out int voiceChoice) && voiceChoice > 0 && voiceChoice <= index)
            {
                var chosenVoice = synth.GetInstalledVoices()[voiceChoice - 1].VoiceInfo.Name;
                synth.SelectVoice(chosenVoice);
                Console.WriteLine($"Voice changed to {chosenVoice}");
            }
            else
            {
                Console.WriteLine("Invalid choice. Using default voice.");
            }
        }
    }
}
//------------------------------------------------------------------
//وهذي طريقة اضافة ال reference  
//Add the System.Speech Assembly Reference:

//Right - click on your project in Solution Explorer.
//Select Add > Reference....
//In the Reference Manager window, go to the Assemblies section.
//Look for System.Speech. If you find it, check the box next to it and click OK.
//If you don't find it in the Assemblies list, you may need to install it via NuGet:

//Install System.Speech via NuGet (if not available in Assemblies):

//Right - click on your project in Solution Explorer.
//Choose Manage NuGet Packages....
//In the Browse tab, search for System.Speech.
//Select it from the list and click Install.
//After installation, the System.Speech namespace should be available in your project.
//Clean and Rebuild Your Project:

//After adding the reference, go to Build > Clean Solution.
//Then, Build > Rebuild Solution.