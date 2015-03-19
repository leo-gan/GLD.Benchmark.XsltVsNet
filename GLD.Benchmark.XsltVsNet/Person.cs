using System;
using System.Collections.Generic;
using System.IO;

namespace GLD.Benchmark.XsltVsNet
{
    [Serializable]
    public enum Gender
    {
        Male,
        Female,
    }

    [Serializable]
    public class Passport
    {
        public string Number { get; set; }

        public string Authority { get; set; }

        public DateTime ExpirationDate { get; set; }
    }

    [Serializable]
    public class PoliceRecord
    {
        public int Id { get; set; }
        public string CrimeCode { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    public class Person
    {
       public Person() :this(10) {}

        public Person(int numberOfPoliceRecords)
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
            PoliceRecords = new PoliceRecord[numberOfPoliceRecords];
            for (var i = 0; i < numberOfPoliceRecords; i++)
                PoliceRecords[i] = new PoliceRecord
                {
                    Id = int.Parse(Randomizer.Id),
                    CrimeCode = Randomizer.Name,
                    Description = Randomizer.Phrase,
                };
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public uint Age { get; set; }

        public Gender Gender { get; set; }

        public Passport Passport { get; set; }

        public PoliceRecord[] PoliceRecords { get; set; }

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

            var originalPoliceRecords = PoliceRecords;
            var comparablePoliceRecords = comparable.PoliceRecords;
            Compare("PoliceRecords.Length", originalPoliceRecords.Length,
                comparablePoliceRecords.Length, errors);

            var minLength = Math.Min(originalPoliceRecords.Length, comparablePoliceRecords.Length);
            for (var i = 0; i < minLength; i++)
            {
                Compare("PoliceRecords[" + i + "].Id", originalPoliceRecords[i].Id,
                    comparablePoliceRecords[i].Id, errors);
                Compare("PoliceRecords[" + i + "].CrimeCode", originalPoliceRecords[i].CrimeCode,
                    comparablePoliceRecords[i].CrimeCode, errors);
            }
            return errors;
        }

        private static void Compare(string objectName, object left, object right,
                                    List<string> errors)
        {
            if (!left.Equals(right))
                errors.Add(String.Format("\t{0}: {1} != {2}", objectName, left, right));
        }

        public string GetXmlString()
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Person));
            using (var sw = new StringWriter())
            {
                serializer.Serialize(sw, (Person)this);
                return sw.ToString();
            }
        }
    }
}