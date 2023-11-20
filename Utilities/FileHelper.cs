namespace OllamaTot.Utilities
{
  public static class FileHelper
  {
    public static string ReadPromptFile(string filename)
    {
      var path = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", filename);
      return System.IO.File.ReadAllText(path);
    }
  }
}
