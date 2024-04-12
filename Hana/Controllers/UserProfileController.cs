using Hana.DB;
using Hana.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using Hana.Hana.Database.Data;
using Microsoft.EntityFrameworkCore;

namespace Hana.Controllers
{
    
    public class UserProfileController : Controller
    {

        private readonly HanaContext _context;

        public UserProfileController(HanaContext context)
        {
            _context = context;
        }

        #region API Endpoints



        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfile>>> GetUserProfiles()
        {
            return await _context.UserProfiles.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfile>> GetUserProfile(int id)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            return userProfile;
        }

        //[HttpPost]
        //public async Task<ActionResult<UserProfile>> PostUserProfile(UserProfile userProfile)
        //{
        //    _context.UserProfiles.Add(userProfile);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetUserProfile), new { id = userProfile.Id }, userProfile);
        //}


          [HttpPut("{id}")]
    public async Task<IActionResult> PutUserProfile(string id, UserProfile userProfile)
    {
        if (id != userProfile.Id)
        {
            return BadRequest();
        }

        _context.Entry(userProfile).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserProfileExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserProfile(int id)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }

            _context.UserProfiles.Remove(userProfile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserProfileExists(string id)
        {
            return _context.UserProfiles.Any(e => e.Id == id);
        }

        #endregion


        // Action method to display a list of user profiles
        public async Task<IActionResult> ShowUsers()
        {
            var userProfiles = await _context.UserProfiles.ToListAsync();
            return View(userProfiles);
        }


        // Action method to display a form for creating a new user profile
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction("ShowUsers", "UserProfile");
            }

            return View(userProfile);
          /*  return View(userProfile);*/ // Return to the Create view with validation errors
        }
        //    private readonly ILogger<UserProfileController> _logger;
        //    private readonly UserDb _userDb;

        //    public UserProfileController(ILogger<UserProfileController> logger, UserDb userDb)
        //    {
        //        _logger = logger;
        //        _userDb = userDb;
        //    }

        //    [HttpGet]
        //    public IActionResult AllUsers()
        //    {
        //        var users = _userDb.GetUser();
        //        return View(users);
        //    }

        //    [HttpGet("createUserProfile")]
        //    public IActionResult CreateUserProfile()
        //    {
        //        return View();
        //    }



        //    [HttpPost("createUserProfile")]
        //    public IActionResult CreateUserProfile([FromForm] UserProfile userProfile)
        //    {
        //        _userDb.CreateUser(userProfile);

        //        return Redirect("/api/UserProfile"); // Redirect to the UserProfile endpoint
        //    }




        //}
    }
}