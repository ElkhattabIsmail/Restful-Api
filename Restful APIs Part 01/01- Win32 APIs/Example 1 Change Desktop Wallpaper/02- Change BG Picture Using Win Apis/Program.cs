using System;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace WallpaperConsole
{
    class Program
    {
        // Import Windows API
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int SystemParametersInfo(
            int uAction,
            int uParam,
            string lpvParam,
            int fuWinIni
        );

        // Constants
        private const int SPI_SETDESKWALLPAPER = 0x0014;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDCHANGE = 0x02;
        private const string RegistryKeyPath = @"Control Panel\Desktop";
        private const string WallpaperStyleKey = "WallpaperStyle";
        private const string TileWallpaperKey = "TileWallpaper";

        // Wallpaper styles
        public enum WallpaperStyle
        {
            Centered = 1,
            Stretched = 2,
            Tiled = 3
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=== Desktop Wallpaper Changer ===");

            // Get image path from user
            Console.Write("Enter image path: ");
            string imagePath = Console.ReadLine();

            // Validate image path
            if (!System.IO.File.Exists(imagePath))
            {
                Console.WriteLine($"Error: File '{imagePath}' not found!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nChoose wallpaper style:");
            Console.WriteLine("1. Centered");
            Console.WriteLine("2. Stretched");
            Console.WriteLine("3. Tiled");
            Console.Write("Enter choice (1-3): ");

            if (!int.TryParse(Console.ReadLine(), out int styleChoice) || styleChoice < 1 || styleChoice > 3)
            {
                Console.WriteLine("Error: Invalid choice! Press any key to exit.");
                Console.ReadKey();
                return;
            }

            // Set wallpaper
            try
            {
                SetWallpaper(imagePath, (WallpaperStyle)styleChoice);
                Console.WriteLine("\nWallpaper updated successfully! Press any key to exit.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.ReadKey();
        }

        static void SetWallpaper(string imagePath, WallpaperStyle style)
        {
            // Set style via registry
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
            {
                switch (style)
                {
                    case WallpaperStyle.Centered:
                        key.SetValue(WallpaperStyleKey, "1");
                        key.SetValue(TileWallpaperKey, "0");
                        break;
                    case WallpaperStyle.Stretched:
                        key.SetValue(WallpaperStyleKey, "2");
                        key.SetValue(TileWallpaperKey, "0");
                        break;
                    case WallpaperStyle.Tiled:
                        key.SetValue(WallpaperStyleKey, "1");
                        key.SetValue(TileWallpaperKey, "1");
                        break;
                }
            }

            // Apply wallpaper
            SystemParametersInfo(
                SPI_SETDESKWALLPAPER,
                0,
                imagePath,
                SPIF_UPDATEINIFILE | SPIF_SENDCHANGE
            );
        }
    }
}