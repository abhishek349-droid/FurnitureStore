using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureStore.Contracts;
using FurnitureStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FurnitureController : ControllerBase
    {
        private readonly IFurnitureRepository _repo;

        public FurnitureController(IFurnitureRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Furniture
        [HttpGet]
        public IActionResult GetAllFurnitures()
        {
            try
            {
                return Ok(_repo.GetFurnitures());
            }
            catch (Exception e)
            {
                return NotFound(new Response { StatusCode = "Failure", Message = e.ToString() });
            }
        }

        // GET: api/Furniture/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult GetFurniture(int id)
        {
            try
            {
                return Ok(_repo.GetFurniture(id));
            }
            catch (Exception e)
            {
                return NotFound(new Response { StatusCode = "Failure", Message = e.ToString() });
            }
        }

        // POST: api/Furniture
        [HttpPost]
        public IActionResult PostFurniture([FromBody] FurnitureModel furniture)
        {
            try
            {
                bool isAdmin = IsAdmin(HttpContext.User);
                if (isAdmin)
                {
                    var res = _repo.AddFurniture(furniture);
                    if (res) return Ok(new Response { StatusCode = "Success", Message = "Furniture saved" });
                    else return NotFound(new Response { StatusCode = "Failure", Message = "Problem occured while storing furniture. Check your request" }); 
                }
                else return Unauthorized(new Response { StatusCode = "unauthorised", Message = "Operation allowed only for admins" });
            }
            catch (Exception e)
            {
                return NotFound(new Response { StatusCode = "Failure", Message = e.ToString() });
            }
        }

        // PUT: api/Furniture/5
        [HttpPut("{id}")]
        public IActionResult PutFurniture(int id, [FromBody] FurnitureModel furniture)
        {
            try
            {
                bool isAdmin = IsAdmin(HttpContext.User);
                if (isAdmin)
                {
                    var res = _repo.UpdateFurniture(id, furniture);
                    if (res) return Ok(new Response { StatusCode = "Success", Message = "Furniture updated" });
                    else return NotFound(new Response { StatusCode = "Failure", Message = "No furniture with the provided ID eists. Please check" }); 
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
        public IActionResult DeleteFurniture(int id)
        {
            try
            {
                bool isAdmin = IsAdmin(HttpContext.User);
                if (isAdmin)
                {
                    var res = _repo.DeleteFurniture(id);
                    if (res) return Ok(new Response { StatusCode = "Success", Message = "Furniture deleted" });
                    else return NotFound(new Response { StatusCode = "Failure", Message = "Furniture couldn't be deleted. Please try again." }); 
                }
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
    }
}
