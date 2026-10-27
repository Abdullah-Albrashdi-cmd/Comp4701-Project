using HR_jop_recruitment_platform.Models;
using HR_jop_recruitment_platform.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HR_jop_recruitment_platform.Pages;

public class IndexModel(RecruitmentStore store) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;

    public IReadOnlyList<JobPosting> Jobs { get; private set; } = [];

    public void OnGet() => Jobs = store.GetJobs(SearchTerm);
}
