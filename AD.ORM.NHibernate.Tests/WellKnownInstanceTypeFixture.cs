using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;
using SharpTestsEx;

namespace AD.ORM.NHibernate.Tests
{
	[TestFixture]
	public class WellKnownInstanceTypeFixture : NHibernateTestCase
	{
		protected override IList<string> Mappings
		{
			get
			{ 
				return new[] {"Dress.hbm.xml"};
			}
		}

		protected override void OnTearDown()
		{
			CommitInNewSession(uow => uow.Delete("from Dress"));
		}

		[Test]
		public void SalvaELeggeUnOggetto()
		{
			object savedId = null;
			CommitInNewSession(uow =>
								   {
									   savedId = uow.Save(new Dress { Description = "T-Shirt", Size = Sizes.Medium });
								   });

			CommitInNewSession(uow =>
									{
										Dress d = uow.Get<Dress>(savedId);
										d.Size.Should().Be.EqualTo(Sizes.Medium);
									});
		}

		[Test]
		public void SalvaEAggiornaUnOggetto()
		{
			object savedId = null;
			CommitInNewSession(uow =>
								   {
									   savedId = uow.Save(new Dress { Description = "T-Shirt", Size = Sizes.Medium });
								   });

			CommitInNewSession(uow =>
								   {
									   Dress dress = uow.Get<Dress>(savedId);
									   dress.Size.Should().Be.EqualTo(Sizes.Medium);
								   });
			

			CommitInNewSession(uow =>
								   {
									   Dress dress = uow.Get<Dress>(savedId);
									   dress.Size = Sizes.Large;
									   uow.Save(dress);
								   });

			CommitInNewSession(uow =>
								   {
									   Dress dress = uow.Get<Dress>((int)savedId);
									   dress.Size.Should().Be.EqualTo(Sizes.Large);
								   });
		}

		[Test]
		public void Query()
		{
			CommitInNewSession(uow =>
			{
				uow.Save(new Dress { Description = "T-Shirt", Size = Sizes.Medium });
				uow.Save(new Dress { Description = "Jeans", Size = Sizes.Small});
			});

			CommitInNewSession(uow =>
									{
										var list = uow.CreateQuery("from Dress d where d.Size = :size")
											.SetParameter("size", Sizes.Small)
											.List<Dress>();
										list.Count.Should().Be.EqualTo(1);
										list[0].Description.Should().Be.EqualTo("Jeans");
									});
		}
	}

	[Serializable]
	public class Size
	{
		internal Size(int id)
		{
			Id = id;
		}
		public int Id { get; protected set; }
		public string Description { get; set; }

		public bool Equals(Size other)
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
			return Equals(obj as Size);
		}
		
		public override int GetHashCode()
		{
			return Id;
		} 
	}

	public class Sizes : ReadOnlyCollection<Size>
	{
		public static Size Small = new Size(1) { Description = "Small" };
		public static Size Medium = new Size(2) { Description = "Medium" };
		public static Size Large = new Size(3) { Description = "Large" };
		public Sizes()
			: base(new[] { Small, Medium, Large})
		{ }
	}

	[Serializable]
	public class SizeType : WellKnownInstanceType<Size>
	{
		public SizeType()
			: base(new Sizes(), (e, k) => e.Id == k, e => e.Id )
		{ }
	}

	public class Dress
	{
		public string Description { get; set; }
		public Size Size { get; set; }
	}
}
