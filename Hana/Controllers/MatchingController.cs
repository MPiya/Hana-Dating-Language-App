using Microsoft.AspNetCore.Mvc;
using Hana.Models;
using System.Linq;
using Hana.Hana.Database.Data;
using Microsoft.EntityFrameworkCore;

namespace Hana.Controllers
{
    public class MatchingController : Controller
    {
        private readonly HanaContext _context;

        public MatchingController(HanaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult SearchUsers()
        {
            // Fetch some random users as default results
            var defaultUsers = _context.UserProfiles.ToList();

            return View("SearchUsers", defaultUsers);
        }

        [HttpGet]
        public IActionResult SearchUsersView(SearchCriteria criteria)
        {
            var matchedUsers = _context.UserProfiles.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Nationality))
            {
                matchedUsers = matchedUsers.Where(u => u.Nationality.Contains(criteria.Nationality));
            }

            if (criteria.AgeFrom > 0 && criteria.AgeTo > 0)
            {
                matchedUsers = matchedUsers.Where(u => u.Age >= criteria.AgeFrom && u.Age <= criteria.AgeTo);
            }

            if (criteria.Gender != "Both")
            {
                matchedUsers = matchedUsers.Where(u => u.Gender == criteria.Gender);
            }

            return View("DisplayMatchedUsers", matchedUsers.ToList());
        }
    }
}