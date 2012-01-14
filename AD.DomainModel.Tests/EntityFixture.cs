using NUnit.Framework;
using SharpTestsEx;

namespace AD.DomainModel.Tests
{
    [TestFixture]
    public class EntityFixture
    {
        [Test]
        public void DiversoDaQualsiasiOggetto()
        {
            var e1 = DomainObjectStub.NewWithId(1);

            ((object) null).Should().Not.Be.EqualTo(e1);
            new object().Should().Not.Be.EqualTo(e1);
        }

        [Test]
        public void IstanzePersistentiConIdDiversi()
        {
            const int ID1 = 1;
            const int ID2 = 2;
            var e1 = DomainObjectStub.NewWithId(ID1);
            var e2 = DomainObjectStub.NewWithId(ID2);

            e2.Should().Not.Be.EqualTo(e1);
            e2.Should().Be.EqualTo(e2);
            Assert.AreNotEqual(1, e1);
        }

        [Test]
        public void IstanzePersistentiDiClassiDiverseConStessoId()
        {
            EntityStubA.NewWithId(1).Should().Not.Be.EqualTo(DomainObjectStub.NewWithId(1));
        }

        [Test]
        public void IstanzeTransientiSonoDiverse()
        {
            (new DomainObjectStub()).Should().Not.Be.EqualTo(new DomainObjectStub());
        }

        [Test]
        public void ClassiEreditateConLoStessoIdSonoUguali()
        {
            EntityStubInherit.NewWithId(1).Should().Be.EqualTo(DomainObjectStub.NewWithId(1));
        }

        [Test]
        [Description("Anche se cambia l'Id, l'HashCode non cambia")]
        public void HashNonCambiaCambiandoId()
        {
            var e = new DomainObjectStub();
            var oldHash = e.GetHashCode();
            e.SetId(100);
            e.GetHashCode().Should().Be.EqualTo(oldHash);
        }

        [Test]
        public void HashCode()
        {
            const int ID1 = 1;
            (new DomainObjectStub()).GetHashCode()
                .Should("oggetti transienti non devono avere lo stesso hashcode.")
                .Not.Be.EqualTo((new DomainObjectStub()).GetHashCode());

            DomainObjectStub.NewWithId(ID1).GetHashCode()
                .Should("oggetti persistenti devono avere lo stesso hash.")
                .Be.EqualTo(DomainObjectStub.NewWithId(ID1).GetHashCode());

            DomainObjectStub.NewWithId(ID1).GetHashCode()
                .Should("oggetti persistenti con ereditarietà devono avere lo stesso hash.")
                .Be.EqualTo(EntityStubInherit.NewWithId(ID1).GetHashCode());
        }

    }

    public class DomainObjectStub : Entity<int>
    {
        public override int Id { get; set; }

        public static DomainObjectStub NewWithId(int id)
        {
            return new DomainObjectStub { Id = id };
        }

        public void SetId(int id)
        {
            Id = id;
        }
    }

    public class EntityStubA : Entity<int>
    {
        public override int Id { get; set; }

        public static EntityStubA NewWithId(int id)
        {
            return new EntityStubA { Id = id };
        }
    }

    public class EntityStubInherit : DomainObjectStub
    {
        public new static EntityStubInherit NewWithId(int id)
        {
            return new EntityStubInherit { Id = id };
        }
    }
}
