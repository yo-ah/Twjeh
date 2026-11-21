using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;

namespace JobApplicationTracker.Services
{
    internal class CompanyService
    {
        public  readonly List<Company> Companies = [];

        public  void  AddCompany()
        {
            Console.Clear();
            Console.WriteLine("========== Create New Company ==========");

            string name = Helper.UpdateEntityData("1] Company Name (req): ", () => Validator.StringValidation("Company Name (req): ", @"^[a-zA-Z.][a-zA-Z0-9.]{2,9}$"));

            string industry = Helper.UpdateEntityData("2] Industry: ", () => Validator.StringValidation("Industry: ", @"^([a-zA-Z.][a-zA-Z0-9.]{0,14})?$"));

            string city = Helper.UpdateEntityData("3] City: ", () => Validator.StringValidation("City: ", @"^([a-zA-Z.][a-zA-Z0-9.]{0,14})?$"));

            CompanyStatus status = Helper.UpdateEntityData("4] Company Status:- \n",
                () =>
                {
                    Helper.ListEnum<CompanyStatus>();
                    return (CompanyStatus)Validator.IntValidation("Status: ", 1, 3) - 1;
                });

            Company company = new() { Id = Companies[^1].Id + 1, Name = name, Industry = industry, City = city, Status = status };

            Companies.Add(company);
            PrintCompany(company);

            Helper.PreviousMenu("Company Added Successfully ", Program.CompanyMenu);
        }

        public  void  UpdateCompany()
        {
            int companyId = Validator.IntValidation("Company ID: ", 1);

            Company? oldCompany = null;
            foreach (var company in Companies)
            {
                if (company.Id == companyId)
                {
                    oldCompany = company;
                    break;
                }
            }
            // this loop will continue running until user chose to back to previous menu
            while (oldCompany is not null)
            {
                Console.Clear();
                Console.WriteLine("============= Update Company =============");

                PrintCompany(oldCompany);

                Console.WriteLine("1] Update Name\n" +
                                  "2] Update Industry\n" +
                                  "3] Update City\n" +
                                  "4] Update Status\n" +
                                  "5] Back to previous menu");

                int choice = Validator.IntValidation("enter your choice: ", 1, 5);
                switch (choice)
                {
                    case 1:
                        oldCompany.Name = Helper.UpdateEntityData("New Company Name: ",
                            () => Validator.StringValidation("Company Name: ", @"^[a-zA-Z.][a-zA-Z0-9.]{2,9}$"));
                        break;

                    case 2:
                        oldCompany.Industry = Helper.UpdateEntityData("New Industry: ",
                            () => Validator.StringValidation("Industry: ", @"^[a-zA-Z.][a-zA-Z0-9.]{2,9}$"));
                        break;

                    case 3:
                        oldCompany.City = Helper.UpdateEntityData("New City: ",
                            () => Validator.StringValidation("City: ", @"^[a-zA-Z.][a-zA-Z0-9.]{2,9}$"));
                        break;

                    case 4:
                        ChangeCompanyStatus(oldCompany, Helper.UpdateEntityData("New Status:- ", () =>
                        {
                            Helper.ListEnum<CompanyStatus>();
                            return (CompanyStatus)Validator.IntValidation("Status: ", 1, 3) - 1;
                        }));
                        break;

                    case 5: Program.CompanyMenu(); break;
                }
            }
            Helper.PreviousMenu($"This id [{companyId}] not found ", Program.CompanyMenu);
        }

        public  void  RemoveCompany()
        {
            Console.Clear();
            Console.WriteLine("=========== Removing A Company View ===========");

            int companyId = Validator.IntValidation("Company ID: ", 1);

            Company? oldCompany = null;
            foreach (var company in Companies)
            {
                if (company.Id == companyId)
                {
                    oldCompany = company;
                    break;
                }
            }

            if (oldCompany is not null)
            {
                PrintCompany(oldCompany);
                Console.Write("do you really want to remove this company? [y]: ");
                string c = Validator.StringValidation("do you really want to remove this company? [y]: ", ".");
                if(c == "y")
                {
                    Companies.Remove(oldCompany);
                    Helper.PreviousMenu("Company has been removed successfully ", Program.CompanyMenu);
                }
                Helper.PreviousMenu("Company still exist ", Program.CompanyMenu);
            }
            Console.WriteLine("this Id not found");
            Helper.PreviousMenu("", Program.CompanyMenu);

        }

        public  void  ListCompany()
        {
            Console.Clear();
            Console.WriteLine("============ Listing all Companies ============");

            foreach (var company in Companies)
            {
                PrintCompany(company);
            }
            Helper.PreviousMenu("", Program.CompanyMenu);
        }

        public  void ListCompanyByStatus(CompanyStatus status)
        {
            if (Enum.IsDefined(status))
            {
                foreach (var company in Companies)
                {
                    if (company.Status == status) PrintCompany(company);
                }
            }
            else
            {
                Console.WriteLine($"invalid value {status} for {typeof(CompanyStatus).Name}");
            }
        }

        public static void ChangeCompanyStatus(Company company, CompanyStatus newStatus)
        {
            if (!Enum.IsDefined(newStatus))
            {
                Console.WriteLine($"invalid value {newStatus} for {typeof(CompanyStatus).Name}");
                Console.ReadKey();
            }
            else if (newStatus == 0)
            {
                Console.WriteLine($"can't change to {(CompanyStatus)0}");
                Console.ReadKey();
            }
            else
            {
                company.Status = newStatus;
            }

        }

        public static void PrintCompany(Company company)
        {
            Console.WriteLine($"{company.Id,-3}{company.Name,-10}{company.Industry,-15}{company.City,-10}{company.Status,-5}");
        }

        public Company? GetCompanyById(int id)
        {
            foreach (var company in Companies)
            {
                if (company.Id == id) return company;
            }
            return default;
        }
    }
}
