using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hana.Hana.Database.Data;
using Hana.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hana.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly HanaContext _context;
        private readonly UserManager<UserIdentity> _userManager;

        public MessageController(HanaContext context, UserManager<UserIdentity> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(string receiverId, string message, string socialMediaPlatform, string socialMediaLink)
        {
            if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(receiverId))
            {
                TempData["Error"] = "Message and receiver are required.";
                return RedirectToAction("MyProfile", "UserProfile");
            }

            var sender = await _userManager.GetUserAsync(User);
            if (sender == null)
            {
                return NotFound("Sender not found.");
            }

            // Get the receiver's profile to get their Id for redirection
            var receiverProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == receiverId);

            if (receiverProfile == null)
            {
                TempData["Error"] = "Receiver not found.";
                return RedirectToAction("MyProfile", "UserProfile");
            }

            var newMessage = new Message
            {
                SenderId = sender.Id,
                ReceiverId = receiverId,
                Content = message,
                SocialMediaPlatform = socialMediaPlatform,
                SocialMediaLink = socialMediaLink,
                CreatedAt = DateTime.UtcNow
            };

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Message sent successfully!";
            return RedirectToAction("ViewUserProfile", "UserProfile", new { id = receiverProfile.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GetMyMessages()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var messages = await _context.Messages
                .Include(m => m.Sender)
                .Where(m => m.ReceiverId == user.Id)
                .OrderByDescending(m => m.CreatedAt)
                .Select(m => new
                {
                    m.Id,
                    m.Content,
                    m.SocialMediaPlatform,
                    m.SocialMediaLink,
                    m.CreatedAt,
                    Sender = new
                    {
                        m.Sender.Id,
                        m.Sender.UserName,
                        m.Sender.Email
                    }
                })
                .ToListAsync();

            return Json(messages);
        }
    }
}