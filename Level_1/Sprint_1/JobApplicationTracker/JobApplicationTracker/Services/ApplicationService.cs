using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationTracker.Services
{
    internal class ApplicationService(CandidateService candidate, JobService job, CompanyService company): IService<Application>
    {
        private readonly DataStore<Application> _store = new();
        private readonly CandidateService _candidateService = candidate;
        private readonly JobService _jobService = job;
        private readonly CompanyService _companyService = company;
        // different approach
        //private readonly Dictionary<int, List<int>> _applications = [];
        // (jobId, candidateId)
        private readonly HashSet<(int, int)> _applications = [];


        public void Add()
        {
            Console.Clear();
            Console.WriteLine("========== Create New Application ==========");

            var job = _jobService.GetByID(Validator.IntValidation("Job ID: ", 1));

            if (job is null)
            {
                Helper.PreviousMenu("there is no Job with this ID ");
                return;
            }
            else if (!job.IsOpen)
            {
                Helper.PreviousMenu($"this job {job.Title} is closed ");
                return;
            }

            if(_companyService.GetByID(job.CompanyId)?.Status != CompanyStatus.Approved)
            {
                Helper.PreviousMenu("can't applay for un approaved company: ");
                return;
            }

            var candidate = _candidateService.GetByID(Validator.IntValidation("Candidate ID: ", 1));
            if (candidate is null)
            {
                Helper.PreviousMenu("there is no Candidate with this ID ");
                return;
            }

            if(IsApplied(job.Id, candidate.Id))
            {
                Helper.PreviousMenu($"candidate: {candidate.FullName} [{candidate.Id}]" +
                    $"already applied for this job {job.Title} [{job.Id}]");
                return;
            }

            Application app = new() {
                JobId = job.Id,
                CandidateId = candidate.Id,
                AppliedDate = DateTime.Now
            };

            Print(app);
            _store.Add(app);
            _applications.Add((job.Id, candidate.Id));

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
                        var job = _jobService.GetByID(newJobId);
                        if (job is null)
                        {
                            Helper.PreviousMenu("there is no Job with this ID ");
                            break;
                        }
                        if (_companyService.GetByID(job.CompanyId)?.Status != CompanyStatus.Approved)
                        {
                            Helper.PreviousMenu("can't applay for un approaved company: ");
                            break;
                        }
                        if (IsApplied(newJobId, oldApp.CandidateId))
                        {
                            Helper.PreviousMenu($"the current candidate already applied for this job ");
                            break;
                        }
                        UpdateApplication((oldApp.JobId, oldApp.CandidateId), (newJobId, oldApp.CandidateId));
                        oldApp.JobId = newJobId;
                        break;

                    case 2:
                        int newCandidateId = Validator.IntValidation("Candidate ID: ", 1);
                        if (_candidateService.GetByID(newCandidateId) is null)
                        {
                            Helper.PreviousMenu("there is no Candidate with this ID ");
                            break;
                        }
                        if (IsApplied(oldApp.JobId, newCandidateId))
                        {
                            Helper.PreviousMenu($"this candidate already applied for current job ");
                            break;
                        }
                        UpdateApplication((oldApp.JobId, oldApp.CandidateId), (oldApp.JobId, newCandidateId));
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
                    _applications.Remove((oldApp.JobId, oldApp.CandidateId));
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
            else if (newStatus <= app.Status)
            {
                Console.WriteLine($"can't change to {newStatus}");
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

        private bool IsApplied(int jobId, int candidateId) => _applications.Contains((jobId, candidateId));

        private void UpdateApplication((int a, int b) old, (int c, int d) current)
        {
            _applications.Remove(old);
            _applications.Add(current);
        }

        // this work on dictionary<jobId, List<candidateId>>
        //private bool IsApplied(int jobId, int candidateId)
        //{
        //    if (_applications.ContainsKey(jobId)) return false;

        //    foreach(int item in _applications[jobId])
        //    {
        //        if (item == candidateId) return true;
        //    }

        //    return false;
        //}
    }
}
