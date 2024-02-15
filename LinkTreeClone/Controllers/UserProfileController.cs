//using LinkTreeClone.DB;
//using LinkTreeClone.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System.Collections.Generic;
//using System.Data.Common;
//using System.Data.SqlClient;
//using System.Data;

//namespace LinkTreeClone.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserProfileController : Controller
//    {
//        private readonly ILogger<UserProfileController> _logger;
//        private readonly UserDb _userDb;

//        public UserProfileController(ILogger<UserProfileController> logger, UserDb userDb)
//        {
//            _logger = logger;
//            _userDb = userDb;
//        }

//        [HttpGet]
//        public IActionResult AllUsers()
//        {
//            var users = _userDb.GetUser();
//            return View(users);
//        }

//        [HttpGet("allUsersAPI")]
//        public IActionResult AllUsersAPI()
//        {
//            var users = _userDb.GetUser();
//            return Ok(users);
//        }

//        [HttpGet("createUserProfile")]
//        public IActionResult CreateUserProfile()
//        {
//            return View();
//        }

//        [HttpPost("createUserProfile")]
//        public IActionResult CreateUserProfile([FromForm] UserProfile userProfile)
//        {
//            _userDb.CreateUser(userProfile);

//            return Redirect("/api/UserProfile"); // Redirect to the UserProfile endpoint
//        }






//    }
//}

using Microsoft.AspNetCore.Mvc;
using LinkTreeClone.DB;
using LinkTreeClone.Models;
using System.Collections.Generic;

namespace LinkTreeClone.Controllers
{
    [Route("api/v1/userprofiles")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserDb _userDb;

        public UserProfileController(UserDb userDb)
        {
            _userDb = userDb;
        }

        [HttpGet]
        public IActionResult GetAllUserProfiles()
        {
            var userProfiles = _userDb.GetUser();
            return Ok(userProfiles);
        }

        //[HttpGet("allUsers")]
        //public IActionResult AllUsers()
        //{
        //    var users = _userDb.GetUser();
        //    return View("Index", users); // Render the "Index" view and pass the list of users
        //}


        //[HttpGet("createUserProfile")]
        //public IActionResult CreateUserProfile()
        //{
        //    return View("Create"); // Render the "Create" view for creating user profiles
        //}
        //public IActionResult GetUserProfile(int id)
        //{
        //    var userProfile = _userDb.GetUser(id);
        //    if (userProfile == null)
        //    {
        //        return NotFound(); // Return 404 if the user profile is not found
        //    }
        //    return Ok(userProfile);
        //}

        [HttpPost]
        public IActionResult CreateUserProfile([FromBody] UserProfile userProfile)
        {
            if (userProfile == null)
            {
                return BadRequest(); // Return 400 Bad Request if the request body is invalid
            }

            _userDb.CreateUser(userProfile);

            return Ok();
        }


    }
}

