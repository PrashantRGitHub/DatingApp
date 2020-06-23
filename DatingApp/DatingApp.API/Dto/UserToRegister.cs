﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Dto
{
    public class UserToRegister
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(8,MinimumLength =4, ErrorMessage = "Password should be minimum of 4 and maximum of 8 letters in length")]
        public string Password { get; set; }
    }
}
