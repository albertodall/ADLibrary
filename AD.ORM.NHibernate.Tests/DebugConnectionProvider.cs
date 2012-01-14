using System.Data;
using Iesi.Collections;
using NHibernate.Connection;

namespace AD.ORM.NHibernate.Tests
{
    /// <summary>
    /// Ported from NH official tests.
    /// This connection provider keeps a list of all open connections,
    /// it is used when testing to check that tests clean up after themselves.
    /// </summary>
    public class DebugConnectionProvider : DriverConnectionProvider
    {
        private readonly ISet _connections;

        public DebugConnectionProvider()
        {
            _connections = new ListSet();
        }

        public override IDbConnection GetConnection()
        {
            IDbConnection connection = base.GetConnection();
            _connections.Add(connection);
            return connection;
        }

        public override void CloseConnection(IDbConnection conn)
        {
            base.CloseConnection(conn);
            _connections.Remove(conn);
        }

        public bool HasOpenConnections
        {
            get
            {
                // check to see if all connections that were at one point opened
                // have been closed through the CloseConnection
                // method
                if (_connections.IsEmpty)
                {
                    // there are no connections, either none were opened or
                    // all of the closings went through CloseConnection.
                    return false;
                }
                else
                {
                    // Disposing of an ISession does not call CloseConnection (should it???)
                    // so a Diposed of ISession will leave an IDbConnection in the list but
                    // the IDbConnection will be closed (atleast with MsSql it works this way).
                    foreach (IDbConnection conn in _connections)
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            return true;
                        }
                    }

                    // all of the connections have been Disposed and were closed that way
                    // or they were Closed through the CloseConnection method.
                    return false;
                }
            }
        }

        public void CloseAllConnections()
        {
            while (!_connections.IsEmpty)
            {
                var en = _connections.GetEnumerator();
                en.MoveNext();
                CloseConnection(en.Current as IDbConnection);
            }
        }
    }
}
