using JobApplicationTracker.Models;
using JobApplicationTracker.Services;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace JobApplicationTracker
{
    internal class Program
    {
        static readonly CompanyService _companies = new();
        static readonly RecruiterService _recruiters = new(_companies);
        static readonly JobService _jobs = new(_companies, _recruiters);
        static readonly CandidateService _candidates = new();
        static readonly ApplicationService _applications = new(_candidates, _jobs);
        static void Main(string[] args)
        {
            LoadSampleData();
            UserInput();
        }

        static void LoadSampleData()
        {
            _companies.Seed([
                    new Company { Name = "COMP_1"},
                    new Company { Name = "COMP_2" },
                    new Company { Name = "COMP_3" }
                    ]);

            _recruiters.Seed([
                    new Recruiter { Id = 1, Name = "REQ_1", CompanyId = 1 },
                    new Recruiter { Id = 2, Name = "REQ_2", CompanyId = 2 },
                    new Recruiter { Id = 3, Name = "REQ_3", CompanyId = 3 }
                ]);

            _jobs.Seed([
                    new Job { Id = 1, Title = "JOB_1", CompanyId = 1, RecruiterId = 3 },
                    new Job { Id = 2, Title = "JOB_2", CompanyId = 2, RecruiterId = 2 },
                    new Job { Id = 3, Title = "JOB_3", CompanyId = 3, RecruiterId = 1 },
                    new Job { Id = 4, Title = "JOB_4", CompanyId = 1, RecruiterId = 3 },
                    new Job { Id = 5, Title = "JOB_5", CompanyId = 2, RecruiterId = 2 },
                    new Job { Id = 6, Title = "JOB_6", CompanyId = 3, RecruiterId = 1 }
                ]);

            _candidates.Seed([
                    new Candidate { Id = 1, FullName = "CAND_1" },
                    new Candidate { Id = 2, FullName = "CAND_2" },
                    new Candidate { Id = 3, FullName = "CAND_3" },
                    new Candidate { Id = 4, FullName = "CAND_4" },
                    new Candidate { Id = 5, FullName = "CAND_5" }
                ]);
        }

        static void UserInput()
        {
            while (true)
            {

                Console.Clear();
                Console.WriteLine("=========== Main Program ===========");
                Console.WriteLine("1] Companies\n" +
                                  "2] Recruiters\n" +
                                  "3] Jobs\n" +
                                  "4] Candidates\n" +
                                  "5] Applications\n" +
                                  "6] Exit");

                int choice = Validator.IntValidation("enter your choice: ", 1, 6);
                switch (choice)
                {
                    case 1: CompanyMenu(); break;
                    case 2: RecruiterMenu(); break;
                    case 3: JobMenu(); break;
                    case 4: CandidateMenu(); break;
                    case 5: ApplicationMenu(); break;
                    case 6: return;
                }
            }
        }

        public static void CompanyMenu() => SubMenu("========== Company View ==========", _companies);

        public static void RecruiterMenu() => SubMenu("========== Recruiter View ==========", _recruiters);

        public static void JobMenu() => SubMenu("========== Job View ==========", _jobs);

        public static void CandidateMenu() => SubMenu("========== Candidate View ==========", _companies);

        public static void ApplicationMenu() => SubMenu("========== Application View ==========", _applications);

        public static void SubMenu<T>(string title, IService<T> service) where T : class, IEntity
        {
            // this loop will continue to run until user wan to back to main menu
            while (true)
            {
                Console.Clear();
                Console.WriteLine(title);
                Console.WriteLine("1] Add\n" +
                                  "2] Update\n" +
                                  "3] List\n" +
                                  "4] Remove\n" +
                                  "5] Back");

                int choice = Validator.IntValidation("enter your choice: ", 1, 5);
                switch (choice)
                {
                    case 1: service.Add(); break;
                    case 2: service.Update(); break;
                    case 3: service.List(); break;
                    case 4: service.Remove(); break;
                    case 5: return;
                }
            }
        }
    }
}
