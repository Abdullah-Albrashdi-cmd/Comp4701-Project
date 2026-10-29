using HR_jop_recruitment_platform.Models;

namespace HR_jop_recruitment_platform.Services;

public class RecruitmentStore
{
    private readonly List<JobPosting> _jobs =
    [
        new("Junior Web Developer", "Technology", "Muscat", "Build and maintain responsive web applications with our product team.", "C# or JavaScript basics; interest in web development.", 800) { Id = 1 },
        new("HR Coordinator", "Human Resources", "Muscat", "Support recruitment, interviews, and employee records.", "Strong communication and organization skills.", 650) { Id = 2 },
        new("Business Analyst", "Business", "Sohar", "Work with teams to document needs and improve business processes.", "Analytical thinking and clear written communication.", 900) { Id = 3 }
    ];

    private readonly List<Candidate> _candidates = [];
    private readonly List<JobApplication> _applications = [];
    private int _nextJobId = 4;
    private int _nextCandidateId = 1;
    private int _nextApplicationId = 1;

    public IReadOnlyList<JobPosting> GetJobs(string? search = null)
    {
        var jobs = _jobs.Where(job => job.IsActive);
        if (!string.IsNullOrWhiteSpace(search))
        {
            jobs = jobs.Where(job =>
                job.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                job.Category.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                job.Location.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        return jobs.OrderByDescending(job => job.PostedDate).ToList();
    }

    public JobPosting GetJob(int id) =>
        _jobs.FirstOrDefault(job => job.Id == id && job.IsActive)
        ?? throw new KeyNotFoundException("That job is no longer available.");

    public JobPosting AddJob(JobPosting job)
    {
        job.Id = _nextJobId++;
        _jobs.Add(job);
        return job;
    }

    public void DeleteJob(int id)
    {
        var job = GetJob(id);
        job.Close();
        _applications.RemoveAll(application => application.JobPostingId == id);
    }

    public JobApplication Apply(int jobId, Candidate details, string? coverLetter)
    {
        var job = GetJob(jobId);
        if (_applications.Any(application =>
                application.JobPostingId == jobId &&
                string.Equals(application.Candidate.Email, details.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("You have already applied for this job.");
        }

        var candidate = _candidates.FirstOrDefault(existing =>
            string.Equals(existing.Email, details.Email, StringComparison.OrdinalIgnoreCase));
        if (candidate is null)
        {
            candidate = new Candidate(_nextCandidateId++, details.FullName, details.Email, details.Phone);
            _candidates.Add(candidate);
        }
        else
        {
            candidate.UpdateProfile(details.FullName, details.Email, details.Phone);
        }

        var application = new JobApplication(_nextApplicationId++, job, candidate, coverLetter);
        _applications.Add(application);
        return application;
    }

    public IReadOnlyList<JobApplication> GetApplications(string email) =>
        _applications
            .Where(application => string.Equals(application.Candidate.Email, email, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(application => application.AppliedDate)
            .ToList();

    public IReadOnlyList<JobApplication> GetAllApplications() =>
        _applications.OrderByDescending(application => application.AppliedDate).ToList();

    public void UpdateApplicationStatus(int applicationId, ApplicationStatus status)
    {
        var application = _applications.FirstOrDefault(item => item.Id == applicationId)
            ?? throw new KeyNotFoundException("That application could not be found.");
        application.UpdateStatus(status);
    }
}
