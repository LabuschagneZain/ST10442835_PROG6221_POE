using System.IO;

namespace ST10442835_PROG6221_POE
{

    class Display
    {
        // Loads the content of the logo file and returns it as a string (for display in a TextBlock)
        public static string LoadLogo()
        {
            string filePath = "Logo.txt";

            try
            {
                return File.ReadAllText(filePath);
            }
            catch (FileNotFoundException)
            {
                return $"[Could not find file: {filePath}]";
            }
            catch (Exception ex)
            {
                return $"[Error occurred: {ex.Message}]";
            }
        }
    }
}
