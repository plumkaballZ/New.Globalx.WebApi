using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace New.Globalx.WebApi.Repos
{
    public class BaseRepo
    {
        private readonly string _connectingString;
        public BaseRepo()
        {
            var connectionStringBuilder = new MySqlConnectionStringBuilder
            {
                Server = "62.75.168.220",
                Database = "Globase",
                UserID = "superErbz",
                Password = "Jqi5fqfb",
                ConnectionTimeout = 60,
                Port = 3306,
                ConvertZeroDateTime = true
            };

            _connectingString = connectionStringBuilder.ConnectionString;
        }


        public T GetSingle<T>(string spName, Dictionary<string, object> paramsDic)
        {
            using var dbConnection = GetOpenConnection();

            var item = dbConnection.Query<T>(spName, DpsFromParaDic(paramsDic),
                commandType: CommandType.StoredProcedure).FirstOrDefault();

            return item;
        }
        public IEnumerable<T> GetList<T>(string spName, Dictionary<string, object> paramsDic)
        {
            using var con = GetOpenConnection();

            var items = con.Query<T>(spName, DpsFromParaDic(paramsDic),
                commandType: CommandType.StoredProcedure);

            return items;
        }
        public void ExecuteSp(string spName, Dictionary<string, object> paramsDic)
        {
            using var conn = GetOpenConnection();

            conn.Execute(spName, DpsFromParaDic(paramsDic),
                commandType: CommandType.StoredProcedure);
        }
        private MySqlConnection GetOpenConnection()
        {
            var conn = new MySqlConnection(_connectingString);
            conn.Open();
            return conn;
        }
        private static DynamicParameters DpsFromParaDic(Dictionary<string, object> paramDic)
        {
            var dps = new DynamicParameters();

            if (paramDic == null) return dps;

            foreach (var (key, value) in paramDic)
            {
                dps.Add(key[0] == '@' ? key : "læs" + "@" + key, value);
            }


            return dps;
        }
    }
}
