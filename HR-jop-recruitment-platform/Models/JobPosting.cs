using System.ComponentModel.DataAnnotations;

namespace HR_jop_recruitment_platform.Models;

public class JobPosting
{
    public int Id { get; set; }

    [Required, StringLength(80, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Category { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Location { get; set; } = string.Empty;

    [Required, StringLength(1200, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    [Required, StringLength(1200, MinimumLength = 5)]
    public string Requirements { get; set; } = string.Empty;

    [Range(0, 10000000)]
    public decimal SalaryRange { get; set; }

    public DateTime PostedDate { get; private set; } = DateTime.UtcNow;
    public bool IsActive { get; private set; } = true;

    public JobPosting()
    {
    }

    public JobPosting(string title, string category, string location, string description, string requirements, decimal salaryRange)
    {
        Title = title;
        Category = category;
        Location = location;
        Description = description;
        Requirements = requirements;
        SalaryRange = salaryRange;
    }

    public void Close() => IsActive = false;
}
