using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace JobApplicationTracker.Services
{
    internal class JobService(CompanyService company, RecruiterService recruiter): IService<Job>
    {
        private readonly DataStore<Job> _store = new();
        private readonly Dictionary<int, Job> _jobs = [];
        private readonly CompanyService _companyService = company;
        private readonly RecruiterService _recruiterService = recruiter;

        public void Seed(IList<Job> list) 
        { 
            _store.Seed(list);
            foreach (var job in _store.GetAll()) _jobs.Add(job.Id, job);
        }

        public void Add()
        {
            Console.Clear();
            Console.WriteLine("========== Create New Job ==========");

            string title = Validator.StringValidation("Job Title (req): ", @"^[a-zA-Z.][ a-zA-Z0-9.]{2,20}$");

            string description = Validator.StringValidation("Description: ", @"^.{0,30}$");
            
            int salary = Validator.IntValidation("Salary: ", 0);

            string Location = Validator.StringValidation("Location: ", @"^.{0,20}$");

            int companyId = Validator.IntValidation("Company ID: ", 1);
            if(_companyService.GetByID(companyId) is null)
            {
                Helper.PreviousMenu("there is no Company with this ID ");
                return;
            }

            int recruiterId = Validator.IntValidation("Recruiter ID: ", 1);
            if (_recruiterService.GetByID(recruiterId) is null)
            {
                Helper.PreviousMenu("there is no Recruiter with this ID ");
                return;
            }

            Job job = new() {
                Title = title,
                Description = description,
                Salary = salary,
                Location = Location,
                IsOpen = true,
                PostedDate = DateTime.Now,
                CompanyId = companyId,
                RecruiterId = recruiterId
            };

            Print(job);
            _store.Add(job);
            _jobs.Add(job.Id, job);

            Helper.PreviousMenu("Job Added Successfully ");
        }

        public void Update()
        {
            int id = Validator.IntValidation("Job ID: ", 1);

            Job? oldJob = _store.GetByID(id);

            // this loop will continue running until user chose to back to previous menu
            while (oldJob is not null)
            {
                Console.Clear();
                Console.WriteLine("============= Update Job =============");

                Print(oldJob);

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
                        oldJob.Title = Validator.StringValidation("Job Title: ", @"^[a-zA-Z.][a-zA-Z0-9.]{2,9}$");
                        break;

                    case 2:
                        oldJob.Description = Validator.StringValidation("Description: ", @"^.{0,30}$");
                        break;

                    case 3:
                        oldJob.Salary =  Validator.IntValidation("Salary: ", 0);
                        break;

                    case 4:
                        oldJob.Location = Validator.StringValidation("Location: ", @"^.{0,20}$");
                        break;

                    case 5:
                        oldJob.IsOpen = !oldJob.IsOpen;
                        Helper.PreviousMenu($"job is " + (oldJob.IsOpen ? "open " : "closed "));
                        break;

                    case 6:
                        oldJob.PostedDate = DateTime.Now;
                        break;

                    case 7:
                        int newCompanyId = Validator.IntValidation("Company ID: ", 1);
                        if(_companyService.GetByID(newCompanyId) is null)
                        {
                            Helper.PreviousMenu("there is no Company with this ID ");
                            break;
                        }
                        oldJob.CompanyId = newCompanyId;
                        break;

                    case 8:
                        int newRecruiterID = Validator.IntValidation("Recruiter ID: ", 1);
                        if(_recruiterService.GetByID(newRecruiterID) is null)
                        {
                            Helper.PreviousMenu("there is no Recruiter with this ID ");
                            break;
                        }
                        oldJob.RecruiterId = newRecruiterID;
                        break;

                    case 9:
                        _store.Update(oldJob);
                        _jobs[oldJob.Id] = oldJob;
                        return;
                }
            }
            Helper.PreviousMenu($"This id [{id}] not found ");
        }

        public void Remove()
        {
            Console.Clear();
            Console.WriteLine("=========== Removing A Job View ===========");

            int id = Validator.IntValidation("Job ID: ", 1);

            Job? oldJob = _store.GetByID(id);

            if (oldJob is not null)
            {
                Print(oldJob);
                Console.Write("do you really want to remove this job? [y]: ");
                string c = Validator.StringValidation("do you really want to remove this job? [y]: ", ".");
                if (c == "y")
                {
                    _store.Remove(id);
                    _jobs.Remove(id);
                    Helper.PreviousMenu("Job has been removed successfully ");
                    return;
                }
                Helper.PreviousMenu("Job still exist ");
                return;
            }
            Helper.PreviousMenu($"This id [{id}] not found ");

        }

        public void List()
        {
            Console.Clear();
            Console.WriteLine("============ Listing all Jobs ============");

            foreach (var job in _store.GetAll())
            {
                Print(job);
            }
            Helper.PreviousMenu("");
        }

        public Job? GetByID(int id) => _store.GetByID(id);

        public void Print(Job job)
        {
            Console.WriteLine($"{job.Id,-3}{job.Title,-7}{job.Description,-30}{job.Salary, -5}{job.Location, -20}{job.IsOpen, -4}" +
                $"{job.PostedDate.Date, -10}{job.CompanyId, -3}{job.RecruiterId,-3}");
        }
    }
}
