using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationTracker.Models
{
    internal class Application: IEntity
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int CandidateId { get; set; }
        public DateTime AppliedDate { get; set; }
        public ApplicationStatus Status { get; set; }
    }
    enum ApplicationStatus
    {
        Submitted,
        UnderReview,
        Interview,
        Accepted,
        Rejected,
    }
}
