using System;
using System.Collections.Generic;
using AD.ORM.NHibernate.Extensions;
using AD.Shared.Collections;
using NUnit.Framework;
using SharpTestsEx;

namespace AD.ORM.NHibernate.Tests
{
    [TestFixture]
    public class ToPagedListExtensionFixture : NHibernateTestCase
    {
        protected override IList<string> Mappings
        {
            get { return new[] { "Person.hbm.xml" }; }
        }

        protected override void OnTearDown()
        {
            CommitInNewSession(uow => uow.Delete("from Person"));
        }

        protected override void OnSetUp()
        {
            CommitInNewSession(uow =>
            {
                uow.Save(new Person { FullName = "Alberto D.", Age = 36 });
                uow.Save(new Person { FullName = "Igor A.", Age = 31 });
                uow.Save(new Person { FullName = "Luca M.", Age = 40 });
            });
        }

        [Test]
        public void ReadPersonsInPagedModeUsingQueryOver()
        {
            int pageIndex = 0;
            const int PAGE_SIZE = 2;

            IPagedList<Person> people = null;
            CommitInNewSession( uow =>
                                    {
                                        people = uow.QueryOver<Person>().ToPagedList(pageIndex, PAGE_SIZE);
                                    });
            people.Count.Should().Be.EqualTo(PAGE_SIZE);
            
            people.Clear();
            pageIndex++;

            CommitInNewSession(uow =>
                                    {
                                        people = uow.QueryOver<Person>().ToPagedList(pageIndex, PAGE_SIZE);
                                    });
            people.Count.Should().Be.EqualTo(1);
        }

        [Test]
        [Ignore("Still in progress")]
        public void ReadPersonsInPagedModeUsingCriteria()
        {
            int pageIndex = 0;
            const int PAGE_SIZE = 2;

            IPagedList<Person> people = null;
            CommitInNewSession(uow =>
                                   {
                                       people = uow.CreateCriteria<Person>().ToPagedList<Person>(pageIndex, PAGE_SIZE);
                                   });
            people.Count.Should().Be.EqualTo(PAGE_SIZE);

            people.Clear();
            pageIndex++;

            CommitInNewSession(uow =>
                                    {
                                        people = uow.CreateCriteria<Person>().ToPagedList<Person>(pageIndex, PAGE_SIZE);
                                    });
            people.Count.Should().Be.EqualTo(1);
        }
    }

    [Serializable]
    public class Person
    {
        public string Id { get; protected set; }
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
}
