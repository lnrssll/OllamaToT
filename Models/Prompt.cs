namespace OllamaTot.Models
{
  public class Prompt
  {
    public int Id { get; set; }
    public string Content { get; set; } = null!;

    public virtual ICollection<TotTask> ProposeTasks { get; set; }
      = new List<TotTask>();
    public virtual ICollection<TotTask> EvaluateTasks { get; set; }
      = new List<TotTask>();
  }
}
