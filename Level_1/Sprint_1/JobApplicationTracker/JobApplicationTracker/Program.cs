using JobApplicationTracker.Models;
using JobApplicationTracker.Services;

namespace JobApplicationTracker
{
    internal class Program
    {
        private static readonly CompanyService _companies = new();
        private static readonly RecruiterService _recruiters = new();
        private static readonly JobService _jobs = new();
        private static readonly CandidateService _candidates = new();
        static void Main(string[] args)
        {
            LoadSampleData();
            UserInput();
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
        static void UserInput()
        {
            Console.Clear();
            Console.WriteLine(@"1] Companies
2] Recruiters
3] Jobs
4] Candidates
5] Exit");

            int choice = IntValidation("enter your choice: ", 1, 5);
            switch(choice)
            {
                case 1: CompanyMenu(); break;
                case 2: RecruiterMenu(); break;
                case 3: JobMenu(); break;
                case 4: CandidateMenu(); break;
                case 5: break;
            }
        }
        // Convert Any Type To Tout Type
        private delegate bool ConvertTo<Tout>(out Tout a);
        static Tout TypeConverter<Tout>(string msg, ConvertTo<Tout> converter)
        {
            Tout output;
            Console.Write(msg);

            while (!converter(out output))
            {
                Console.WriteLine($"invalid {typeof(Tout).Name} input");
                Console.Write(msg);
            }
            return output;
        }
        // validate any int input & validates the range also
        static int IntValidation(string msg, int startRange, int endRange)
        {
            int number = TypeConverter(msg, (out int n) => int.TryParse(Console.ReadLine(), out n));
            
            while(number <  startRange || number > endRange)
            {
                Console.WriteLine($"input should be between [{startRange}-{endRange}]");
                number = TypeConverter(msg, (out int n) =>  int.TryParse(Console.ReadLine(), out n));
            }
            
            return number;
        }
        static void CompanyMenu()
        {
            SubMenu("========== Company View ==========");

            int choice = IntValidation("enter your choice: ", 1,  5);

        }
        static void RecruiterMenu()
        {
            SubMenu("========== Recruiter View ==========");
        }
        static void JobMenu()
        {
            SubMenu("========== Job View ==========");
        }
        static void CandidateMenu()
        {
            SubMenu("========== Candidate View ==========");
        }
        static void SubMenu(string title)
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine(@"1] Add
2] Update
3] List
4] Remove
5] Back");

            int choice = IntValidation("enter your choice: ", 1, 5);
            switch(choice)
            {
                case 1: break;
                case 2: break;
                case 3: break;
                case 4: break;
                case 5: UserInput();  break;
            }
        }
    }
}
