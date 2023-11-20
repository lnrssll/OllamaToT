namespace OllamaTot.Models
{
  public class TotTask
  {
    public int Id { get; set; }
    public string Content { get; set; } = null!;

    public int ProposePromptId { get; set; }
    public Prompt ProposePrompt { get; set; } = null!;

    public int EvaluatePromptId { get; set; }
    public Prompt EvaluatePrompt { get; set; } = null!;

    public string? Response { get; set; }
  }
}
