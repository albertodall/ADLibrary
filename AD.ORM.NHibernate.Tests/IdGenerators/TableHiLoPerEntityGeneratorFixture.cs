using System;
using System.Collections.Generic;
using NUnit.Framework;
using SharpTestsEx;

namespace AD.ORM.NHibernate.Tests.IdGenerators
{
    [TestFixture]
    class TableHiLoPerEntityGeneratorFixture : NHibernateTestCase
    {
        protected override IList<string> Mappings
        {
            get { return new[] { @"IdGenerators.CityIdGen.hbm.xml", @"IdGenerators.PersonIdGen.hbm.xml" }; }
        }

        protected override void OnSetUp()
        {
            var p1 = new Person { FullName = "Alberto D.", Age = 36 };
            var p2 = new Person { FullName = "Igor A.", Age = 31 };
            var p3 = new Person { FullName = "Luca M.", Age = 40 };

            var c1 = new City { Name = "Piacenza" };
            var c2 = new City { Name = "Parma" };
            var c3 = new City { Name = "Reggio E." };

            CommitInNewSession(uow =>
                {
                    uow.Save(p1);
                    uow.Save(p2);
                    uow.Save(p3);

                    uow.Save(c1);
                    uow.Save(c2);
                    uow.Save(c3);
                });
        }

        protected override void OnTearDown()
        {
            CommitInNewSession(uow =>
                {
                    uow.CreateQuery("delete from Person").ExecuteUpdate();
                    uow.CreateQuery("delete from City").ExecuteUpdate();
                });
        }

        [Test]
        public void DiverseEntityHannoLoStessoId()
        {
            Person person = null;
            City city = null;

            CommitInNewSession(uow =>
                {
                    person = uow.QueryOver<Person>().Where(p => p.FullName == "Alberto D.").SingleOrDefault<Person>();
                    city = uow.QueryOver<City>().Where(c => c.Name == "Piacenza").SingleOrDefault<City>();
                });

            person.Id.Should().Be.EqualTo(city.Id);
        }
    }

    [Serializable]
    public class Person
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }

        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Person);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    [Serializable]
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
