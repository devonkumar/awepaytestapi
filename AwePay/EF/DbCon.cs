using Dapper;
using AwePay.Common;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AwePay.EF
{
    public interface IDbCon
    {

    }
    public class DbCon : IDbCon
    {

        string sqlconn;
        private IConfiguration _config;

        public DbCon(IConfiguration config)
        {
            // string envir = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //  string jfile = (envir == "Development") ? "appsettings.json" : "appsettings." + envir + ".json";
            //   AppSetting = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(jfile).Build();

            //AppSetting["ConnectionStrings:PosConn"];
            //string conString = Microsoft.Extensions.Configuration.ConfigurationExtensions.GetConnectionString(this.Configuration, "DefaultConnection");
            _config = config;
            //AppSetting.GetConnectionString("ExConn");
            sqlconn = _config.GetConnectionString("devcon");

        }

        //public IEnumerable<DataRow> AsEnumerable(this DataTable table)
        //{
        //    for (int i = 0; i < table.Rows.Count; i++)
        //    {
        //        yield return table.Rows[i];
        //    }
        //}

        //public int insert(string sql)
        //{


        //    int ret;

        //    SqlConnection conn = new SqlConnection(sqlconn);

        //    //string sqlq = "INSERT INTO Currencies (name,sym,pref,sfe,rfe,spc,rpc,nonf,memf,sver,rver) "+
        //    //            "VALUES('"+C.name+"','"+C.sym+"')";
        //    try
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.ExecuteNonQuery();

        //        ret = 1;
        //    }
        //    catch (System.Data.SqlClient.SqlException ex)
        //    {
        //        ret = 0;
        //        string msg = "Insert Error:";
        //        msg += ex.Message;
        //        throw new Exception(msg);
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }

        //    return ret;
        //}


        //public int update(string sql)
        //{

        //    SqlConnection conn = new SqlConnection(sqlconn);

        //    try
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (System.Data.SqlClient.SqlException ex)
        //    {
        //        string msg = "Update Error:";
        //        msg += ex.Message;
        //        throw new Exception(msg);
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }

        //    return 1;
        //}


        //public int Delete(string sql)
        //{
        //    SqlConnection conn = new SqlConnection(sqlconn);

        //    try
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand(sql, conn);
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (System.Data.SqlClient.SqlException ex)
        //    {
        //        string msg = "Delete Error:";
        //        msg += ex.Message;
        //        throw new Exception(msg);
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }

        //    return 1;
        //}


        public async Task<DRQueRes<T>> GetList<T>(string sql, PageQuery @p = null)
        {
            @p.SortBy = (@p.SortBy == "Id") ? "t1.Id" : @p.SortBy;
            @p.PageSize = (@p.PageSize == 0) ? 10 : @p.PageSize;


            if ((@p.SearchCol ?? "").Length > 0)
            {
                sql += $" AND {@p.SearchCol} LIKE '%{@p.SearchBy}%' ";
            }
            string count = sql + ";";

            sql += $" ORDER BY {@p.SortBy} {@p.SortDir} LIMIT {@p.PageOff},{@p.PageSize}; ";

            //List<T> list = new List<T>();

            using (IDbConnection con = new MySqlConnection(sqlconn))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var multi = await con.QueryMultipleAsync(sql + count);

                var list = multi.Read<T>().ToList();
                int cnt = (list.Count() > 0) ? multi.Read<int>().First() : 0;
                @p.ResCount = cnt;

                var ret = new DRQueRes<T>(list, cnt);
                return ret;

            }
        }


        public async Task<List<T>> SqlToList<T>(string sql, object param = null)
        {
            using (IDbConnection con = new MySqlConnection(sqlconn))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var k = await con.QueryAsync<T>(sql, param);

                List<T> list = k.ToList();
                return list;

            }


        }


        public async Task<T> SqlToEnt<T>(string sql, object param = null) where T : new()
        {
            T obj = new T();
            if (sql == "")
                return obj;

            using (IDbConnection con = new MySqlConnection(sqlconn))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                //DynamicParameters parameter = new DynamicParameters();
                //parameter.Add("@Id", id);
                //groupMeeting = con.Query<GroupMeeting>("GetGroupMeetingByID", parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();

                obj = await con.QueryFirstOrDefaultAsync<T>(sql, param);

            }

            return obj;
        }

        public async Task<SPReturn> SPROC(string sql, object param = null)
        {


            using (IDbConnection con = new MySqlConnection(sqlconn))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var k = await con.QueryFirstOrDefaultAsync<SPReturn>(sql, param, commandType: CommandType.StoredProcedure);
                ;
                return k;

            }
        }

        public async Task<T> SPROC<T>(string sql, object param = null) where T : new()
        {

            T obj = new T();
            if (sql == "")
                return obj;


            using (IDbConnection con = new MySqlConnection(sqlconn))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var k = await con.QueryFirstOrDefaultAsync<T>(sql, param, commandType: CommandType.StoredProcedure);

                return k;

            }
        }

        public async Task<List<T>> SPROCLIST<T>(string sql, object param = null) where T : new()
        {

            using (IDbConnection con = new MySqlConnection(sqlconn))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var k = await con.QueryAsync<T>(sql, param, commandType: CommandType.StoredProcedure);

                List<T> list = k.ToList();
                return list;
            }
        }

        public async Task<(List<T>, List<Q>)> SPROCLIST<T, Q>(string sql) where T : new() where Q : new()
        {

            using (IDbConnection con = new MySqlConnection(sqlconn))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var res = await con.QueryMultipleAsync(sql, commandType: CommandType.StoredProcedure);

                var table1 = await res.ReadAsync<T>();
                var table2 = await res.ReadAsync<Q>();

                return (table1.ToList(), table2.ToList());
            }
        }



        public async Task<int> Execute(string sql, object param = null)
        {
            int affectedRows = 0;

            using (IDbConnection con = new MySqlConnection(sqlconn))
            {
                affectedRows = con.ExecuteAsync(sql, param).Result;


            }

            return affectedRows;
        }


        public async Task<int> Count(string sql, object param = null)
        {
            int cntRows = 0;

            using (IDbConnection con = new MySqlConnection(sqlconn))
            {
                cntRows = con.ExecuteScalar<int>(sql);
            }

            return cntRows;
        }



    }
}
