using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace JobApplicationTracker.Services
{
    internal class RecruiterService
    {
        public readonly List<Recruiter> Recruiters = [];

        public void AddRecruiter()
        {
            Console.Clear();
            Console.WriteLine("========== Create New Recruiter ==========");

            string name = Helper.UpdateEntityData("1] Recruiter Name (req): ", () => Validator.StringValidation("Recruiter Name (req): ", @"^[a-zA-Z.][a-zA-Z0-9.]{2,9}$"));

            string email = Helper.UpdateEntityData("2] Email: ", () => Validator.StringValidation("Email: ", Validator.EmailPattern));

            int companyId = Helper.UpdateEntityData("3] Company ID: ", () => Validator.IntValidation("Company ID: ", 1));

            CheckCompanyID(companyId);

            Recruiter recruiter = new() { Id = Recruiters[^1].Id + 1, Name = name, Email = email, CompanyId = companyId };

            PrintRecruiter(recruiter);
            Recruiters.Add(recruiter);

            Helper.PreviousMenu("Recruiter Added Successfully ", Program.RecruiterMenu);
        }

        public void UpdateRecruiter()
        {
            int recruiterId = Validator.IntValidation("Recruiter ID: ", 1);

            Recruiter? oldRecruiter = null;
            foreach (var recruiter in Recruiters)
            {
                if (recruiter.Id == recruiterId)
                {
                    oldRecruiter = recruiter;
                    break;
                }
            }
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

                    case 4: Program.RecruiterMenu(); break;
                }
            }
            Helper.PreviousMenu($"This id [{recruiterId}] not found ", Program.RecruiterMenu);
        }

        public void RemoveRecruiter()
        {
            Console.Clear();
            Console.WriteLine("=========== Removing A Recruiter View ===========");

            int recruiterId = Validator.IntValidation("Recruiter ID: ", 1);

            Recruiter? oldRecruiter = null;
            foreach (var recruiter in Recruiters)
            {
                if (recruiter.Id == recruiterId)
                {
                    oldRecruiter = recruiter;
                    break;
                }
            }

            if (oldRecruiter is not null)
            {
                PrintRecruiter(oldRecruiter);
                Console.Write("do you really want to remove this recruiter? [y]: ");
                string c = Validator.StringValidation("do you really want to remove this recruiter? [y]: ", ".");
                if (c == "y")
                {
                    Recruiters.Remove(oldRecruiter);
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
            Console.WriteLine("============ Listing all Recruiters ============");

            foreach (var recruiter in Recruiters)
            {
                PrintRecruiter(recruiter);
            }
            Helper.PreviousMenu("", Program.RecruiterMenu);
        }

        public static void PrintRecruiter(Recruiter recruiter)
        {
            Console.WriteLine($"{recruiter.Id,-3}{recruiter.Name,-10}{recruiter.Email,-15}");
        }

        private static void CheckCompanyID(int id)
        {
            if (Program._companies.GetCompanyById(id) is null)
            {
                Console.WriteLine("there is no Company with this ID");
                Helper.PreviousMenu("", Program.RecruiterMenu);
            }
        }
    }
}
