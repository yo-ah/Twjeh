using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Text;

namespace JobApplicationTracker.Services
{
    internal class RecruiterService(CompanyService service): IService<Recruiter>
    {
        private readonly DataStore<Recruiter> _store = new();
        private readonly CompanyService _companyService = service;

        public void Seed(IList<Recruiter> list) => _store.Seed(list);

        public void Add()
        {
            Console.Clear();
            Console.WriteLine("========== Create New Recruiter ==========");

            string name = Validator.StringValidation("Recruiter Name (req): ", @"^[a-zA-Z.][ a-zA-Z0-9.]{2,20}$");

            string email = Validator.StringValidation("Email: ", Validator.EmailPattern);

            int companyId = Validator.IntValidation("Company ID: ", 1);
            if (_companyService.GetByID(companyId) is null)
            {
                Helper.PreviousMenu("there is no Company with this ID ");
                return;
            }

            Recruiter recruiter = new() { Name = name, Email = email, CompanyId = companyId };

            Print(recruiter);
            _store.Add(recruiter);

            Helper.PreviousMenu("Recruiter Added Successfully ");
        }

        public void Update()
        {
            int id = Validator.IntValidation("Recruiter ID: ", 1);

            Recruiter? oldRecruiter = _store.GetByID(id);

            // this loop will continue running until user chose to back to previous menu
            while (oldRecruiter is not null)
            {
                Console.Clear();
                Console.WriteLine("============= Update Recruiter =============");

                Print(oldRecruiter);

                Console.WriteLine("1] Update Name\n" +
                                  "2] Update Email\n" +
                                  "3] Update Company ID\n" +
                                  "4] Back to previous menu");

                int choice = Validator.IntValidation("enter your choice: ", 1, 4);
                switch (choice)
                {
                    case 1:
                        oldRecruiter.Name = Validator.StringValidation("New Recruiter Name: ", @"^[a-zA-Z.][a-zA-Z0-9.]{2,9}$");
                        break;

                    case 2:
                        oldRecruiter.Email = Validator.StringValidation("New Email: ", Validator.EmailPattern);
                        break;

                    case 3:
                        int newCompanyId = Validator.IntValidation("New Company ID: ", 1);
                        if(_companyService.GetByID(newCompanyId) is null)
                        {
                            Helper.PreviousMenu("there is no Company with this ID ");
                            break;
                        }

                        oldRecruiter.CompanyId = newCompanyId;
                        break;

                    case 4:
                        _store.Update(oldRecruiter);
                        return;
                }
            }
            Helper.PreviousMenu($"This id [{id}] not found ");
        }

        public void Remove()
        {
            Console.Clear();
            Console.WriteLine("=========== Removing A Recruiter View ===========");

            int id = Validator.IntValidation("Recruiter ID: ", 1);

            Recruiter? oldRecruiter = _store.GetByID(id);
            if (oldRecruiter is not null)
            {
                Print(oldRecruiter);
                Console.Write("do you really want to remove this recruiter? [y]: ");
                string c = Validator.StringValidation("do you really want to remove this recruiter? [y]: ", ".");
                if (c == "y")
                {
                    _store.Remove(id);
                    Helper.PreviousMenu("Recruiter has been removed successfully ");
                    return;
                }
                Helper.PreviousMenu("Recruiter still exist ");
                return;
            }
            Helper.PreviousMenu("this Id not found ");
        }

        public void List()
        {
            Console.Clear();
            Console.WriteLine("============ Listing all _store ============");

            foreach (var recruiter in _store.GetAll())
            {
                Print(recruiter);
            }
            Helper.PreviousMenu("");
        }

        public Recruiter? GetByID(int id) => _store.GetByID(id);

        public void Print(Recruiter recruiter)
        {
            Console.WriteLine($"{recruiter.Id,-3}{recruiter.Name,-10}{recruiter.Email,-15}");
        }
    }
}
