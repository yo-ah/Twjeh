using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationTracker.Models
{
    internal class Job: IEntity
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; } = "";
        public decimal Salary { get; set { if (value > 0) field = value; } }
        public string Location { get; set; } = "";
        public bool IsOpen { get; set; } = true;
        public DateTime PostedDate { get; set; }
        public int CompanyId { get; set; }
        public int RecruiterId { get; set; }
    }
}
