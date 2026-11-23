using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationTracker.Services
{
    internal class CandidateService
    {
        private readonly DataStore<Candidate> _store = new();

        public void Seed(IList<Candidate> list) => _store.Seed(list);

        public void AddCandidate()
        {
            Console.Clear();
            Console.WriteLine("========== Create New Candidate ==========");

            string fullName = Helper.UpdateEntityData("1] Candidate Full Name (req): ", () => Validator.StringValidation("Candidate Name (req): ", @"^[a-zA-Z.][ a-zA-Z0-9.]{2,20}$"));

            string email = Helper.UpdateEntityData("2] Email: ", () => Validator.StringValidation("Email: ", Validator.EmailPattern));

            CheckUniquenessOfEmail(email);

            string phone = Helper.UpdateEntityData("3] Phone no: ", () => Validator.StringValidation("Phone no: ", @"^(?:(?:\+20|0020|0)1\d{9})?$"));
            
            int years = Helper.UpdateEntityData("4] Years of Experience: ", () => Validator.IntValidation("Years of Experience: ", 0, 40));

            Candidate candidate = new() { FullName = fullName, Email = email, Phone = phone, YearsOfExperience = years };

            PrintCandidate(candidate);
            _store.Add(candidate);

            Helper.PreviousMenu("Candidate Added Successfully ", Program.CandidateMenu);
        }

        public void UpdateCandidate()
        {
            int id = Validator.IntValidation("Candidate ID: ", 1);

            Candidate? oldCandidate = _store.GetByID(id);

            // this loop will continue running until user chose to back to previous menu
            while (oldCandidate is not null)
            {
                Console.Clear();
                Console.WriteLine("============= Update Candidate =============");

                PrintCandidate(oldCandidate);

                Console.WriteLine("1] Update Name\n" +
                                  "2] Update Email\n" +
                                  "3] Update Phone no\n" +
                                  "4] Update Years Of Experience\n" +
                                  "5] Back to previous menu");

                int choice = Validator.IntValidation("enter your choice: ", 1, 5);
                switch (choice)
                {
                    case 1:
                        oldCandidate.FullName = Helper.UpdateEntityData("New Candidate Full Name: ",
                            () => Validator.StringValidation("Candidate Full Name: ", @"^[^\s].{2,19}$"));
                        break;

                    case 2:
                        oldCandidate.Email = Helper.UpdateEntityData("New Email: ", () => 
                            {
                                string email = Validator.StringValidation("Email: ", Validator.EmailPattern);
                                CheckUniquenessOfEmail(email);
                                return email;
                            });
                        break;

                    case 3:
                        oldCandidate.Phone = Helper.UpdateEntityData("New Phone no: ", () => Validator.StringValidation("Phone no: ", @"^(?:(?:\+20|0020|0)1\d{9})?$"));
                        break;

                    case 4:
                        oldCandidate.YearsOfExperience = Helper.UpdateEntityData("New Years Of Experience: ", () => Validator.IntValidation("Years Of Experience: ", 0));
                        break;

                    case 5:
                        _store.Update(oldCandidate);
                        Program.CandidateMenu();
                        break;
                }
            }
            Helper.PreviousMenu($"This id [{id}] not found ", Program.CandidateMenu);
        }

        public void RemoveCandidate()
        {
            Console.Clear();
            Console.WriteLine("=========== Removing A Candidate View ===========");

            int id = Validator.IntValidation("Candidate ID: ", 1);

            Candidate? oldCandidate = _store.GetByID(id);

            if (oldCandidate is not null)
            {
                PrintCandidate(oldCandidate);
                Console.Write("do you really want to remove this candidate? [y]: ");
                string c = Validator.StringValidation("do you really want to remove this candidate? [y]: ", ".");
                if (c == "y")
                {
                    _store.Remove(id);
                    Helper.PreviousMenu("Candidate has been removed successfully ", Program.CandidateMenu);
                }
                Helper.PreviousMenu("Candidate still exist ", Program.CandidateMenu);
            }
            Console.WriteLine("this Id not found");
            Helper.PreviousMenu("", Program.CandidateMenu);

        }

        public void ListCandidate()
        {
            Console.Clear();
            Console.WriteLine("============ Listing all Candidates ============");

            foreach (var candidate in _store.GetAll())
            {
                PrintCandidate(candidate);
            }
            Helper.PreviousMenu("", Program.CandidateMenu);
        }

        public Candidate? GetByID(int id) => _store.GetByID(id);

        public static void PrintCandidate(Candidate candidate)
        {
            Console.WriteLine($"{candidate.Id,-3}{candidate.FullName,-10}{candidate.Email,-15}{candidate.Phone, -13}{candidate.YearsOfExperience,-2}");
        }

        private void CheckUniquenessOfEmail(string email)
        {
            if (email == string.Empty) return;

            foreach(var candidate in _store.GetAll())
            {
                if(candidate.Email == email)
                {
                    Console.WriteLine("this email already exist");
                    Helper.PreviousMenu("", Program.CandidateMenu);
                }
            }
        }
    }
}
