using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Text;

namespace JobApplicationTracker.Services
{
    internal class RecruiterService(CompanyService service)
    {
        private readonly DataStore<Recruiter> _store = new();
        private readonly CompanyService _companyService = service;

        public void Seed(IList<Recruiter> list) => _store.Seed(list);

        public void AddRecruiter()
        {
            Console.Clear();
            Console.WriteLine("========== Create New Recruiter ==========");

            string name = Helper.UpdateEntityData("1] Recruiter Name (req): ", () => Validator.StringValidation("Recruiter Name (req): ", @"^[a-zA-Z.][a-zA-Z0-9.]{2,9}$"));

            string email = Helper.UpdateEntityData("2] Email: ", () => Validator.StringValidation("Email: ", Validator.EmailPattern));

            int companyId = Helper.UpdateEntityData("3] Company ID: ", () => Validator.IntValidation("Company ID: ", 1));

            CheckCompanyID(companyId);

            Recruiter recruiter = new() { Name = name, Email = email, CompanyId = companyId };

            PrintRecruiter(recruiter);
            _store.Add(recruiter);

            Helper.PreviousMenu("Recruiter Added Successfully ", Program.RecruiterMenu);
        }

        public void UpdateRecruiter()
        {
            int id = Validator.IntValidation("Recruiter ID: ", 1);

            Recruiter? oldRecruiter = _store.GetByID(id);

            // this loop will continue running until user chose to back to previous menu
            while (oldRecruiter is not null)
            {
                Console.Clear();
                Console.WriteLine("============= Update Recruiter =============");

                PrintRecruiter(oldRecruiter);

                Console.WriteLine("1] Update Name\n" +
                                  "2] Update Email\n" +
                                  "3] Update Company ID\n" +
                                  "4] Back to previous menu");

                int choice = Validator.IntValidation("enter your choice: ", 1, 4);
                switch (choice)
                {
                    case 1:
                        oldRecruiter.Name = Helper.UpdateEntityData("New Recruiter Name: ",
                            () => Validator.StringValidation("Recruiter Name: ", @"^[a-zA-Z.][a-zA-Z0-9.]{2,9}$"));
                        break;

                    case 2:
                        oldRecruiter.Email = Helper.UpdateEntityData("New Email: ",
                            () => Validator.StringValidation("Email: ", Validator.EmailPattern));
                        break;

                    case 3:
                        oldRecruiter.CompanyId = Helper.UpdateEntityData("New Company ID: ",
                            () => 
                            {
                                int id = Validator.IntValidation("Company ID: ", 1);
                                CheckCompanyID(id);
                                return id;
                            });
                        break;

                    case 4:
                        _store.Update(oldRecruiter);
                        Program.RecruiterMenu();
                        break;
                }
            }
            Helper.PreviousMenu($"This id [{id}] not found ", Program.RecruiterMenu);
        }

        public void RemoveRecruiter()
        {
            Console.Clear();
            Console.WriteLine("=========== Removing A Recruiter View ===========");

            int id = Validator.IntValidation("Recruiter ID: ", 1);

            Recruiter? oldRecruiter = _store.GetByID(id);
            if (oldRecruiter is not null)
            {
                PrintRecruiter(oldRecruiter);
                Console.Write("do you really want to remove this recruiter? [y]: ");
                string c = Validator.StringValidation("do you really want to remove this recruiter? [y]: ", ".");
                if (c == "y")
                {
                    _store.Remove(id);
                    Helper.PreviousMenu("Recruiter has been removed successfully ", Program.RecruiterMenu);
                }
                Helper.PreviousMenu("Recruiter still exist ", Program.RecruiterMenu);
            }
            Console.WriteLine("this Id not found");
            Helper.PreviousMenu("", Program.RecruiterMenu);

        }

        public void ListRecruiter()
        {
            Console.Clear();
            Console.WriteLine("============ Listing all _store ============");

            foreach (var recruiter in _store.GetAll())
            {
                PrintRecruiter(recruiter);
            }
            Helper.PreviousMenu("", Program.RecruiterMenu);
        }

        public Recruiter? GetByID(int id) => _store.GetByID(id);

        public static void PrintRecruiter(Recruiter recruiter)
        {
            Console.WriteLine($"{recruiter.Id,-3}{recruiter.Name,-10}{recruiter.Email,-15}");
        }

        private void CheckCompanyID(int id)
        {
            if (_companyService.GetByID(id) is null)
            {
                Console.WriteLine("there is no Company with this ID");
                Helper.PreviousMenu("", Program.RecruiterMenu);
            }
        }
    }
}
