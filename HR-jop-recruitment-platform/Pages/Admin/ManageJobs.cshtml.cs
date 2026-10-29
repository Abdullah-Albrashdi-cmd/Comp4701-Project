using HR_jop_recruitment_platform.Models;
using HR_jop_recruitment_platform.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HR_jop_recruitment_platform.Pages.Admin;

public class ManageJobsModel(RecruitmentStore store) : PageModel
{
    [BindProperty]
    public JobPosting NewJob { get; set; } = new();

    public IReadOnlyList<JobPosting> Jobs { get; private set; } = [];
    public IReadOnlyList<JobApplication> Applications { get; private set; } = [];
    public IReadOnlyList<ApplicationStatus> Statuses { get; } = Enum.GetValues<ApplicationStatus>();
    [TempData]
    public string? Message { get; set; }

    public void OnGet()
    {
        Jobs = store.GetJobs();
        Applications = store.GetAllApplications();
    }

    public IActionResult OnPostCreate()
    {
        if (!ModelState.IsValid)
        {
            OnGet();
            return Page();
        }

        store.AddJob(NewJob);
        Message = "The job was added.";
        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int id)
    {
        try
        {
            store.DeleteJob(id);
            Message = "The job was closed.";
        }
        catch (KeyNotFoundException exception)
        {
            Message = exception.Message;
        }

        return RedirectToPage();
    }

    public IActionResult OnPostStatus(int applicationId, ApplicationStatus status)
    {
        if ((ModelState.TryGetValue(nameof(status), out var state) && state.Errors.Count > 0) || !Enum.IsDefined(status))
        {
            Message = "Choose a valid application status.";
            return RedirectToPage();
        }

        try
        {
            store.UpdateApplicationStatus(applicationId, status);
            Message = "The application status was updated.";
        }
        catch (Exception exception) when (exception is KeyNotFoundException or ArgumentOutOfRangeException)
        {
            Message = exception.Message;
        }

        return RedirectToPage();
    }
}
