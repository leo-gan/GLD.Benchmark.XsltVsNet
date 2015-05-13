using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NetSerializer;

namespace GLD.Benchmark.XsltVsNet
{
    internal class NetTransformer : ITransformer
    {
        private readonly XmlSerializer _xmlSerializerPerson = new XmlSerializer(typeof(Person));
        private readonly XmlSerializer _xmlSerializerEmployee = new XmlSerializer(typeof(Employee));
        private readonly Serializer _jsonSerializer = new Serializer(new Type[] { typeof(Person), typeof(Employee), typeof(Gender), typeof(Passport), typeof(PoliceRecord), typeof(HistoryRecord) });

        private XmlWriterSettings _xmlWriterSettings = new XmlWriterSettings()
        {
            Indent = false,
            NewLineHandling = NewLineHandling.None
        };

        #region ITransformer Members

        public string Transform(string serialized, bool tryJson)
        {
            Person person = tryJson ? JsonDeserialize<Person>(serialized) : XmlDeserializePerson(serialized);

            Employee employee = MapPersonToEmploee(person);

            return tryJson ? JsonSerialize<Employee>(employee) : XmlSerializeEmployee(employee);
        }

        public string Enrich(string serialized, bool tryJson)
        {
            Person person = tryJson ? JsonDeserialize<Person>(serialized) : XmlDeserializePerson(serialized);

            person = EnrichPerson(person);

            return tryJson ? JsonSerialize<Person>(person) : XmlSerializePerson(person);

        }

        #endregion

        private static Employee MapPersonToEmploee(Person person)
        {
            var employee = new Employee
            {
                Age = person.Age + 1,
                FirstName = person.FirstName,
                Gender = person.Gender,
                LastName = person.LastName,
                Passport = new Passport
                {
                    Authority = person.Passport.Authority,
                    ExpirationDate = person.Passport.ExpirationDate + TimeSpan.FromDays(365),
                    Number = person.Passport.Number,
                },
                HistoryRecords = new HistoryRecord[person.PoliceRecords.Length]
            };
            for (var i = 0; i < person.PoliceRecords.Length; i++)
                employee.HistoryRecords[i] = new HistoryRecord
                {
                    Id = person.PoliceRecords[i].Id + 1,
                    CrimeCode = person.PoliceRecords[i].CrimeCode,
                };
            return employee;
        }

        private static Person EnrichPerson(Person person)
        {
            person.Age = person.Age + 10;
            person.FirstName = person.FirstName;
            person.LastName = person.LastName;

            person.Passport.Authority = person.Passport.Authority;
            person.Passport.ExpirationDate = person.Passport.ExpirationDate +
                                             TimeSpan.FromDays(3650);
            person.Passport.Number = person.Passport.Number;

            foreach (PoliceRecord t in person.PoliceRecords)
            {
                t.Id = t.Id + 10;
                t.CrimeCode = t.CrimeCode;
                t.Description = t.Description;
                ;
            }
            return person;
        }

        public string JsonSerialize<T>(object person)
        {
            using (var ms = new MemoryStream())
            {
                _jsonSerializer.Serialize(ms, (T)person);
                ms.Flush();
                ms.Position = 0;
                 return Convert.ToBase64String(ms.ToArray());
                //var buffer = ms.ToArray();
                //return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            }
        }

        public T JsonDeserialize<T>(string serialized)
        {
            var b = Convert.FromBase64String(serialized);
            //var b = Encoding.UTF8.GetBytes(serialized);
            using (var stream = new MemoryStream(b))
            {
                stream.Seek(0, SeekOrigin.Begin);
                return (T)_jsonSerializer.Deserialize(stream);
            }
        }

        public string XmlSerializePerson(Person myObject)
        {
            var sb = new StringBuilder();
            using (var sw = XmlWriter.Create(sb, _xmlWriterSettings))
            {
                _xmlSerializerPerson.Serialize(sw, myObject);
            }
            return sb.ToString();
       }

        public Person XmlDeserializePerson(string serialized)
        {
            using (var sr = new StringReader(serialized))
            {
                return (Person)_xmlSerializerPerson.Deserialize(sr);
            }
        }
        public string XmlSerializeEmployee(Employee myObject)
        {
            var sb = new StringBuilder();
            using (var sw = XmlWriter.Create(sb, _xmlWriterSettings))
            {
                _xmlSerializerEmployee.Serialize(sw, myObject);
            }
            return sb.ToString();
         }

        public Employee XmlDeserializeEmployee(string serialized)
        {
            using (var sr = new StringReader(serialized))
            {
                return (Employee)_xmlSerializerEmployee.Deserialize(sr);
            }
        }
    }
}