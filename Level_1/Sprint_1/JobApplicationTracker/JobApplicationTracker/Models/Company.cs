using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationTracker.Models
{
    internal record Company (string Name, string Industry = "", string City = "", CompanyStatus Status = CompanyStatus.Pending)
    {
        private static int _idCounter = 0;
        public int Id { get; } = ++_idCounter;
        public string Name { get; set; } = Name;
        public string Industry { get; set; } = Industry;
        public string City { get; set; } = City;
        public CompanyStatus Status { get; set; } = Status;
    }
    enum CompanyStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
