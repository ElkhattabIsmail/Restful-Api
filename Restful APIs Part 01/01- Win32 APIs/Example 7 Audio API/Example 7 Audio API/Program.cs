using NAudio.CoreAudioApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//To control the volume of the sound in a Windows application using C#, you can use the coreaudioapi which is part of the Windows API. This approach involves interacting with the system's audio session to adjust the volume levels.

//Here's a basic example of how to increase the volume using C#. You'll need to add a reference to the CoreAudioApi which is available through the NAudio package, a popular .NET audio library.

//First, you'll need to install the NAudio package if you haven't already:

//Install - Package NAudio


//The command Install-Package NAudio is used in the Package Manager Console within Visual Studio. Here’s how you can run it:

//Open Visual Studio: Start Visual Studio and open your project.
//Open Package Manager Console: Go to the menu bar, click on Tools, then NuGet Package Manager, and select Package Manager Console. This will open a console window at the bottom or side of your Visual Studio window.
//Install the Package: Type Install-Package NAudio into the console and press Enter. This will download and install the NAudio package into your project.
//This will set up the NAudio library in your project, allowing you to use its functionalities for managing audio, such as controlling the volume.
namespace Audio_Control_Api
{
    class Program
    {
        static MMDeviceEnumerator enumerator;
        static MMDevice device;

        static void Main(string[] args)
        {
            Console.WriteLine("Audio Control Console Application");
            Console.WriteLine("Press '+' to increase volume, '-' to decrease volume, 's' to set a specific volume, 'q' to quit.");

            // Initialize audio device enumerator and get the default audio endpoint
            enumerator = new MMDeviceEnumerator();
            device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            while (true)
            {
                // Get the user input for volume control
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.OemPlus || key == ConsoleKey.Add) // '+' Key
                {
                    AdjustVolume(0.02f); // Increase volume by 2%
                }
                else if (key == ConsoleKey.OemMinus || key == ConsoleKey.Subtract) // '-' Key
                {
                    AdjustVolume(-0.02f); // Decrease volume by 2%
                }
                else if (key == ConsoleKey.S) // 's' Key
                {
                    SetSpecificVolume(); // Set a specific volume
                }
                else if (key == ConsoleKey.Q) // 'q' Key
                {
                    Console.WriteLine("Exiting program...");
                    break;
                }
            }
        }

        // Adjust the volume by a given percentage (+ or -)
        static void AdjustVolume(float adjustment)
        {
            float currentVolume = device.AudioEndpointVolume.MasterVolumeLevelScalar;
            float newVolume = currentVolume + adjustment;

            // Clamp the new volume to a valid range [0.0, 1.0]
            newVolume = Clamp(newVolume, 0.0f, 1.0f);

            // Set the new volume level
            device.AudioEndpointVolume.MasterVolumeLevelScalar = newVolume;

            // Display the updated volume level
            Console.WriteLine($"Volume adjusted to: {newVolume * 100:F0}%");
        }

        // Clamp the volume value between 0.0 and 1.0
        static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        // Set a specific volume level (from 0 to 100 percent)
        static void SetSpecificVolume()
        {
            Console.Write("Enter the desired volume percentage (0-100): ");
            string input = Console.ReadLine();

            if (float.TryParse(input, out float percentage))
            {
                if (percentage >= 0 && percentage <= 100)
                {
                    // Convert percentage to scalar value between 0.0 and 1.0
                    float newVolume = percentage / 100.0f;

                    // Clamp volume to the valid range [0.0, 1.0]
                    newVolume = Clamp(newVolume, 0.0f, 1.0f);

                    // Set the new volume
                    device.AudioEndpointVolume.MasterVolumeLevelScalar = newVolume;

                    // Display the updated volume level
                    Console.WriteLine($"Volume set to: {newVolume * 100:F0}%");
                }
                else
                {
                    Console.WriteLine("Please enter a valid percentage between 0 and 100.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
    }
}