using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username cannot be null")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password cannot be null")]
        public string Password { get; set; }
    }
}
