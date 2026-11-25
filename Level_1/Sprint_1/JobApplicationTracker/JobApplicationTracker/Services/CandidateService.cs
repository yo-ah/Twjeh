using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationTracker.Services
{
    internal class CandidateService: IService<Candidate>
    {
        private readonly DataStore<Candidate> _store = new();
        private readonly HashSet<string> _emails = [];

        public void Seed(IList<Candidate> list) => _store.Seed(list);

        public void Add()
        {
            Console.Clear();
            Console.WriteLine("========== Create New Candidate ==========");

            string fullName = Validator.StringValidation("Candidate Full Name (req): ", @"^[a-zA-Z.][ a-zA-Z0-9.]{2,20}$");

            string email = Validator.StringValidation("Email: ", Validator.EmailPattern);
            if (!IsUnique(email))
            {
                Helper.PreviousMenu("this email already exist ");
                return;
            }

            string phone = Validator.StringValidation("Phone no: ", @"^(?:(?:\+20|0020|0)1\d{9})?$");
            
            int years = Validator.IntValidation("Years of Experience: ", 0, 40);

            Candidate candidate = new() { FullName = fullName, Email = email, Phone = phone, YearsOfExperience = years };

            Print(candidate);
            _store.Add(candidate);

            Helper.PreviousMenu("Candidate Added Successfully ");
        }

        public void Update()
        {
            int id = Validator.IntValidation("Candidate ID: ", 1);

            Candidate? oldCandidate = _store.GetByID(id);

            // this loop will continue running until user chose to back to previous menu
            while (oldCandidate is not null)
            {
                Console.Clear();
                Console.WriteLine("============= Update Candidate =============");

                Print(oldCandidate);

                Console.WriteLine("1] Update Name\n" +
                                  "2] Update Email\n" +
                                  "3] Update Phone no\n" +
                                  "4] Update Years Of Experience\n" +
                                  "5] Back to previous menu");

                int choice = Validator.IntValidation("enter your choice: ", 1, 5);
                switch (choice)
                {
                    case 1:
                        oldCandidate.FullName = Validator.StringValidation("Candidate Full Name: ",
                                                                           @"^[a-zA-Z.][ a-zA-Z0-9.]{2,20}$");
                        break;

                    case 2:
                        string email = Validator.StringValidation("Email: ", Validator.EmailPattern);
                        if(email == oldCandidate.Email)
                        {
                            Helper.PreviousMenu("you can't enter same email ");
                            return;
                        }
                        if (!IsUnique(email)) break;
                        oldCandidate.Email = email;
                        break;

                    case 3:
                        oldCandidate.Phone = Validator.StringValidation("Phone no: ", @"^(?:(?:\+20|0020|0)1\d{9})?$");
                        break;

                    case 4:
                        oldCandidate.YearsOfExperience = Validator.IntValidation("Years Of Experience: ", 0, 40);
                        break;

                    case 5:
                        _store.Update(oldCandidate);
                        return;
                }
            }
            Helper.PreviousMenu($"This id [{id}] not found ");
        }

        public void Remove()
        {
            Console.Clear();
            Console.WriteLine("=========== Removing A Candidate View ===========");

            int id = Validator.IntValidation("Candidate ID: ", 1);

            Candidate? oldCandidate = _store.GetByID(id);

            if (oldCandidate is not null)
            {
                Print(oldCandidate);
                Console.Write("do you really want to remove this candidate? [y]: ");
                string c = Validator.StringValidation("do you really want to remove this candidate? [y]: ", ".");
                if (c == "y")
                {
                    _store.Remove(id);
                    Helper.PreviousMenu("Candidate has been removed successfully ");
                    return;
                }
                Helper.PreviousMenu("Candidate still exist ");
                return;
            }
            Helper.PreviousMenu($"This id [{id}] not found ");

        }

        public void List()
        {
            Console.Clear();
            Console.WriteLine("============ Listing all Candidates ============");

            foreach (var candidate in _store.GetAll())
            {
                Print(candidate);
            }
            Helper.PreviousMenu("");
        }

        public Candidate? GetByID(int id) => _store.GetByID(id);

        public void Print(Candidate candidate)
        {
            Console.WriteLine($"{candidate.Id,-3}{candidate.FullName,-10}{candidate.Email,-15}{candidate.Phone, -13}{candidate.YearsOfExperience,-2}");
        }

        private bool IsUnique(string email)
        {
            if (email == string.Empty) return true;

            return _emails.Contains(email);
        }
    }
}
