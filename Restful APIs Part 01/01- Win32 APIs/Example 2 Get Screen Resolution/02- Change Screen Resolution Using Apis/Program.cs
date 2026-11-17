using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ScreenResolutionChanger
{
    class Program
    {
        #region Win32 API Declarations

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(
            string deviceName, int modeNum, ref DEVMODE devMode);

        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(
            ref DEVMODE devMode, int flags);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplayDevices(
            string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public int dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct DISPLAY_DEVICE
        {
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            public int StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        // Constants
        private const int ENUM_CURRENT_SETTINGS = -1;
        private const int DISP_CHANGE_SUCCESSFUL = 0;
        private const int DISP_CHANGE_BADMODE = -2;
        private const int DISP_CHANGE_FAILED = -1;
        private const int DISP_CHANGE_RESTART = 1;
        private const int CDS_TEST = 0x00000002;
        private const int CDS_UPDATEREGISTRY = 0x00000001;
        private const int DM_PELSWIDTH = 0x00080000;
        private const int DM_PELSHEIGHT = 0x00100000;
        private const int DM_DISPLAYFREQUENCY = 0x00400000;

        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("Screen Resolution Changer");
            Console.WriteLine("------------------------");

            // Get current resolution
            DEVMODE currentMode = GetCurrentResolution();
            Console.WriteLine($"Current Resolution: {currentMode.dmPelsWidth} x " +
                $"{currentMode.dmPelsHeight} @ {currentMode.dmDisplayFrequency}Hz");

            // List available display modes
            Console.WriteLine("\nAvailable display modes:");
            ListAvailableResolutions();

            // Change resolution example
            Console.WriteLine("\nEnter resolution to change to:");
            Console.Write("Width: ");
            int width = int.Parse(Console.ReadLine());
            Console.Write("Height: ");
            int height = int.Parse(Console.ReadLine());
            Console.Write("Refresh Rate (Hz, press Enter for default): ");
            string freqInput = Console.ReadLine();
            int frequency = string.IsNullOrEmpty(freqInput) ? 0 : int.Parse(freqInput);

            bool success = ChangeResolution(width, height, frequency);

            if (success)
            {
                Console.WriteLine($"Resolution changed to: {width} x {height}" +
                    (frequency > 0 ? $" @ {frequency}Hz" : ""));

                Console.WriteLine("\nWould you like to revert to original resolution after 10 seconds? (Y/N)");
                if (Console.ReadKey().Key == ConsoleKey.Y)
                {
                    Console.WriteLine("\nReverting in 10 seconds...");
                    System.Threading.Thread.Sleep(10000);
                    ChangeResolution(currentMode.dmPelsWidth, currentMode.dmPelsHeight, currentMode.dmDisplayFrequency);
                    Console.WriteLine("Reverted to original resolution.");
                }
            }
            else
            {
                Console.WriteLine("Failed to change resolution. The specified mode may not be supported.");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Gets the current screen resolution
        /// </summary>
        static DEVMODE GetCurrentResolution()
        {
            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref dm);
            return dm;
        }

        /// <summary>
        /// Lists all available resolutions
        /// </summary>
        static void ListAvailableResolutions()
        {
            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));

            int modeIndex = 0;
            HashSet<string> uniqueResolutions = new HashSet<string>();

            while ( EnumDisplaySettings(null, modeIndex, ref dm))
            {
                string resolution = $"{dm.dmPelsWidth} x {dm.dmPelsHeight} @ {dm.dmDisplayFrequency}Hz";
                if (uniqueResolutions.Add(resolution))
                {
                    Console.WriteLine(resolution);
                }
                modeIndex++;

                if (uniqueResolutions.Count == 105)
                    Console.WriteLine("Final.");
            }
            
        }

        /// <summary>
        /// Changes the screen resolution
        /// </summary>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="frequency">Refresh rate in Hz (0 for default)</param>
        /// <returns>True if successful, false otherwise</returns>
        static bool ChangeResolution(int width, int height, int frequency)
        {
            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));

            // Get current settings as a starting point
            if (!EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref dm))
            {
                return false;
            }

            // Set requested values
            dm.dmPelsWidth = width;
            dm.dmPelsHeight = height;

            // Only set frequency if provided
            if (frequency > 0)
            {
                dm.dmDisplayFrequency = frequency;
                dm.dmFields = DM_PELSWIDTH | DM_PELSHEIGHT | DM_DISPLAYFREQUENCY;
            }
            else
            {
                dm.dmFields = DM_PELSWIDTH | DM_PELSHEIGHT;
            }

            // Test if the mode is valid
            int result = ChangeDisplaySettings(ref dm, CDS_TEST);
            if (result != DISP_CHANGE_SUCCESSFUL)
            {
                return false;
            }

            // Apply the changes
            result = ChangeDisplaySettings(ref dm, CDS_UPDATEREGISTRY);
            return (result == DISP_CHANGE_SUCCESSFUL);
        }
    }
}