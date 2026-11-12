using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JobApplicationTracker.Models
{
    internal class Recruiter : IEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Email { get; set { if (Regex.IsMatch(value, @"^.+@.+\..+$")) field = value; } } = "";
        public int CompanyId { get; set; }
    }
}
