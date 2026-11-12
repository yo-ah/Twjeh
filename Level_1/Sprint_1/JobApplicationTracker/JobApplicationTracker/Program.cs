using JobApplicationTracker.Models;
using JobApplicationTracker.Services;

namespace JobApplicationTracker
{
    internal class Program
    {
        private static readonly CompanyService _companies = new();// _companies = [];
        private static readonly RecruiterService _recruiters = new();
        private static readonly JobService _jobs = new();
        private static readonly CandidateService _candidates = new();
        static void Main(string[] args)
        {
            LoadSampleData();
        }
        static void LoadSampleData()
        {
            _companies.Companies.AddRange(
                    new Company { Id = 1, Name = "COMP_1" },
                    new Company { Id = 2, Name = "COMP_2" },
                    new Company { Id = 3, Name = "COMP_3" }
                );

            _recruiters.Recruiters.AddRange(
                    new Recruiter { Id = 1, Name = "R1" },
                    new Recruiter { Id = 2, Name = "R2" },
                    new Recruiter { Id = 3, Name = "R3" }
                );

            _jobs.Jobs.AddRange(
                    new Job { Id = 1, Title = "J1" },
                    new Job { Id = 2, Title = "J2" },
                    new Job { Id = 3, Title = "J3" },
                    new Job { Id = 4, Title = "J4" },
                    new Job { Id = 5, Title = "J5" },
                    new Job { Id = 6, Title = "J6" }
                );

            _candidates.Candidates.AddRange(
                    new Candidate { Id = 1, FullName = "CAND_1" },
                    new Candidate { Id = 2, FullName = "CAND_2" },
                    new Candidate { Id = 3, FullName = "CAND_3" },
                    new Candidate { Id = 4, FullName = "CAND_4" },
                    new Candidate { Id = 5, FullName = "CAND_5" }
                );
        }
    }
}
