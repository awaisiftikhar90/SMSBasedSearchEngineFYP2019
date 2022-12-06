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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Interface());
            //GoogleSearchAPIProxy proxy = new GoogleSearchAPIProxy();
            //proxy.GetGoogleSearchData("rawalpindi weather");

            //Console.ReadLine();
        }
    }
}
