using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FurnitureStore.Contracts;
using FurnitureStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FurnitureStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController] 
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UserController(IUserRepository repo)
        {
            _repo = repo;
        }

        // GET: api/User/AllUser
        [HttpGet]
        [Route("AllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                if(IsAdmin(HttpContext.User)) return Ok(_repo.GetUsers());
                else return Unauthorized(new Response { StatusCode = "unauthorised", Message = "Operation allowed only for admins" });
            }
            catch (Exception e)
            {
                return NotFound(new Response { StatusCode = "Failure", Message = e.ToString() });
            }
        }

        private bool IsAdmin(System.Security.Claims.ClaimsPrincipal currentUser)
        {
            if (currentUser.HasClaim(c => c.Type == "Role"))
            {
                var role = currentUser.Claims.FirstOrDefault(c => c.Type == "Role").Value;
                if (role == "Admin") return true;
                return false;
            }
            return false;
        }

        private bool IsRightfulUser(System.Security.Claims.ClaimsPrincipal currentUser, string username)
        {
            if (currentUser.HasClaim(c => c.Type == ClaimTypes.Name))
            {
                var usernameInToken = currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                if (username == usernameInToken) return true;
                return false;
            }
            return false;
        }

        // GET: api/User/Users/5
        [HttpGet]
        [Route("Users/{id}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                var user = _repo.GetUser(id);
                var isadmin = IsAdmin(HttpContext.User);

                if (isadmin) return Ok(user);
                                
                if (IsRightfulUser(HttpContext.User, user.Username)) return Ok(user);
                else return Unauthorized(new Response { StatusCode = "Failure", Message = "You can only view your record" });
            }
            catch (Exception e)
            {
                return NotFound(new Response { StatusCode = "Failure", Message = e.ToString() });
            }
        }

        // POST: api/User
        [AllowAnonymous]
        [HttpPost]
        public IActionResult RegisterUser([FromBody] UsersModel user)
        {
            try
            {
                var res = _repo.RegisterUser(user);
                if (res) return Ok(new Response { StatusCode = "Success", Message = "user saved" });
                else return NotFound(new Response { StatusCode = "Failure", Message = "Problem occured while storing user. Check your request" });
            }
            catch (Exception e)
            {
                return NotFound(new Response { StatusCode = "Failure", Message = e.ToString() });
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UsersModel user)
        {
            try
            {
                bool isAdmin = IsAdmin(HttpContext.User);
                if (isAdmin)
                {
                    var res = _repo.UpdateUser(id, user);
                    if (res) return Ok(new Response { StatusCode = "Success", Message = "user updated" });
                    else return NotFound(new Response { StatusCode = "Failure", Message = "No user with the provided ID eists. Please check" });
                }
                else return Unauthorized(new Response { StatusCode = "unauthorised", Message = "Operation allowed only for admins" });
            }
            catch (Exception e)
            {
                return NotFound(new Response { StatusCode = "Failure", Message = e.ToString() });
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                bool isAdmin = IsAdmin(HttpContext.User);
                if (isAdmin)
                {
                    var res = _repo.DeleteUser(id);
                    if (res) return Ok(new Response { StatusCode = "Success", Message = "user deleted" });
                    else return NotFound(new Response { StatusCode = "Failure", Message = "User couldn't be deleted. Please try again." }); 
                }
                else return Unauthorized(new Response { StatusCode = "unauthorised", Message = "Operation allowed only for admins" });
            }
            catch (Exception e)
            {
                return NotFound(new Response { StatusCode = "Failure", Message = e.ToString() });
            }
        }
    }
}
