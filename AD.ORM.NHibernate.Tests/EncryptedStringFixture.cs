using System;
using System.Collections.Generic;
using NHibernate;
using NUnit.Framework;

namespace AD.ORM.NHibernate.Tests
{
    [TestFixture]
    public class EncryptedStringFixture : NHibernateTestCase
    {
        protected override IList<string> Mappings
        {
            get { return new[] { "User.hbm.xml" }; }
        }

        protected override void OnTearDown()
        {
            using (ISession s = OpenSession())
            {
                s.Delete("from User");
                s.Flush();
            }
        }

        [Test]
        public void SalvaStringaCrittografataELaRilegge()
        {
            var user = new User { Id = 1, Name = "Joe Satriani", Password = "ibanez" };

            using (ISession s = OpenSession())
            {
                s.Save(user);
                s.Flush();
            }

            using (ISession s = OpenSession())
            {
                var decryptedUser = s.Get<User>(1);

                Assert.AreEqual(1, decryptedUser.Id);
                Assert.AreEqual("Joe Satriani", decryptedUser.Name);
                Assert.AreEqual("ibanez", decryptedUser.Password);
            }
        }

        [Test]
        public void SalvaStringaCrittografataNullaELaRilegge()
        {
            var user = new User { Id = 2, Name = "Eric Clapton", Password = null };

            using (ISession s = OpenSession())
            {
                s.Save(user);
                s.Flush();
            }

            using (ISession s = OpenSession())
            {
                var decryptedUser = s.Get<User>(2);

                Assert.AreEqual(2, decryptedUser.Id);
                Assert.AreEqual("Eric Clapton", decryptedUser.Name);
                Assert.IsNull(decryptedUser.Password);
            }
        }
    }

    [Serializable]
    public class User
    {
        public int Id { get; set;}
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
