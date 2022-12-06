using System;
using System.Collections.Generic;
//sing System.Data.OleDb;  //in order to connect access database Object Linking and Embedding, Database "object represents a unique connection to a data source  With a client/server database system, it is equivalent to a network connection to the server
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;

namespace GoogleSearcAPClient.DataBase
{
     public class DataLayer
    {       

        public List<SmsOffline> GetDataByQuery(string query)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString))
            {
                string sqlQuery = "select * from table_Record where Query like @query";
                return db.Query<SmsOffline>
                (sqlQuery, new { query= "%" + query + "%" }).ToList();
            }
        }

        public bool InsertSMS(String query, String answer)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString))
            {
                string insertQuery = @"INSERT INTO [dbo].[table_Record]([Query], [Answer]) VALUES (@query, @answer)";                
                var result = db.Execute(insertQuery, new
                {
                    Query=query,
                    Answer=answer
                });
            }
            return true;
        }        
    }
}