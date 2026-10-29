using System.ComponentModel.DataAnnotations;
using HR_jop_recruitment_platform.Models;
using HR_jop_recruitment_platform.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HR_jop_recruitment_platform.Pages;

public class JobDetailsModel(RecruitmentStore store) : PageModel
{
    public JobPosting? Job { get; private set; }
    public string? ErrorMessage { get; private set; }

    [BindProperty]
    public Candidate Applicant { get; set; } = new();

    [BindProperty]
    [StringLength(1000)]
    public string? CoverLetter { get; set; }

    [TempData]
    public string? SuccessMessage { get; set; }

    public IActionResult OnGet(int id)
    {
        try
        {
            Job = store.GetJob(id);
            return Page();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    public IActionResult OnPost(int id)
    {
        try
        {
            Job = store.GetJob(id);
            if (!ModelState.IsValid)
                return Page();

            store.Apply(id, Applicant, CoverLetter);
            SuccessMessage = "Your application was submitted.";
            return RedirectToPage("/MyApplications", new { email = Applicant.Email });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException exception)
        {
            ErrorMessage = exception.Message;
            return Page();
        }
    }
}
