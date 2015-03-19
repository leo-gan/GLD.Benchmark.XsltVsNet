using System;
using System.IO;
using System.Xml.Serialization;

namespace GLD.Benchmark.XsltVsNet
{
    internal class NetTransformer : ITransformer
    {
        private readonly XmlSerializer _personSerializer = new XmlSerializer(typeof(Person));
        private readonly XmlSerializer _employeeSerializer = new XmlSerializer(typeof(Employee));

        #region ITransformer Members

        public string Transform(string sourceXml)
        {
            Person person;
            using (var sr = new StringReader(sourceXml))
            {
                person = (Person) _personSerializer.Deserialize(sr);
            }

            Employee employee = MapPersonToEmploee(person);

            using (var sw = new StringWriter())
            {
                _employeeSerializer.Serialize(sw, employee);
                return sw.ToString();
            }
        }

        public string Enrich(string sourceXml)
        {
            Person person;
            using (var sr = new StringReader(sourceXml))
            {
                person = (Person) _personSerializer.Deserialize(sr);
            }

            person = EnrichPerson(person);

            using (var sw = new StringWriter())
            {
                _personSerializer.Serialize(sw, person);
                return sw.ToString();
            }
        }

        #endregion

        private static Employee MapPersonToEmploee(Person person)
        {
            var employee = new Employee
            {
                Age = person.Age + 1,
                FirstName = "T " + person.FirstName,
                Gender = person.Gender,
                LastName = "T " + person.LastName,
                Passport = new Passport
                {
                    Authority = "T " + person.Passport.Authority,
                    ExpirationDate = person.Passport.ExpirationDate + TimeSpan.FromDays(365),
                    Number = "T " + person.Passport.Number,
                },
                HistoryRecords = new HistoryRecord[person.PoliceRecords.Length]
            };
            for (var i = 0; i < person.PoliceRecords.Length; i++)
                employee.HistoryRecords[i] = new HistoryRecord
                {
                    Id = person.PoliceRecords[i].Id + 1,
                    CrimeCode = "T " + person.PoliceRecords[i].CrimeCode,
                };
            return employee;
        }

        private static Person EnrichPerson(Person person)
        {
            person.Age = person.Age + 10;
            person.FirstName = "E " + person.FirstName;
            person.LastName = "E " + person.LastName;

            person.Passport.Authority = "E " + person.Passport.Authority;
            person.Passport.ExpirationDate = person.Passport.ExpirationDate +
                                             TimeSpan.FromDays(3650);
            person.Passport.Number = "E " + person.Passport.Number;

            foreach (PoliceRecord t in person.PoliceRecords)
            {
                t.Id = t.Id + 10;
                t.CrimeCode = "E " + t.CrimeCode;
                t.Description = "E " + t.Description;
                ;
            }
            return person;
        }
    }
}