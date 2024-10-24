using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using ClaimManagementSystem.Models;


public class ClaimsController : Controller
{
    private readonly ClaimsApplicationDbContext _context;

    public ClaimsController(ClaimsApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("CreateClaim")]
    public IActionResult CreateClaim()
    {
        return View();
    }

    // GET: Claims/ListClaims
    public async Task<IActionResult> ListClaims(string? searchString, string? sortOrder)
    {
        var claims = _context.Claims.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            claims = claims.Where(c => c.AdditionalNotes.Contains(searchString));
        }

        // Sorting logic
        switch (sortOrder)
        {
            case "date":
                claims = claims.OrderBy(c => c.Timestamp); // Assuming you have a Timestamp property
                break;
            case "amount":
                claims = claims.OrderBy(c => c.HourlyRate);
                break;
            default:
                claims = claims.OrderBy(c => c.Id); // Default sorting
                break;
        }

        var claimList = await claims.ToListAsync();
        return View(claimList);
    }


    // POST: Claims/Create
    [HttpPost("CreateClaim")]
    public async Task<IActionResult> CreateClaim(ClaimViewModel model)
    {
        if (ModelState.IsValid)
        {
            var claim = new Claim
            {
                HoursWorked = model.HoursWorked,
                HourlyRate = model.HourlyRate,
                AdditionalNotes = model.AdditionalNotes,
                LecturerName = model.LecturerName,
                DocumentPath = string.Join(",", await UploadDocuments(model.SupportingDocuments)), // Handle multiple files
                Timestamp = DateTime.UtcNow // Set the timestamp here
            };

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Claim created successfully!";
            return RedirectToAction(nameof(ListClaims));
        }

        return View(model);
    }

    private async Task<string> UploadDocument(IFormFile? supportingDocuments)
    {
        throw new NotImplementedException();
    }

    //Method to upload Documents
    private async Task<string> UploadDocument(IFormFile[]? supportingDocuments)
    {
        throw new NotImplementedException();
    }

    // Method to handle file uploads
    private async Task<List<string>> UploadDocuments(IFormFile[] supportingDocuments)
    {
        var documentPaths = new List<string>();
        if (supportingDocuments == null || supportingDocuments.Length == 0)
        {
            return documentPaths; // Return an empty list if no documents
        }

        // Define the upload folder path
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

        // Ensure the directory exists
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        foreach (var file in supportingDocuments)
        {
            if (file != null && file.Length > 0)
            {
                // Create a unique filename to avoid conflicts
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Store the relative path for database storage
                documentPaths.Add($"/uploads/{uniqueFileName}");
            }
        }

        return documentPaths;
    }

    // GET: Claims/Edit/5
    public async Task<IActionResult> EditClaim(int id)
    {
        var claim = await _context.Claims.FindAsync(id);
        if (claim == null)
        {
            return NotFound();
        }

        var model = new ClaimViewModel
        {
            Id = claim.Id, // Ensure you have Id in your ClaimViewModel
            HoursWorked = claim.HoursWorked,
            HourlyRate = claim.HourlyRate,
            AdditionalNotes = claim.AdditionalNotes,
            // DocumentPath can be set if needed for display purposes but not required for editing
        };

        return View(model);
    }

    // POST: Claims/Edit/5
    [HttpPost]
    public async Task<IActionResult> EditClaim(ClaimViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Retrieve the claim from the database using the ID from the model
            var claim = await _context.Claims.FindAsync(model.Id);
            if (claim == null) return NotFound(); // Ensure claim exists

            // Update properties of the claim
            claim.HoursWorked = model.HoursWorked;
            claim.HourlyRate = model.HourlyRate;
            claim.AdditionalNotes = model.AdditionalNotes;

            // Handle document uploads if necessary
            claim.DocumentPath = string.Join(",", await UploadDocuments(model.SupportingDocuments));

            await _context.SaveChangesAsync();

            // Log the action
            await LogAudit("Edit Claim", User.Identity.Name, $"Claim ID: {claim.Id} edited.");

            TempData["SuccessMessage"] = "Claim updated successfully!";
            return RedirectToAction(nameof(ListClaims));
        }

        return View(model);
    }

    private async Task<object?[]> UploadDocuments(IFormFile? supportingDocuments)
    {
        throw new NotImplementedException();
    }

    // GET: Claims/Delete/5
    public async Task<IActionResult> DeleteClaim(int id)
    {
        var claim = await _context.Claims.FindAsync(id);
        if (claim == null)
        {
            return NotFound();
        }

        return View(claim);
    }

    // POST: Claims/Delete/5
    [HttpPost, ActionName("DeleteClaim")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var claim = await _context.Claims.FindAsync(id);
        if (claim != null)
        {
            _context.Claims.Remove(claim);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(ListClaims));
    }

    //
    public async Task<IActionResult> ExportClaims()
    {
        var claims = await _context.Claims.ToListAsync();

        // Convert to CSV format (or any other format you prefer)
        var csv = new StringBuilder();
        csv.AppendLine("Id,Hours Worked,Hourly Rate,Additional Notes");

        foreach (var claim in claims)
        {
            csv.AppendLine($"{claim.Id},{claim.HoursWorked},{claim.HourlyRate},{claim.AdditionalNotes}");
        }

        var bytes = Encoding.UTF8.GetBytes(csv.ToString());
        return File(bytes, "text/csv", "claims.csv");
    }

    // GET: Claims/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var claim = await _context.Claims.FindAsync(id);
        if (claim == null)
        {
            return NotFound();
        }

        return View(claim); // Pass the claim object to the view
    }

    //
    private async Task LogAudit(string action, string userId, string details)
    {
        var auditLog = new AuditLog
        {
            Action = action,
            UserId = userId,
            Timestamp = DateTime.UtcNow,
            Details = details
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
    }
}