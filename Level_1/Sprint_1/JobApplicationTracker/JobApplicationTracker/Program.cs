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
        static readonly ApplicationService _applications = new(_candidates, _jobs, _companies);
        static void Main(string[] args)
        {
            LoadSampleData();
            UserInput();
        }

        static void LoadSampleData()
        {
            _companies.Seed([
                    new Company { Name = "COMP_1", City = "Cairo", Industry = "Energy" },
                    new Company { Name = "COMP_2", City = "Cairo", Industry = "Energy", Status = CompanyStatus.Approved },
                    new Company { Name = "COMP_3", City = "Cairo", Industry = "Energy" }
                    ]);

            _recruiters.Seed([
                    new Recruiter { Name = "REQ_1", CompanyId = 1 },
                    new Recruiter { Name = "REQ_2", CompanyId = 3 },
                    new Recruiter { Name = "REQ_3", CompanyId = 3 }
                ]);

            _jobs.Seed([
                    new Job { Title = "JOB_1", CompanyId = 2, RecruiterId = 3, Salary = 1000 },
                    new Job { Title = "JOB_2", CompanyId = 2, RecruiterId = 2, Salary = 1000 },
                    new Job { Title = "JOB_3", CompanyId = 2, RecruiterId = 2, Salary = 1000 },
                    new Job { Title = "JOB_4", CompanyId = 2, RecruiterId = 3, Salary = 1000 },
                    new Job { Title = "JOB_5", CompanyId = 2, RecruiterId = 2, Salary = 1000 },
                    new Job { Title = "JOB_6", CompanyId = 2, RecruiterId = 3, Salary = 1000 }
                ]);

            _candidates.Seed([
                    new Candidate { FullName = "CAND_1", Email = "test1@gmail.com" },
                    new Candidate { FullName = "CAND_2", Email = "test2@gmail.com" },
                    new Candidate { FullName = "CAND_3", Email = "test3@gmail.com" },
                    new Candidate { FullName = "CAND_4", Email = "test4@gmail.com" },
                    new Candidate { FullName = "CAND_5", Email = "test5@gmail.com" }
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

        public static void CandidateMenu() => SubMenu("========== Candidate View ==========", _candidates);

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
