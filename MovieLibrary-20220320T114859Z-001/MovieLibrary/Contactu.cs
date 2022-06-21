﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary
{
    public partial class Contactu
    {
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = " Name is required.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Invalid First Name.")]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = " Name is required.")]
        [RegularExpression("\\d{10}", ErrorMessage = "Pls check the number again")]
        public string Phoneno { get; set; }
        [Required(ErrorMessage = "subject  should be required")]
        public string Subjects { get; set; }
        [Required(ErrorMessage = "Give message")]
        public string Message { get; set; }
    }
}