// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected ApplicationDbContext() { }

    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<Task> Tasks { get; set; }
    public virtual DbSet<TaskHistory> TaskHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurações adicionais (chaves, relacionamentos, restrições)
        modelBuilder.Entity<Task>()
            .Property(t => t.Priority)
            .IsRequired();

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
