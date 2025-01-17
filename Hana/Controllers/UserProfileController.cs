using Hana.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Hana.Hana.Database.Data;

namespace Hana.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly HanaContext _context;
        private readonly UserManager<UserIdentity> _userManager;

        public UserProfileController(HanaContext context, UserManager<UserIdentity> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserProfile userProfile)
        {
            // Remove validation for UserId and User since we'll set them
            ModelState.Remove("UserId");
            ModelState.Remove("User");

            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    ModelState.AddModelError(string.Empty, modelError.ErrorMessage);
                }
                return View(userProfile);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Check if profile already exists
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
                
                _context.Update(existingProfile);
            }
            else
            {
                // Set the UserId before creating new profile
                userProfile.UserId = user.Id;
                userProfile.User = user;
                _context.Add(userProfile);
            }

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyProfile));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error saving profile: " + ex.Message);
                return View(userProfile);
            }
        }

        public async Task<IActionResult> ShowUsers()
        {
            var profiles = await _context.UserProfiles
                .Include(p => p.User)
                .ToListAsync();
            return View(profiles);
        }
    }
}