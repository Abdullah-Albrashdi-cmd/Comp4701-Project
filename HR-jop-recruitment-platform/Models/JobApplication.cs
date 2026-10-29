using System.ComponentModel.DataAnnotations;

namespace HR_jop_recruitment_platform.Models;

public class JobApplication
{
    public int Id { get; }
    public int JobPostingId { get; }
    public int CandidateId { get; }
    public JobPosting JobPosting { get; }
    public Candidate Candidate { get; }

    [StringLength(1000)]
    public string? CoverLetter { get; }

    public DateTime AppliedDate { get; } = DateTime.UtcNow;
    public ApplicationStatus Status { get; private set; } = ApplicationStatus.Pending;

    public JobApplication(int id, JobPosting jobPosting, Candidate candidate, string? coverLetter)
    {
        Id = id;
        JobPosting = jobPosting;
        Candidate = candidate;
        JobPostingId = jobPosting.Id;
        CandidateId = candidate.Id;
        CoverLetter = coverLetter?.Trim() ?? string.Empty;
    }

    public void UpdateStatus(ApplicationStatus status)
    {
        if (!Enum.IsDefined(status))
        {
            throw new ArgumentOutOfRangeException(nameof(status), "Choose a valid application status.");
        }

        Status = status;
    }
}
