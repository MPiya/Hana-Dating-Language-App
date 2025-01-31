using Hana.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Hana.Hana.Database.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using AspNetCoreGeneratedDocument;
using System.Security.Claims;

namespace Hana.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly HanaContext _context;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserProfileController(HanaContext context, UserManager<UserIdentity> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> MyProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
            {
                return RedirectToAction(nameof(Create));
            }

            return View(profile);
        }

        //[HttpGet("ViewProfile/{id}")]
    [HttpGet("ViewProfile/{id}")]
        public async Task<IActionResult> ViewUserProfile(int id)
        {
            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.Id == id);

            if (profile == null)
            {
                return NotFound();
            }
                    return View("MyProfile", profile);
                }






        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserProfile userProfile, List<IFormFile> profilePictures)
        {
            ModelState.Remove("UserId");
            ModelState.Remove("User");

            if (!ModelState.IsValid || profilePictures.Count < 1 || profilePictures.Count > 5)
            {
                ModelState.AddModelError(string.Empty, "Please upload between 1 and 5 images.");
                return View(userProfile);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var existingProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (existingProfile != null)
            {
                // Update existing profile
                existingProfile.Name = userProfile.Name;
                existingProfile.Age = userProfile.Age;
                existingProfile.Bio = userProfile.Bio;
                existingProfile.Interest = userProfile.Interest;
                existingProfile.Nationality = userProfile.Nationality;
                existingProfile.SpeakLanguage = userProfile.SpeakLanguage;
                existingProfile.LearnLanguage = userProfile.LearnLanguage;
                existingProfile.Gender = userProfile.Gender;
                existingProfile.InstagramAccount = userProfile.InstagramAccount;

                // Handle file uploads
                foreach (var file in profilePictures)
                {
                    if (file.Length > 0 && file.Length <= 1 * 1024 * 1024) // Limit file size to 1MB
                    {
                        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                        if (new[] { ".jpg", ".jpeg", ".png" }.Contains(extension))
                        {
                            var fileName = $"{Guid.NewGuid()}{extension}";
                            var directoryPath = Path.Combine("wwwroot", "images", "profiles");
                            var filePath = Path.Combine(directoryPath, fileName);

                            // Ensure the directory exists
                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            existingProfile.ImageUrls.Add("/images/profiles/" + fileName);
                        }
                    }
                }

                _context.Update(existingProfile);
            }
            else
            {
                userProfile.UserId = user.Id;
                userProfile.User = user;

                // Handle file uploads
                foreach (var file in profilePictures)
                {
                    if (file.Length > 0 && file.Length <= 1 * 1024 * 1024) // Limit file size to 1MB
                    {
                        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                        if (new[] { ".jpg", ".jpeg", ".png" }.Contains(extension))
                        {
                            var fileName = $"{Guid.NewGuid()}{extension}";
                            var filePath = Path.Combine("wwwroot/images/profiles", fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            userProfile.ImageUrls.Add("/images/profiles/" + fileName);
                        }
                    }
                }

                _context.Add(userProfile);
            }

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyProfile));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the profile.");
                return View(userProfile);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var profile = await _context.UserProfiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
            {
                return RedirectToAction(nameof(Create));
            }

            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserProfile userProfile, List<IFormFile> profilePictures)
        {
            ModelState.Remove("UserId");
            ModelState.Remove("User");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var existingProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            // Check total number of images
            int currentImageCount = existingProfile.ImageUrls.Count;
            if (currentImageCount + profilePictures.Count > 5)
            {
                ModelState.AddModelError("", $"Maximum 5 pictures allowed. You currently have {currentImageCount} pictures.");
                return View(existingProfile);
            }

            if (!ModelState.IsValid)
            {
                return View(existingProfile);
            }

            if (existingProfile != null)
            {
                // Update existing profile
                existingProfile.Name = userProfile.Name;
                existingProfile.Age = userProfile.Age;
                existingProfile.Bio = userProfile.Bio;
                existingProfile.Interest = userProfile.Interest;
                existingProfile.Nationality = userProfile.Nationality;
                existingProfile.SpeakLanguage = userProfile.SpeakLanguage;
                existingProfile.LearnLanguage = userProfile.LearnLanguage;
                existingProfile.Gender = userProfile.Gender;
                existingProfile.InstagramAccount = userProfile.InstagramAccount;

                // Handle file uploads
                foreach (var file in profilePictures)
                {
                    if (file.Length > 0 && file.Length <= 1 * 1024 * 1024) // Limit file size to 1MB
                    {
                        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                        if (new[] { ".jpg", ".jpeg", ".png" }.Contains(extension))
                        {
                            var fileName = $"{Guid.NewGuid()}{extension}";
                            var directoryPath = Path.Combine("wwwroot", "images", "profiles");
                            var filePath = Path.Combine(directoryPath, fileName);

                            // Ensure the directory exists
                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            existingProfile.ImageUrls.Add("/images/profiles/" + fileName);
                        }
                    }
                }

                _context.Update(existingProfile);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(MyProfile));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage([FromBody] string imageUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (profile == null)
            {
                return NotFound();
            }
                
            if (profile.ImageUrls.Remove(imageUrl))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }



        public async Task<IActionResult> ShowUsers()
        {
            var profiles = await _context.UserProfiles
                .Include(p => p.User)
                .ToListAsync();
            return View(profiles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateImageOrder([FromBody] UpdateImageOrderRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);
            if (profile == null) return NotFound();

            profile.ImageUrls = request.ImageUrls;
            await _context.SaveChangesAsync();
            
            return Ok();
        }

        public class UpdateImageOrderRequest
        {
            public List<string> ImageUrls { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CropImage([FromForm] IFormFile image, [FromForm] string imageUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);
            if (profile == null) return NotFound();

            try
            {
                // Generate a new filename for the cropped version
                var originalFileName = Path.GetFileNameWithoutExtension(imageUrl);
                var extension = Path.GetExtension(imageUrl);
                var croppedFileName = $"{originalFileName}_cropped{extension}";
                var newPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profiles", croppedFileName);
                
                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Update the ImageUrls to include cropped version
                var index = profile.ImageUrls.IndexOf(imageUrl);
                if (index != -1)
                {
                    profile.ImageUrls[index] = $"/images/profiles/{croppedFileName}";
                    await _context.SaveChangesAsync();
                }

                return Ok(new { croppedUrl = $"/images/profiles/{croppedFileName}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}