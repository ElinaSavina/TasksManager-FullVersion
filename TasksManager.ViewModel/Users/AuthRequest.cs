﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TasksManager.ViewModel.Users
{
    public class AuthRequest
    {
        [Required]
        [MaxLength(64)]
        public string Login { get; set; }

        [Required]
        [MaxLength(128)]
        public string Password { get; set; }
    }
}
