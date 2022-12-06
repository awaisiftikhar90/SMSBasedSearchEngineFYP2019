using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSearcAPClient.Database
{
    public class DataLayer
    {
        public void GetData()
        {
            String connectionstring = "Provider=Microsoft.Jet.OLEDB.4.0; DataSource=D:\\project\\29-03-2019\\Client\\GoogleSearcAPClient\\GoogleSearcAPClient\\Database\\SMS_DB.accdb";
            OleDbConnection conn = null;
            OleDbCommand cmd = null;
            OleDbDataReader rdr = null;
            String mySQL = "SELECT * SMS_Offline";

            try
            {
                conn = new OleDbConnection(connectionstring);
                conn.Open();


                cmd = new OleDbCommand(mySQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.Write(String.Format("{0}\n,{1}\n", rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString()));
                }
                Console.Read();

                conn.Close();
            }
            catch (Exception ex)
            {
                Console.Error.Write("Error founded: " + ex.Message);
            }
            finally
            {
                if (conn != null) conn.Dispose();
                if (cmd != null) cmd.Dispose();
                if (rdr != null) rdr.Dispose();
            }
        }

    }
}
