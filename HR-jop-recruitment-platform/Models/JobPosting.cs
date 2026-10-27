namespace HR_jop_recruitment_platform.Models
{
    public class JobPosting
    {
        public int JobPostingID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public double SalaryRange { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.Now;
    }
}
