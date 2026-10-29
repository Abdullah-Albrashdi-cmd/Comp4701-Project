using System.ComponentModel.DataAnnotations;
using HR_jop_recruitment_platform.Models;
using HR_jop_recruitment_platform.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HR_jop_recruitment_platform.Pages;

public class MyApplicationsModel(RecruitmentStore store) : PageModel
{
    [BindProperty(SupportsGet = true)]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    public IReadOnlyList<JobApplication> Applications { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    [TempData]
    public string? SuccessMessage { get; set; }

    public void OnGet()
    {
        if (!ModelState.IsValid)
            ErrorMessage = "Enter a valid email address to find your applications.";
        else if (!string.IsNullOrWhiteSpace(Email))
            Applications = store.GetApplications(Email.Trim());
    }
}
