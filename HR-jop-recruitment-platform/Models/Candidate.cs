using System.ComponentModel.DataAnnotations;

namespace HR_jop_recruitment_platform.Models;

public class Candidate
{
    public int Id { get; private set; }

    [Required, StringLength(80, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [Required, RegularExpression(@"^[0-9+() -]{7,20}$", ErrorMessage = "Enter a valid phone number.")]
    public string Phone { get; set; } = string.Empty;

    public Candidate()
    {
    }

    public Candidate(int id, string fullName, string email, string phone)
    {
        Id = id;
        UpdateProfile(fullName, email, phone);
    }

    public void UpdateProfile(string fullName, string email, string phone)
    {
        FullName = fullName.Trim();
        Email = email.Trim();
        Phone = phone.Trim();
    }
}
