using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;

namespace JobApplicationTracker.Services
{
    internal class CompanyService : IService<Company>
    {
        private readonly DataStore<Company> _store = new();
        private readonly Stack<(int, CompanyStatus)> _undo = [];

        public void Seed(IList<Company> list) => _store.Seed(list);

        public void Add()
        {
            Console.Clear();
            Console.WriteLine("========== Create New Company ==========");

            string name = Validator.StringValidation("Company Name (req): ", @"^[a-zA-Z.][ a-zA-Z0-9.]{2,20}$");

            string industry = Validator.StringValidation("Industry: ", @"^([a-zA-Z.][ a-zA-Z0-9.]{0,14})?$");

            string city = Validator.StringValidation("City: ", @"^([a-zA-Z.][a-zA-Z0-9.]{0,14})?$");

            Company company = new() { Name = name, Industry = industry, City = city };
            Print(company);
            _store.Add(company);

            Helper.PreviousMenu("Company Added Successfully ");
        }

        public void Update()
        {
            int id = Validator.IntValidation("Company ID: ", 1);

            Company? oldCompany = _store.GetByID(id);

            // this loop will continue running until user chose to back to previous menu
            while (oldCompany is not null)
            {
                Console.Clear();
                Console.WriteLine("============= Update Company =============");

                Print(oldCompany);

                Console.WriteLine("1] Update Name\n" +
                                  "2] Update Industry\n" +
                                  "3] Update City\n" +
                                  "4] Update Status\n" +
                                  "5] Undo Option\n" +
                                  "6] Back to previous menu");

                int choice = Validator.IntValidation("enter your choice: ", 1, 6);
                switch (choice)
                {
                    case 1:
                        oldCompany.Name = Validator.StringValidation("Company Name: ", @"^[a-zA-Z.][a-zA-Z0-9.]{2,9}$");
                        break;

                    case 2:
                        oldCompany.Industry = Validator.StringValidation("Industry: ", @"^[a-zA-Z.][a-zA-Z0-9.]{2,9}$");
                        break;

                    case 3:
                        oldCompany.City = Validator.StringValidation("City: ", @"^[a-zA-Z.][a-zA-Z0-9.]{2,9}$");
                        break;

                    case 4:
                        Helper.ListEnum<CompanyStatus>();
                        var newStatus = (CompanyStatus)Validator.IntValidation("Status: ", 1, 3) - 1;
                        ChangeStatus(oldCompany, newStatus);
                        break;

                    case 5:
                        UndoCompanyStatus();
                        break;

                    case 6:
                        _store.Update(oldCompany);
                        return;
                }
            }
            Helper.PreviousMenu($"This id [{id}] not found ");
        }

        public void UndoCompanyStatus()
        {
            if (_undo.Count == 0)
            {
                Helper.PreviousMenu("there is no thing to undo ");
                return;
            }

            var oldStatus = _undo.Pop();
            var company = GetByID(oldStatus.Item1);

            if (company is null)
            {
                Helper.PreviousMenu($"this company (ID: {oldStatus.Item1}) have been deleted\n");
                return;
            }

            company.Status = oldStatus.Item2;
            Helper.PreviousMenu($"restored {company.Name} to {oldStatus} ");
        }

        public void Remove()
        {
            Console.Clear();
            Console.WriteLine("=========== Removing A Company View ===========");

            int id = Validator.IntValidation("Company ID: ", 1);

            Company? oldCompany = _store.GetByID(id);

            if (oldCompany is not null)
            {
                Print(oldCompany);
                if (Validator.StringValidation("do you really want to remove this company? [y]: ", ".") == "y")
                {
                    _store.Remove(id);
                    Helper.PreviousMenu("Company has been removed successfully ");
                    return;
                }
                Helper.PreviousMenu("Company still exist ");
                return;
            }
            Helper.PreviousMenu($"This id [{id}] not found ");
        }

        public void List()
        {
            Helper.ListEnum<CompanyStatus>();
            ListByStatus((CompanyStatus)Validator.IntValidation("Company Status: ", 1, 3) - 1);
            Helper.PreviousMenu("");
        }

        public void ListByStatus(CompanyStatus status)
        {
            Console.Clear();
            Console.WriteLine($"============ Listing {status} Companies ============");

            if (Enum.IsDefined(status))
            {
                foreach (var company in _store.GetAll())
                {
                    if (company.Status == status) Print(company);
                }
            }
            else
            {
                Console.WriteLine($"invalid value {status} for {typeof(CompanyStatus).Name}");
            }
        }

        public Company? GetByID(int id) => _store.GetByID(id);

        public void ChangeStatus(Company company, CompanyStatus newStatus)
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
                _undo.Push((company.Id, company.Status));
                company.Status = newStatus;
            }

        }

        public void Print(Company company)
        {
            Console.WriteLine($"{company.Id,-3}{company.Name,-10}{company.Industry,-15}{company.City,-10}{company.Status,-5}");
        }
    }
}
