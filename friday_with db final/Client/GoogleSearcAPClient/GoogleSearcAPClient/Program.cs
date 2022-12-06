using GoogleSearcAPClient.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace GoogleSearcAPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // DataLayer dl = new DataLayer();
            // var result = dl.InsertSMS("Awais  hy", "Yes");//dl.GetDataByQuery("what");

            Application.EnableVisualStyles();      //enable visual styles for the application.
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Interface());


            //GoogleSearchAPIProxy proxy = new GoogleSearchAPIProxy();
            //proxy.GetGoogleSearchData("DBMS");
           
            //Console.ReadLine();
        }
    }
}
