using Microsoft.SqlServer.Management.Smo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mssqlRebuildTables
{
    public class methods
    {
        public void Log_Save_File(string info, string urlpath, string value)
        {
            string path = Directory.GetCurrentDirectory();
            DateTime simdi = DateTime.Now;
            path = path + @"\Log";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = string.Format("MSSRT_LOG_{0}_{1}_{2}.txt", simdi.Year, simdi.Month, simdi.Day);
            fileName = path + @"\" + fileName;

            if (!File.Exists(fileName))
                File.Create(fileName).Close();

            FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(string.Format("{0};{1};{3};{2}", simdi.ToString("yyyy-MM-dd HH:mm:ss.FFF"), info, value, urlpath));
            sw.Close();
            fs.Close();
            sw.Dispose();
            fs.Dispose();

        }

        public void Info_Save_File(model_info model)
        {
            string path = Directory.GetCurrentDirectory();
            DateTime simdi = DateTime.Now;
            path = path + @"\Info";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //string fileName = string.Format("MSSRT_LOG_{0}_{1}_{2}.txt", simdi.Year, simdi.Month, simdi.Day);
            string fileName = string.Format("MSSRT_Info.txt");
            fileName = path + @"\" + fileName;

            if (File.Exists(fileName))
                File.Delete(fileName);

            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(string.Format("{0}", JsonConvert.SerializeObject(model)));
            sw.Close();
            fs.Close();
            sw.Dispose();
            fs.Dispose();

        }

        public model_info Info_Get_File()
        {
            string path = Directory.GetCurrentDirectory();
            DateTime simdi = DateTime.Now;
            path = path + @"\Info";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //string fileName = string.Format("MSSRT_LOG_{0}_{1}_{2}.txt", simdi.Year, simdi.Month, simdi.Day);
            string fileName = string.Format("MSSRT_Info.txt");
            fileName = path + @"\" + fileName;

            model_info e_inf = new model_info()
            {
                dbServerName="HATA",dbName="HATA",dbUserName="HATA",dbUserPass="HATA",timeHour=0,timeMinute=0,tryCount=0,waitTime=0
            };

            if (!File.Exists(fileName))
            {
                return e_inf;
            }
            else
            {
                string[] ss_arr = File.ReadAllLines(fileName);
                string res = string.Empty;

                for (int i = 0; i < ss_arr.Count(); i++)
                {
                    res += ss_arr[i];
                }

                return JsonConvert.DeserializeObject<model_info>(res);

            }

        }

        public bool Connect_test (string dbsrv,string dbname, string user, string pass)
        {
            bool result = false;

            Server srv = new Server();

            srv.ConnectionContext.LoginSecure = false;
            srv.ConnectionContext.Login = user;
            srv.ConnectionContext.Password = pass;
            srv.ConnectionContext.ServerInstance = dbsrv;
            string dbName = dbname;

            Database db = new Database();

            db = srv.Databases[dbName];

            if (db.Name == dbname)
                result = true;


            return result;
        }

    }
}
