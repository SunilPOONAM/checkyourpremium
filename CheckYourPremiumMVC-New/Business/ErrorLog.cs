using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
namespace Business
{
    public class ErrorLog
    {
        public void Error(string errorMessage, string innerException)
        {
            System.IO.StreamWriter sw;
            try
            {
                string filename = AppDomain.CurrentDomain.BaseDirectory + "log\\" + "logErrors" + DateTime.Today.ToString("yyyy_MM_dd") + ".log";
                String path = HttpContext.Current.Server.MapPath("~/log/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //File is not exist so create new file.
                if (!File.Exists(filename))
                {
                    FileStream fs = File.Create(filename);
                    fs.Close();
                }
                sw = new System.IO.StreamWriter(filename, true);
                sw.WriteLine("ErrorMessage" + " : " + errorMessage);
                sw.WriteLine("InnerExceptionMessage" + " : " + innerException);
                sw.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
