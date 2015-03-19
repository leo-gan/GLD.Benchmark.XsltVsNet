using System;
using System.Collections.Generic;

namespace GLD.Benchmark.XsltVsNet
{
    

   
    [Serializable]
    public class HistoryRecord
    {
        public int Id { get; set; }
        public string CrimeCode { get; set; }
    }

    [Serializable]
    public class Employee
    {
        // private static int maxHistoryRecordCounter = 20;

        public Employee()
        {
            FirstName = Randomizer.Name;
            LastName = Randomizer.Name;
            Age = (uint) Randomizer.Rand.Next(120);
            Gender = (Randomizer.Rand.Next(0, 1) == 0) ? Gender.Male : Gender.Female;
            Passport = new Passport
            {
                Authority = Randomizer.Phrase,
                ExpirationDate =
                    Randomizer.GetDate(DateTime.UtcNow, DateTime.UtcNow + TimeSpan.FromDays(1000)),
                Number = Randomizer.Id
            };
            var curHistoryRecordCounter = Randomizer.Rand.Next(20);
            HistoryRecords = new HistoryRecord[curHistoryRecordCounter];
            for (var i = 0; i < curHistoryRecordCounter; i++)
                HistoryRecords[i] = new HistoryRecord
                {
                    Id = int.Parse(Randomizer.Id),
                    CrimeCode = Randomizer.Name
                };
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public uint Age { get; set; }

        public Gender Gender { get; set; }

        public Passport Passport { get; set; }

        // OverwriteList happens to be very important in this case, where constructor generates this array!
        // IsRequired is important!
        public HistoryRecord[] HistoryRecords { get; set; }

        public List<string> Compare(Person comparable)
        {
            var errors = new List<string> {"  ************** Comparison failed! "};
            if (comparable == null)
            {
                errors.Add("comparable: is null!");
                return errors;
            }

            Compare("FirstName", FirstName, comparable.FirstName, errors);
            Compare("LastName", LastName, comparable.LastName, errors);
            Compare("Age", Age, comparable.Age, errors);
            Compare("Gender", Gender, comparable.Gender, errors);
            Compare("Passport.Authority", Passport.Authority, comparable.Passport.Authority, errors);
            Compare("Passport.ExpirationDate", Passport.ExpirationDate,
                comparable.Passport.ExpirationDate, errors);
            Compare("Passport.Number", Passport.Number, comparable.Passport.Number, errors);
            Compare("FirstName", FirstName, comparable.FirstName, errors);
            Compare("FirstName", FirstName, comparable.FirstName, errors);
            Compare("FirstName", FirstName, comparable.FirstName, errors);
            Compare("FirstName", FirstName, comparable.FirstName, errors);

            var originalHistoryRecords = HistoryRecords;
            var comparableHistoryRecords = comparable.HistoryRecords;
            Compare("HistoryRecords.Length", originalHistoryRecords.Length,
                comparableHistoryRecords.Length, errors);

            var minLength = Math.Min(originalHistoryRecords.Length, comparableHistoryRecords.Length);
            for (var i = 0; i < minLength; i++)
            {
                Compare("HistoryRecords[" + i + "].Id", originalHistoryRecords[i].Id,
                    comparableHistoryRecords[i].Id, errors);
                Compare("HistoryRecords[" + i + "].CrimeCode", originalHistoryRecords[i].CrimeCode,
                    comparableHistoryRecords[i].CrimeCode, errors);
            }
            return errors;
        }

        private static void Compare(string objectName, object left, object right,
                                    List<string> errors)
        {
            if (!left.Equals(right))
                errors.Add(String.Format("\t{0}: {1} != {2}", objectName, left, right));
        }
    }
}