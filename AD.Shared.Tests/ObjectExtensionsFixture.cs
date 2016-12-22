namespace AD.Shared.Tests
{
    using System;
    using Extensions;
    using NUnit.Framework;
    using SharpTestsEx;

    [TestFixture]
    public class ObjectExtensionsFixture
    {
        [Test]
        public void CloneObject_DifferentInstances_ReturnsTrue()
        {
            Person p1 = new Person { FirstName = "Alberto", LastName = "Dallagiacoma" };
            Person p2 = p1.Clone<Person>();

            p2.Should().Not.Be.SameInstanceAs(p1);
        }

        [Test]
        public void CloneNonSerializabledObject_ThrowException()
        {
            NonSerializablePerson p1 = new NonSerializablePerson { FirstName = "Alberto", LastName = "Dallagiacoma" };
            Assert.Throws<InvalidOperationException>(() => p1.Clone<NonSerializablePerson>());
        }

        [Test]
        public void CloneNullObject_ReturnsDefault()
        {
            Person p1 = null;
            Person p2 = p1.Clone<Person>();

            p2.Should().Be.Null();
        }

        [Serializable]
        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class NonSerializablePerson
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
