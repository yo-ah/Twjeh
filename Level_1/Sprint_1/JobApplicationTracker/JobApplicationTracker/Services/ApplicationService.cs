using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationTracker.Services
{
    internal class ApplicationService(CandidateService candidate, JobService job): IService<Application>
    {
        private readonly DataStore<Application> _store = new();
        private readonly CandidateService _candidateService = candidate;
        private readonly JobService _jobService = job;

        public void Add()
        {
            Console.Clear();
            Console.WriteLine("========== Create New Application ==========");

            int jobId = Validator.IntValidation("Job ID: ", 1);
            if(_jobService.GetByID(jobId) is null)
            {
                Helper.PreviousMenu("there is no Job with this ID ");
                return;
            }

            int candidateId = Validator.IntValidation("Candidate ID: ", 1);
            if(_candidateService.GetByID(candidateId) is null)
            {
                Helper.PreviousMenu("there is no Candidate with this ID ");
                return;
            }

            Application app = new() {
                JobId = jobId,
                CandidateId = candidateId,
                AppliedDate = DateTime.Now
            };

            Print(app);
            _store.Add(app);

            Helper.PreviousMenu("Job Added Successfully ");
        }

        public void Update()
        {
            int id = Validator.IntValidation("Application ID: ", 1);

            Application? oldApp = _store.GetByID(id);

            // this loop will continue running until user chose to back to previous menu
            while (oldApp is not null)
            {
                Console.Clear();
                Console.WriteLine("============= Update Application =============");

                Print(oldApp);

                Console.WriteLine("1] Update Job ID\n" +
                                  "2] Update Candidate ID\n" +
                                  "3] Update Status\n" +
                                  "4] Back to previous menu");

                int choice = Validator.IntValidation("enter your choice: ", 1, 4);
                switch (choice)
                {
                    case 1:
                        int newJobId = Validator.IntValidation("Job ID: ", 1);
                        if (_jobService.GetByID(newJobId) is null)
                        {
                            Helper.PreviousMenu("there is no Job with this ID ");
                            break;
                        }
                        oldApp.JobId = newJobId;
                        break;

                    case 2:
                        int newCandidateId = Validator.IntValidation("Candidate ID: ", 1);
                        if (_candidateService.GetByID(newCandidateId) is null)
                        {
                            Helper.PreviousMenu("there is no Candidate with this ID ");
                            break;
                        }
                        oldApp.CandidateId = newCandidateId;
                        break;

                    case 3:
                        Helper.ListEnum<ApplicationStatus>();
                        ChangeStatus(oldApp, (ApplicationStatus)Validator.IntValidation("Status: ", 1, 3) - 1);
                        break;

                    case 4:
                        _store.Update(oldApp);
                        return;
                }
            }
            Helper.PreviousMenu($"This id [{id}] not found ");
        }

        public void Remove()
        {
            Console.Clear();
            Console.WriteLine("=========== Removing A Application View ===========");

            int id = Validator.IntValidation("Application ID: ", 1);

            Application? oldApp = _store.GetByID(id);

            if (oldApp is not null)
            {
                Print(oldApp);
                Console.Write("do you really want to remove this Application? [y]: ");
                string c = Validator.StringValidation("do you really want to remove this Application? [y]: ", ".");
                if (c == "y")
                {
                    _store.Remove(id);
                    Helper.PreviousMenu("Application has been removed successfully ");
                    return;
                }
                Helper.PreviousMenu("Application still exist ");
                return;
            }
            Helper.PreviousMenu($"This id [{id}] not found ");

        }

        public void List()
        {
            Console.Clear();
            Console.WriteLine("============ Listing all Application ============");

            foreach (var app in _store.GetAll())
            {
                Print(app);
            }
            Helper.PreviousMenu("");
        }

        public static void ChangeStatus(Application app, ApplicationStatus newStatus)
        {
            if (!Enum.IsDefined(newStatus))
            {
                Console.WriteLine($"invalid value {newStatus} for {typeof(ApplicationStatus).Name}");
                Console.ReadKey();
            }
            else if (newStatus == 0)
            {
                Console.WriteLine($"can't change to {(ApplicationStatus)0}");
                Console.ReadKey();
            }
            else
            {
                app.Status = newStatus;
            }

        }

        public Application? GetByID(int id) => _store.GetByID(id);

        public  void Print(Application app)
        {
            Console.WriteLine($"{app.Id,-3}{app.JobId,-3}{app.CandidateId,-3}{app.AppliedDate,-23}{app.Status,-7}");
        }

    }
}
