using JobApplicationTracker.Models;
using JobApplicationTracker.Services;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace JobApplicationTracker
{
    internal class Program
    {
        public static readonly CompanyService _companies = new();
        public static readonly RecruiterService _recruiters = new();
        public static readonly JobService _jobs = new();
        public static readonly CandidateService _candidates = new();
        public static readonly ApplicationService _applications = new();
        static void Main(string[] args)
        {
            LoadSampleData();
            UserInput(); 
        }

        static void LoadSampleData()
        {
            _companies.Companies.AddRange(
                    new Company { Id = 1, Name = "COMP_1"},
                    new Company { Id = 2, Name = "COMP_2" },
                    new Company { Id = 3, Name = "COMP_3" }
                );

            _recruiters.Recruiters.AddRange(
                    new Recruiter { Id = 1, Name = "REQ_1", CompanyId = 1 },
                    new Recruiter { Id = 2, Name = "REQ_2", CompanyId = 2 },
                    new Recruiter { Id = 3, Name = "REQ_3", CompanyId = 3 }
                );

            _jobs.Jobs.AddRange(
                    new Job { Id = 1, Title = "JOB_1", CompanyId = 1, RecruiterId = 3 },
                    new Job { Id = 2, Title = "JOB_2", CompanyId = 2, RecruiterId = 2 },
                    new Job { Id = 3, Title = "JOB_3", CompanyId = 3, RecruiterId = 1 },
                    new Job { Id = 4, Title = "JOB_4", CompanyId = 1, RecruiterId = 3 },
                    new Job { Id = 5, Title = "JOB_5", CompanyId = 2, RecruiterId = 2 },
                    new Job { Id = 6, Title = "JOB_6", CompanyId = 3, RecruiterId = 1 }
                );

            _candidates.Candidates.AddRange(
                    new Candidate { Id = 1, FullName = "CAND_1" },
                    new Candidate { Id = 2, FullName = "CAND_2" },
                    new Candidate { Id = 3, FullName = "CAND_3" },
                    new Candidate { Id = 4, FullName = "CAND_4" },
                    new Candidate { Id = 5, FullName = "CAND_5" }
                );
        }

        static void UserInput()
        {
            Console.Clear();
            Console.WriteLine("=========== Main Program ===========");
            Console.WriteLine(@"1] Companies
2] Recruiters
3] Jobs
4] Candidates
5] Exit");

            int choice = Validator.IntValidation("enter your choice: ", 1, 5);
            switch (choice)
            {
                case 1: CompanyMenu(); break;
                case 2: RecruiterMenu(); break;
                case 3: JobMenu(); break;
                case 4: CandidateMenu(); break;
                case 5: Exit(); break;
            }
        }

        public static void CompanyMenu()
        {
            SubMenu("========== Company View ==========");

            int choice = Validator.IntValidation("enter your choice: ", 1, 5);
            switch (choice)
            {
                case 1: _companies.AddCompany(); break;
                case 2: _companies.UpdateCompany(); break;
                case 3: _companies.ListCompany() ; break;
                case 4: _companies.RemoveCompany(); break;
                case 5: UserInput(); break;
            }

        }

        public static void RecruiterMenu()
        {
            SubMenu("========== Recruiter View ==========");

            int choice = Validator.IntValidation("enter your choice: ", 1, 5);
            switch (choice)
            {
                case 1: _recruiters.AddRecruiter(); break;
                case 2: _recruiters.UpdateRecruiter(); break;
                case 3: _recruiters.ListRecruiter(); break;
                case 4: _recruiters.RemoveRecruiter(); break;
                case 5: UserInput(); break;
            }
        }

        public static void JobMenu()
        {
            SubMenu("========== Job View ==========");

            int choice = Validator.IntValidation("enter your choice: ", 1, 5);
            switch (choice)
            {
                case 1: _jobs.AddJob(); break;
                case 2: _jobs.UpdateJob(); break;
                case 3: _jobs.ListJob(); break;
                case 4: _jobs.RemoveJob(); break;
                case 5: UserInput(); break;
            }
        }

        public static void CandidateMenu()
        {
            SubMenu("========== Candidate View ==========");

            int choice = Validator.IntValidation("enter your choice: ", 1, 5);
            switch (choice)
            {
                case 1: _candidates.AddCandidate(); break;
                case 2: _candidates.UpdateCandidate(); break;
                case 3: _candidates.ListCandidate(); break;
                case 4: _candidates.RemoveCandidate(); break;
                case 5: UserInput(); break;
            }
        }

        public static void SubMenu(string title)
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine(@"1] Add
2] Update
3] List
4] Remove
5] Back");
        }

        // prevent any recursive calls from the main menu to any other menu
        static void Exit()
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
