using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace gmp
{
    public class DbParameter
    {
        public string Name;
        public object Value;

        public DbParameter(string ParameterName, object ParameterValue)
        {
            Name = ParameterName;
            Value = ParameterValue;
        }
    }

    public abstract class DbConnect
    {
        protected static string ConnectionString;
        protected string QueryString;
        protected List<DbParameter> Parameters;

        public string Query
        {
            get { return QueryString; }
            set { QueryString = value; }
        }

        public void ClearParameters()
        {
            Parameters.Clear();
        }

        public void AddParameter(string Name, Object Value)
        {
            Parameters.Add(new DbParameter(Name, Value));
        }

        public bool CheckParameter(string Name)
        {
            Name = Name.ToLower();

            foreach (DbParameter Parameter in Parameters)
            {
                if (Parameter.Name.ToLower() == Name) return true;
            }

            return false;
        }

        public abstract void OpenConnection();

        public abstract void CloseConnection();

        public abstract void ExecuteNonQuery();

        public abstract DbDataReader ExecuteReader();

        public abstract Object ExecuteScalar();

        public static DbConnect CreateConnection()
        {
            switch (Properties.Settings.Default.DbType.ToLower())
            {
                case "mysql":
                    return new MySqlConnect();
                default:
                    throw new Exception("Unknown parameter DbType");
            }
        }
    }

    public class MySqlConnect : DbConnect
    {
        private MySqlConnection Connection;

        static MySqlConnect()
        {
            ConnectionString = "Server=" + Properties.Settings.Default.DbServer + ";"
                + "Port=" + Properties.Settings.Default.DbPort.ToString() + ";"
                + "Database=" + Properties.Settings.Default.DbDatabase + ";"
                + "Uid=" + Properties.Settings.Default.DbUser + ";"
                + "Password=" + Properties.Settings.Default.DbPassword + ";"
                + "Pooling=True;"
                + (Properties.Settings.Default.DbMinPoolSize > 0 ? "Min Pool Size=" + Properties.Settings.Default.DbMinPoolSize.ToString() + ";" : "")
                + (Properties.Settings.Default.DbMaxPoolSize > 0 ? "Max Pool Size=" + Properties.Settings.Default.DbMaxPoolSize.ToString() + ";" : "")
                + (Properties.Settings.Default.DbConnectTimeout > 0 ? "Connect Timeout=" + Properties.Settings.Default.DbConnectTimeout.ToString() + ";" : "");
        }

        public MySqlConnect()
        {
            Connection = new MySqlConnection(ConnectionString);
            Parameters = new List<DbParameter>();
        }

        ~MySqlConnect()
        {
            if (Connection.State != System.Data.ConnectionState.Closed) CloseConnection();
        }

        public override void OpenConnection()
        {
            try
            {
                if (Connection.State != System.Data.ConnectionState.Open) Connection.Open();
            }
            catch (Exception e)
            {
                throw new Exception((Properties.Settings.Default.DebugInfo ? e.Message : "Open database connection error"));
            }
        }

        public override void CloseConnection()
        {
            try
            {
                if (Connection.State != System.Data.ConnectionState.Closed) Connection.Close();
            }
            catch (Exception e)
            {
                throw new Exception((Properties.Settings.Default.DebugInfo ? e.Message : "Close database connection error"));
            }
        }

        public override void ExecuteNonQuery()
        {
            MySqlCommand Command = new MySqlCommand(Query, Connection);

            for (int i = 0; i < Parameters.Count(); i++)
            {
                Command.Parameters.AddWithValue(Parameters[i].Name, Parameters[i].Value);
            }

            Command.ExecuteNonQuery();
        }

        public override DbDataReader ExecuteReader()
        {
            MySqlCommand Command = new MySqlCommand(Query, Connection);

            for (int i = 0; i < Parameters.Count(); i++)
            {
                Command.Parameters.AddWithValue(Parameters[i].Name, Parameters[i].Value);
            }

            return Command.ExecuteReader();
        }

        public override Object ExecuteScalar()
        {
            MySqlCommand Command = new MySqlCommand(Query, Connection);

            for (int i = 0; i < Parameters.Count(); i++)
            {
                Command.Parameters.AddWithValue(Parameters[i].Name, Parameters[i].Value);
            }

            return Command.ExecuteScalar();
        }
    }

}