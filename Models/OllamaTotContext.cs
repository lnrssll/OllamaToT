using Microsoft.EntityFrameworkCore;

namespace OllamaTot.Models
{
  public class OllamaTotContext : DbContext
  {
    public OllamaTotContext(DbContextOptions<OllamaTotContext> options)
      : base(options)
    {
    }

    public DbSet<Prompt> Prompts { get; set; } = null!;
    public DbSet<TotTask> TotTasks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<TotTask>()
        .HasOne(t => t.ProposePrompt)
        .WithMany(t => t.ProposeTasks)
        .HasForeignKey(t => t.ProposePromptId);

      modelBuilder.Entity<TotTask>()
        .HasOne(t => t.EvaluatePrompt)
        .WithMany(t => t.EvaluateTasks)
        .HasForeignKey(t => t.EvaluatePromptId);
    }
  }
}
