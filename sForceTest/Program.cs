using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sForceTest.sForce;


namespace sForceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ForceLogin test = new ForceLogin();

           Console.Write(test.Login());
            test.Query();

            test.Logout();


           

            
        
        }
    }
}
