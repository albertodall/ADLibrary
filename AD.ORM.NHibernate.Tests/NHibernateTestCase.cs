using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using log4net.Config;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace AD.ORM.NHibernate.Tests
{
	/// <summary>
	/// Ported from NH official tests.
	/// </summary>
	public abstract class NHibernateTestCase
	{
		private const bool OUTPUT_DDL = false;
		private static readonly ILog Log = LogManager.GetLogger(typeof(NHibernateTestCase)); 

		protected Configuration Config; 
		protected ISessionFactoryImplementor Sessions;

	    private DebugConnectionProvider _connectionProvider;                  
		
		/// <summary>                 
		/// /// Mapping files used in the TestCase                 
		/// </summary>                 
		protected abstract IList<string> Mappings { get; } 

		/// <summary>
		/// Assembly to load mapping files from.
		/// </summary>
		protected virtual string MappingsAssembly
		{
			get { return "AD.ORM.NHibernate.Tests"; }
		}

	    public ISession LastOpenedSession { get; private set; }

	    static NHibernateTestCase()
		{
			XmlConfigurator.Configure();
		}

		/// <summary>
		/// Creates the tables used in this TestCase
		/// </summary>
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			try
			{
				Configure();
				CreateSchema();
				BuildSessionFactory();
			}
			catch (Exception e)
			{
				Log.Error("Error while setting up the test fixture", e);
				throw;
			}
		}

		/// <summary>
		/// Removes the tables used in this TestCase.
		/// </summary>
		/// <remarks>
		/// If the tables are not cleaned up sometimes SchemaExport runs into
		/// Sql errors because it can't drop tables because of the FKs.  This 
		/// will occur if the TestCase does not have the same hbm.xml files
		/// included as a previous one.
		/// </remarks>
		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			DropSchema();
			Cleanup();
		}

		protected virtual void OnSetUp() { }

		/// <summary>
		/// Set up the test. This method is not overridable, but it calls
		/// <see cref="OnSetUp" /> which is.
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			OnSetUp();
		}

		protected virtual void OnTearDown() { }

		/// <summary>
		/// Checks that the test case cleans up after itself. This method
		/// is not overridable, but it calls <see cref="OnTearDown" /> which is.
		/// </summary>
		[TearDown]
		public void TearDown()
		{
			OnTearDown();

			bool wasClosed = CheckSessionWasClosed();
			bool wasCleaned = CheckDatabaseWasCleaned();
			bool wereConnectionsClosed = CheckConnectionsWereClosed();
			bool fail = !wasClosed || !wasCleaned || !wereConnectionsClosed;

			if (fail)
			{
				Assert.Fail("Test didn't clean up after itself");
			}
		}

		private bool CheckSessionWasClosed()
		{
			if (LastOpenedSession != null && LastOpenedSession.IsOpen)
			{
				Log.Error("Test case didn't close a session, closing");
				LastOpenedSession.Close();
				return false;
			}

			return true;
		}

		private bool CheckDatabaseWasCleaned()
		{
			if (Sessions.GetAllClassMetadata().Count == 0)
			{
				// Return early in the case of no mappings, also avoiding
				// a warning when executing the HQL below.
				return true;
			}

			bool empty;
			using (ISession s = Sessions.OpenSession())
			{
				empty = s.CreateQuery("from System.Object o").List().Count == 0;
			}

			if (!empty)
			{
				Log.Error("Test case didn't clean up the database after itself, re-creating the schema");
				DropSchema();
				CreateSchema();
			}

			return empty;
		}

		private bool CheckConnectionsWereClosed()
		{
			if (_connectionProvider == null || !_connectionProvider.HasOpenConnections)
			{
				return true;
			}

			Log.Error("Test case didn't close all open connections, closing");
			_connectionProvider.CloseAllConnections();
			return false;
		}

		protected virtual void Configure()
		{
			Config = new Configuration();
			Assembly assembly = Assembly.Load(MappingsAssembly);
			Configure(Config);

			foreach (var file in Mappings)
			{
				Config.AddResource(MappingsAssembly + "." + file, assembly);
			}
		}

		private void CreateSchema()
		{
			new SchemaExport(Config).Create(OUTPUT_DDL, true);
		}

		private void DropSchema()
		{
			new SchemaExport(Config).Drop(OUTPUT_DDL, true);
		}

		protected virtual void BuildSessionFactory()
		{
			Sessions = (ISessionFactoryImplementor)Config.BuildSessionFactory();
			_connectionProvider = Sessions.ConnectionProvider as DebugConnectionProvider;
		}

		private void Cleanup()
		{
			Sessions.Close();
			Sessions = null;
			_connectionProvider = null;
			LastOpenedSession = null;
			Config = null;
		}

		protected virtual ISession OpenSession()
		{
			LastOpenedSession = Sessions.OpenSession();
			return LastOpenedSession;
		}

		#region Properties overridable by subclasses

		protected virtual void Configure(Configuration configuration)
		{
			configuration.Configure();
		}

		#endregion

		protected void CommitInNewSession(Action<ISession> executeInNewSession)
		{
			using (var session = Sessions.OpenSession())
			{
				using (var tx = session.BeginTransaction())
				{
					executeInNewSession(session);
					session.Flush();
					tx.Commit();
				}
			}
		}

		protected bool ExistsInDb<T>(object id)
		{
			using (var s = OpenSession())
			{
				return !ReferenceEquals(s.Get<T>(id), null);
			}
		}
	}
}
