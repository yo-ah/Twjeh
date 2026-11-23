using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace JobApplicationTracker.Services
{
    internal class JobService(CompanyService company, RecruiterService recruiter)
    {
        private readonly DataStore<Job> _store = new();
        private readonly Dictionary<int, Job> _jobs = [];
        private readonly CompanyService _companyService = company;
        private readonly RecruiterService _recruiterService = recruiter;

        public void Seed(IList<Job> list) => _store.Seed(list);

        public void AddJob()
        {
            Console.Clear();
            Console.WriteLine("========== Create New Job ==========");

            string title = Helper.UpdateEntityData("1] Job Title (req): ", () => Validator.StringValidation("Job Title (req): ", @"^[a-zA-Z.][a-zA-Z0-9.]{2,9}$"));

            string description = Helper.UpdateEntityData("2] Description: ", () => Validator.StringValidation("Description: ", @"^.{0,30}$"));
            
            int salary = Helper.UpdateEntityData("3] Salary: ", () => Validator.IntValidation("Salary: ", 0));

            string Location = Helper.UpdateEntityData("4] Location: ", () => Validator.StringValidation("Location: ", @"^.{0,20}$"));

            int companyId = Helper.UpdateEntityData("5] Company ID: ", () => Validator.IntValidation("Company ID: ", 1));
            IsExist(() => _companyService.GetByID(companyId), () => Helper.PreviousMenu("there is no Company with this ID", Program.JobMenu));
            
            int recruiterId = Helper.UpdateEntityData("6] Recruiter ID: ", () => Validator.IntValidation("Recruiter ID: ", 1));
            IsExist(() => _recruiterService.GetByID(recruiterId), () => Helper.PreviousMenu("there is no Recruiter with this ID", Program.JobMenu));

            Job job = new()
            {
                Title = title,
                Description = description,
                Salary = salary,
                Location = Location,
                IsOpen = true,
                PostedDate = DateTime.Now,
                CompanyId = companyId,
                RecruiterId = recruiterId
            };

            PrintJob(job);
            _store.Add(job);
            _jobs.Add((_store.NextID() - 1), job);

            Helper.PreviousMenu("Job Added Successfully ", Program.JobMenu);
        }

        public void UpdateJob()
        {
            int id = Validator.IntValidation("Job ID: ", 1);

            Job? oldJob = _store.GetByID(id);

            // this loop will continue running until user chose to back to previous menu
            while (oldJob is not null)
            {
                Console.Clear();
                Console.WriteLine("============= Update Job =============");

                PrintJob(oldJob);

                Console.WriteLine("1] Update Title\n" +
                                  "2] Update Description\n" +
                                  "3] Update Salary\n" +
                                  "4] Update Location\n" +
                                  "5] Update Status\n" +
                                  "6] Update Post Date\n" +
                                  "7] Update Company ID\n" +
                                  "8] Update Recruiter ID\n" +
                                  "9] Back to previous menu");

                int choice = Validator.IntValidation("enter your choice: ", 1, 9);
                switch (choice)
                {
                    case 1:
                        oldJob.Title = Helper.UpdateEntityData("New Job Title: ",
                            () => Validator.StringValidation("Job Title: ", @"^[a-zA-Z.][a-zA-Z0-9.]{2,9}$"));
                        break;

                    case 2:
                        oldJob.Description = Helper.UpdateEntityData("New Description: ",
                            () => Validator.StringValidation("Description: ", @"^.{0,30}$"));
                        break;

                    case 3:
                        oldJob.Salary = Helper.UpdateEntityData("New Salary: ", 
                            () => Validator.IntValidation("Salary: ", 0));
                        break;

                    case 4:
                        oldJob.Location = Helper.UpdateEntityData("New Location: ", 
                            () => Validator.StringValidation("Location: ", @"^.{0,20}$"));
                        break;

                    case 5:
                        oldJob.IsOpen = !oldJob.IsOpen;
                        break;

                    case 6:
                        oldJob.PostedDate = DateTime.Now;
                        break;

                    case 7:
                        oldJob.CompanyId = Helper.UpdateEntityData("New Company ID: ", () =>
                        {
                            int id = Validator.IntValidation("Company ID: ", 1);
                            IsExist(() => _recruiterService.GetByID(id), () => Helper.PreviousMenu("there is no Company with this ID", Program.JobMenu));
                            return id;
                        });
                        break;

                    case 8:
                        oldJob.RecruiterId = Helper.UpdateEntityData("New Recruiter ID: ", () =>
                             {
                                 int id = Validator.IntValidation("Recruiter ID: ", 1);
                                 IsExist(() => _recruiterService.GetByID(id), () => Helper.PreviousMenu("there is no Recruiter with this ID", Program.JobMenu));
                                 return id;
                             });
                        break;

                    case 9:
                        _store.Update(oldJob);
                        _jobs[oldJob.Id] = oldJob;
                        Program.JobMenu();
                        break;
                }
            }
            Helper.PreviousMenu($"This id [{id}] not found ", Program.JobMenu);
        }

        public void RemoveJob()
        {
            Console.Clear();
            Console.WriteLine("=========== Removing A Job View ===========");

            int id = Validator.IntValidation("Job ID: ", 1);

            Job? oldJob = null;
            foreach (var job in _store.GetAll())
            {
                if (job.Id == id)
                {
                    oldJob = job;
                    break;
                }
            }

            if (oldJob is not null)
            {
                PrintJob(oldJob);
                Console.Write("do you really want to remove this job? [y]: ");
                string c = Validator.StringValidation("do you really want to remove this job? [y]: ", ".");
                if (c == "y")
                {
                    _store.Remove(id);
                    _jobs.Remove(id);
                    Helper.PreviousMenu("Job has been removed successfully ", Program.JobMenu);
                }
                Helper.PreviousMenu("Job still exist ", Program.JobMenu);
            }
            Console.WriteLine("this Id not found");
            Helper.PreviousMenu("", Program.JobMenu);

        }

        public void ListJob()
        {
            Console.Clear();
            Console.WriteLine("============ Listing all _store ============");

            foreach (var job in _store.GetAll())
            {
                PrintJob(job);
            }
            Helper.PreviousMenu("", Program.JobMenu);
        }

        public void FindJobByID()
        {
            if(_jobs.TryGetValue(Validator.IntValidation("Job ID: ", 1), out var job))
            {
                PrintJob(job);
                Helper.PreviousMenu("", Program.JobMenu);
            }
            Console.WriteLine("this Id not found");
            Helper.PreviousMenu("", Program.JobMenu);
        }

        public static void PrintJob(Job job)
        {
            Console.WriteLine($"{job.Id,-3}{job.Title,-7}{job.Description,-30}{job.Salary, -5}{job.Location, -20}{job.IsOpen, -4}" +
                $"{job.PostedDate.Date, -10}{job.CompanyId, -3}{job.RecruiterId,-3}");
        }

        private static void IsExist(Func<object?> func, Action action)
        {
            if (func() is not null) return;
            action();
        }
    }
}
