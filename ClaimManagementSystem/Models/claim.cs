using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class Claim // Changed from 'claim' to 'Claim'
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Hours Worked is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Hours Worked must be greater than 0.")]
    public int HoursWorked { get; set; }

    [Required(ErrorMessage = "Hourly Rate is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Hourly Rate must be greater than 0.")]
    public decimal HourlyRate { get; set; }

    [MaxLength(500, ErrorMessage = "Additional Notes cannot exceed 500 characters.")]
    public string? AdditionalNotes { get; set; }

    public string? DocumentPath { get; set; } // Path to any uploaded documents
    public DateTime Timestamp { get; internal set; }

    [Required(ErrorMessage = "Lecturer Name is required.")] // Add validation if necessary
    public string LecturerName { get; internal set; } // Consider making this public if you need to set it externally

    // ClaimViewModel is designed to facilitate data transfer between the view and controller
    public class ClaimViewModel
    {
        [Required(ErrorMessage = "Hours Worked is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Hours Worked must be greater than 0.")]
        public int HoursWorked { get; set; }

        [Required(ErrorMessage = "Hourly Rate is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Hourly Rate must be greater than 0.")]
        public decimal HourlyRate { get; set; }

        [MaxLength(500, ErrorMessage = "Additional Notes cannot exceed 500 characters.")]
        public string? AdditionalNotes { get; set; }

        [Required(ErrorMessage = "Supporting Documents are required.")]
        public IFormFile[]? SupportingDocuments { get; set; } // Change to array for multiple uploads

        [Required(ErrorMessage = "Lecturer Name is required.")] // Ensure this is included
        public string LecturerName { get; set; } // Ensure this property exists
    }

    public Claim() { } // Constructor

    public Claim(string email1, string? email2)
    {
        // Constructor logic if needed
    }
}