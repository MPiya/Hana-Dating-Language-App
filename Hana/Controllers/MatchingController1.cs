using Hana.Hana.Database.Data;
using Hana.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Hana.Controllers
{

    public class MatchingController : Controller
    {
        private readonly HanaContext _context;

        public MatchingController(HanaContext context)
        {
            _context = context;
        }

        #region ApiEndpoints

        // Action method to serve the API endpoint for searching matched users
        [HttpGet("api/search")]
        public async Task<ActionResult<IEnumerable<UserProfile>>> SearchUsersApi(string nationality, string gender)
        {
            try
            {
                var matchedUsers = await _context.UserProfiles
                    .Where(u => u.Nationality == nationality && u.Gender == gender)
                    .ToListAsync();

                if (matchedUsers == null || matchedUsers.Count == 0)
                {
                    return NotFound("No users found with the specified nationality and gender.");
                }

                return Ok(matchedUsers);
            }
            catch (Exception ex)
            {
              
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        #endregion



        public IActionResult SearchUsers()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchUsersView(string nationality, string gender)
        {
          
            var matchedUsers = _context.UserProfiles
                .Where(u => u.Nationality == nationality && u.Gender == gender)
                .ToList();


            // The reason to serialize object because if used "matchedUsers" directly it doesn't work because TempData don't accept complex object
            TempData["MatchedUsers"] = JsonConvert.SerializeObject(matchedUsers);


            return RedirectToAction("DisplayMatchedUsers");
        }

        
        public IActionResult DisplayMatchedUsers()
        {
           
            var serializedUsers = TempData["MatchedUsers"] as string;

            if (string.IsNullOrEmpty(serializedUsers))
            {
              
                return RedirectToAction("SearchUsers");
            }

           
            var matchedUsers = JsonConvert.DeserializeObject<List<UserProfile>>(serializedUsers);

            return View(matchedUsers);
        }


    }
}