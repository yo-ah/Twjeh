using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationTracker.Models
{
    internal class Company : IEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Industry { get; set; } = "";
        public string City { get; set; } = "";
        public CompanyStatus Status { get; set; }
    }
    public enum CompanyStatus
    {
        Pending,
        Approved,
        Rejected,
    }
}
