using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NHibernate.SqlTypes;
using NUnit.Framework;
using SharpTestsEx;

namespace AD.ORM.NHibernate.Tests
{
	[TestFixture]
	public class GenericWellKnownInstanceTypeFixture : NHibernateTestCase
	{
		protected override IList<string> Mappings
		{
			get
			{ 
				return new[] {"Citizen.hbm.xml"};
			}
		}

		protected override void OnTearDown()
		{
			CommitInNewSession(uow => uow.Delete("from Citizen"));
		}

		[Test]
		public void SalvaELeggeUnOggetto()
		{
			object savedId = null;
			CommitInNewSession(uow =>
								   {
									   savedId = uow.Save(new Citizen { FullName = "Alberto D.", City = Cities.Parma });
								   });

			CommitInNewSession(uow =>
								   {
									   Citizen c = uow.Get<Citizen>(savedId);
									   c.City.Should().Be.EqualTo(Cities.Parma);
								   });
		}

		[Test]
		public void SalvaEAggiornaUnOggetto()
		{
			object savedId = null;
			CommitInNewSession(uow =>
								   {
									   savedId = uow.Save(new Citizen { FullName = "Igor A.", City = Cities.Piacenza });
								   });               

			CommitInNewSession(uow =>
								   {
									   Citizen citizen = uow.Get<Citizen>(savedId);
									   citizen.City.Should().Be.EqualTo(Cities.Piacenza);
								   });

			CommitInNewSession(uow =>
								   {
									   Citizen citizen = uow.Get<Citizen>(savedId);
									   citizen.City = Cities.ReggioEmilia;
									   uow.Save(citizen);
								   });
			
			CommitInNewSession(uow =>
								   {
									   Citizen citizen = uow.Get<Citizen>(savedId);
									   citizen.City.Should().Be.EqualTo(Cities.ReggioEmilia);
								   });
		}

		[Test]
		public void Query()
		{
			CommitInNewSession(uow =>
			{
				uow.Save(new Citizen { FullName = "Alberto D.", City = Cities.Parma, TVBrand = Brands.Sony});
				uow.Save(new Citizen { FullName = "Igor A.", City = Cities.Piacenza, TVBrand = Brands.Sony });
				uow.Save(new Citizen { FullName = "Luca M.", City = Cities.Piacenza, TVBrand = Brands.Panasonic });
			});

			CommitInNewSession(uow =>
			{
				var list = uow.CreateQuery("from Citizen c where c.City = :city")
					.SetParameter("city", Cities.Piacenza)
					.List<Citizen>();
				list.Count.Should().Be.EqualTo(2);
			});
		}
	}

	[Serializable]
	public class City
	{
		internal City(string id)
		{
			Id = id;
		}
		public string Id { get; protected set; }
		public string Name { get; set; }

		public bool Equals(City other)
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
			return Equals(obj as City);
		}
		
		public override int GetHashCode()
		{
			return Id.GetHashCode();
		} 
	}

	public class Cities : ReadOnlyCollection<City>
	{
		public static City Piacenza = new City("PC") { Name = "Piacenza" };
		public static City Parma = new City("PR") { Name = "Parma" };
		public static City ReggioEmilia = new City("RE") { Name = "Reggio Emilia" };
		public Cities()
			: base(new[] { Piacenza, Parma, ReggioEmilia})
		{ }
	}

	[Serializable]
	public class CityType : GenericWellKnownInstanceType<City, string>
	{
		public CityType()
			: base(new Cities(), (e, k) => e.Id == k, e => e.Id )
		{ }

		public override SqlType[] SqlTypes
		{
			get { return new[] {SqlTypeFactory.GetString(2)}; }
		}
	}

	[Serializable]
	public class Brand
	{
		internal Brand(short id)
		{
			Id = id;
		}
		public short Id { get; protected set; }
		public string Name { get; set; }

		public bool Equals(Brand other)
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
			return Equals(obj as Brand);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}

	public class Brands : ReadOnlyCollection<Brand>
	{
		public static Brand Sony = new Brand(1) { Name = "Sony" };
		public static Brand Philips = new Brand(2) { Name = "Philips" };
		public static Brand Panasonic = new Brand(3) { Name = "Panasonic" };
		public Brands()
			: base(new[] { Sony, Philips, Panasonic })
		{ }
	}

	[Serializable]
	public class BrandType : GenericWellKnownInstanceType<Brand, short>
	{
		public BrandType()
			: base(new Brands(), (e, k) => e.Id == k, e => e.Id)
		{ }

		public override SqlType[] SqlTypes
		{
			get { return new[] { SqlTypeFactory.Int16 }; }
		}
	}

	[Serializable]
	public class Citizen
	{
		public string FullName { get; set; }
		public City City{ get; set; }
		public Brand TVBrand { get; set; }
	}
}
