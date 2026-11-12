using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationTracker.Models
{
    internal class Candidate: IEntity
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public string Email { get; set; } =  "";
        public string Phone { get; set; } = "";
        public int YearsOfExperience { get; set { if (value >= 0) field = value; } }
    }
}
