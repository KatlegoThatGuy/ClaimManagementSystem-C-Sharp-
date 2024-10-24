using System.ComponentModel.DataAnnotations;

public class ClaimViewModel
{
    public int Id { get; set; } // Add this property if needed

    [Required(ErrorMessage = "Hours Worked is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Hours Worked must be greater than 0.")]
    public int HoursWorked { get; set; }

    [Required(ErrorMessage = "Hourly Rate is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Hourly Rate must be greater than 0.")]
    public decimal HourlyRate { get; set; }

    [MaxLength(500, ErrorMessage = "Additional Notes cannot exceed 500 characters.")]
    public string? AdditionalNotes { get; set; }

    [Required(ErrorMessage = "Lecturer Name is required.")]
    public string LecturerName { get; set; } // Ensure this property exists

    [Required(ErrorMessage = "Supporting Documents are required.")]
    public IFormFile[]? SupportingDocuments { get; set; } // Change to array for multiple uploads
}