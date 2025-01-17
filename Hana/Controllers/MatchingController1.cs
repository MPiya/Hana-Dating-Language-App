//using Hana.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using Hana.Hana.Database.Data;

//namespace Hana.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class MatchingController : ControllerBase
//    {
//        private readonly HanaContext _context;

//        public MatchingController(HanaContext context)
//        {
//            _context = context;
//        }

//        [HttpGet("search")]
//        public async Task<ActionResult<IEnumerable<UserProfile>>> SearchUsersApi(string nationality, string gender)
//        {
//            if (string.IsNullOrEmpty(nationality) && string.IsNullOrEmpty(gender))
//            {
//                return BadRequest("Please provide at least one search criteria");
//            }

//            var matchedUsers = await _context.UserProfiles
//                .Where(u => (string.IsNullOrEmpty(nationality) || u.Nationality == nationality) &&
//                           (string.IsNullOrEmpty(gender) || u.Gender == gender))
//                .ToListAsync();

//            if (!matchedUsers.Any())
//            {
//                return NotFound("No users found matching the criteria");
//            }

//            return Ok(matchedUsers);
//        }

//        [HttpGet("display")]
//        public IActionResult DisplayMatchedUsers(string nationality, string gender)
//        {
//            if (string.IsNullOrEmpty(nationality) && string.IsNullOrEmpty(gender))
//            {
//                return BadRequest("Please provide at least one search criteria");
//            }

//            var matchedUsers = _context.UserProfiles
//                .Where(u => (string.IsNullOrEmpty(nationality) || u.Nationality == nationality) &&
//                           (string.IsNullOrEmpty(gender) || u.Gender == gender))
//                .ToList();

//            if (!matchedUsers.Any())
//            {
//                ViewBag.Message = "No users found matching the criteria";
//                return View("DisplayMatchedUsers", new List<UserProfile>());
//            }

//            // Serialize the matched users to TempData to persist across redirects
//            TempData["MatchedUsers"] = JsonConvert.SerializeObject(matchedUsers);

//            return View("DisplayMatchedUsers", matchedUsers);
//        }

//        [HttpGet("search-form")]
//        public IActionResult SearchForm()
//        {
//            return View();
//        }

//        [HttpPost("search-form")]
//        public IActionResult ProcessSearchForm(string nationality, string gender)
//        {
//            return RedirectToAction("DisplayMatchedUsers", new { nationality, gender });
//        }

//        private List<UserProfile> GetMatchedUsersFromTempData()
//        {
//            var serializedUsers = TempData["MatchedUsers"] as string;
//            if (string.IsNullOrEmpty(serializedUsers))
//            {
//                return new List<UserProfile>();
//            }

//            var matchedUsers = JsonConvert.DeserializeObject<List<UserProfile>>(serializedUsers);
//            return matchedUsers ?? new List<UserProfile>();
//        }
//    }
//}