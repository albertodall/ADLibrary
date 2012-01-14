using System.Collections.Generic;
using System.Text;
using NHibernate.Dialect;
using NHibernate.Id;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;

namespace AD.ORM.NHibernate
{
    public class TableHiLoPerEntityGenerator : TableHiLoGenerator
    {
        public const string ENTITIES_LIST_PARAM = "entity_rows";
        public const string ENTITIES_LIST_DEFAULT_VALUE = "Entity";

        public const string ENTITY_NAME_COLUMN_PARAM = "entity_column";
        public const string ENTITY_NAME_COLUMN_DEFAULT_VALUE = "entity";

        private string _tableName;
        private string _columnName;
        private string _entityColumnName;
        private string _entitiesList;

        public override void Configure(IType type, IDictionary<string, string> parms, Dialect dialect)
        {
            base.Configure(type, parms, dialect);
            _tableName = PropertiesHelper.GetString(TableParamName, parms, DefaultTableName);
            _columnName = PropertiesHelper.GetString(ColumnParamName, parms, DefaultColumnName);
            _entitiesList = PropertiesHelper.GetString(ENTITIES_LIST_PARAM, parms, ENTITIES_LIST_DEFAULT_VALUE);
            _entityColumnName = PropertiesHelper.GetString(ENTITY_NAME_COLUMN_PARAM, parms, ENTITY_NAME_COLUMN_DEFAULT_VALUE);
        }

        public override string[] SqlCreateStrings(Dialect dialect)
        {
            var createTableString = new StringBuilder(512);
            createTableString.AppendFormat("create table {0} ( ", _tableName);
            createTableString.AppendFormat("{0} {1}, ", _entityColumnName, dialect.GetTypeName(SqlTypeFactory.GetString(64)));
            createTableString.AppendFormat("{0} {1} )", _columnName, dialect.GetTypeName(columnSqlType));

            var insertString = new StringBuilder(1024);
            foreach (var entityName in _entitiesList.Split(','))
            {
                insertString.AppendFormat("insert into {0} values ( \"{1}\", 1 );", _tableName, entityName);
            }

            return new[] { createTableString.ToString(), insertString.ToString() };
        }
    }
}
