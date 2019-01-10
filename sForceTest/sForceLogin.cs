using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using sForceTest.sForce;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace sForceTest
{
    public class ForceLogin

    {
        public static SoapClient client;
        private static SoapClient loginclient;
        private static SessionHeader sessionHeader;
        private static EndpointAddress endPoint;
        private static QueryResult queryresult;

        public bool Login()
        {
            string username = "cswany@kinetixhr.com";
            string password = "HikariCat18o1EEyBAtvN0NYh01v24mlLZv";
            BasicHttpBinding binding1 = new BasicHttpBinding();
            binding1.Security.Mode = BasicHttpSecurityMode.Transport;



            loginclient = new SoapClient();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            LoginResult lr = loginclient.login(null, username, password);

            endPoint = new EndpointAddress(lr.serverUrl);

            sessionHeader = new SessionHeader
            {
                sessionId = lr.sessionId

            };

            client = new SoapClient(binding1, endPoint);


            return true;


        }


        public void Logout()
        {
            client.logout(sessionHeader);

        }


        public static string Serialize(object input)
        {
            if (input == null)
                return null;

            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(input.GetType());

            using (MemoryStream ms = new MemoryStream())
            using (StreamReader sr = new StreamReader(ms))
            {
                ser.Serialize(ms, input);
                ms.Seek(0, 0);
                return sr.ReadToEnd();

            }

        }


        public void Query()

        {
            QueryResult queryResult = new QueryResult();

            string SOQL = "SELECT ID FROM TR1__Closing_Report__c WHERE TR1__Closing_Report__c.LastModifiedDate >= YESTERDAY ";

            client.query(sessionHeader, null, null, null, SOQL, out queryResult);
            Boolean done = false;

            if (queryResult.size > 0)

                while (!done)
                {
                    foreach (TR1__Closing_Report__c ClosingReport in queryResult.records)
                    {

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"c:\users\cswany\Desktop\Placement ID TR.xml", true))
                        {
                            file.WriteLine(ClosingReport.Id);

                        }
                    //   System.IO.File. Console.Write.(Convert.ToString(ClosingReport.Id)); 


                    }
                    if (queryResult.done)
                    {
                        done = true;
                    }
                    else
                    {
                        client.queryMore(sessionHeader, null, queryResult.queryLocator, out queryResult);

                    }



                }
           // System.IO.File.WriteAllLines(@"c:\users\cswany\Desktop\Placement ID TR.xml", Serialize(queryresult)); 






        }
    }
}