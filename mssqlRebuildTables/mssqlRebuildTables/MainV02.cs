using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mssqlRebuildTables
{
    public partial class MainV02 : Form
    {
        public MainV02()
        {
            InitializeComponent();
        }

        methods opt_m = new methods();
        string log_satirArasi = "---------------------------------------------------------------------";

        #region Method lar -------------------------------------------------------

        private void FirstThigs()
        {
            model_info e_inf = new methods().Info_Get_File();

            if (e_inf.dbServerName != "HATA")
            {
                txt_dbServer.Text = e_inf.dbServerName;
                txt_dbName.Text = e_inf.dbName;
                txt_Username.Text = e_inf.dbUserName;
                txt_userpass.Text = e_inf.dbUserPass;
                txt_tryCount.Text = e_inf.tryCount.ToString();
                txt_waitSecond.Text = e_inf.waitTime.ToString();
                txt_timeHour.Text = e_inf.timeHour.ToString();
                txt_timeMinute.Text = e_inf.timeMinute.ToString();

            }
            else
            {
                txt_dbServer.Text = string.Empty;
                txt_dbName.Text = string.Empty;
                txt_Username.Text = string.Empty;
                txt_userpass.Text = string.Empty;
                txt_tryCount.Text = "0";
                txt_waitSecond.Text = "0";
                txt_timeHour.Text = "0";
                txt_timeMinute.Text = "0";

            }
        }

        private void Save()
        {
            int _trycount = 0;
            int.TryParse(txt_tryCount.Text, out _trycount);

            int _wait = 0;
            int.TryParse(txt_waitSecond.Text, out _wait);

            int _tHour = 0;
            int.TryParse(txt_timeHour.Text, out _tHour);

            int _tMin = 0;
            int.TryParse(txt_timeMinute.Text, out _tMin);


            model_info e_inf = new model_info()
            {
                dbName = txt_dbName.Text,
                dbServerName = txt_dbServer.Text,
                dbUserName = txt_Username.Text,
                dbUserPass = txt_userpass.Text,
                timeHour = _tHour,
                timeMinute = _tMin,
                tryCount = _trycount,
                waitTime = _wait

            };

            new methods().Info_Save_File(e_inf);
        }

        private void RebuildAll()
        {
            Server srv = new Server();

            string log_tmp = string.Empty;

            int _trycount = 0;
            int.TryParse(txt_tryCount.Text, out _trycount);

            int _waitsec = 0;
            int.TryParse(txt_waitSecond.Text, out _waitsec);

            srv.ConnectionContext.LoginSecure = false;
            srv.ConnectionContext.Login = txt_Username.Text;
            srv.ConnectionContext.Password = txt_userpass.Text;
            srv.ConnectionContext.ServerInstance = txt_dbServer.Text;
            string dbName = txt_dbName.Text;

            Database db = new Database();

            db = srv.Databases[dbName];

            if (db.Name != dbName)
            {

                log_tmp = Environment.NewLine + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":DB isimleri eşleşmediğinden işlem iptal edildi";
                log_tmp += Environment.NewLine + log_satirArasi + Environment.NewLine;
                txt_Log.Text += log_tmp;
                opt_m.Log_Save_File("HATA", "HATA", log_tmp);
            }


            foreach (Table tbl in db.Tables)
            {

                log_tmp = string.Format("Tablo adi : {0}, Başladığı Zaman : {1}", tbl.Name.ToString(), DateTime.Now.ToString()) + Environment.NewLine;
                log_tmp += log_satirArasi + Environment.NewLine;
                opt_m.Log_Save_File("INFO", "INFO", log_tmp);
                txt_Log.Text += log_tmp;
                Application.DoEvents();

                for (int i = 0; i < tbl.Indexes.Count; i++)
                {
                    int denemesayisi = 0;
                bas:

                    //log += string.Format("_{0}_Tablonun_{1}_indexi_{2}_baslangicTarihi_{3}", tbl.Name.ToString(), i.ToString(), tbl.Indexes[i].Name.ToString(), DateTime.Now.ToString()) + Environment.NewLine;
                    log_tmp = string.Format("Tablo adi : {2},  Index Adi : {0}, Başlangıç Zaman : {1}", tbl.Indexes[i].ToString(), DateTime.Now.ToString(), tbl.Name.ToString()) + Environment.NewLine;
                    log_tmp += log_satirArasi + Environment.NewLine;
                    opt_m.Log_Save_File("INFO", "TABLES", log_tmp);
                    txt_Log.Text += log_tmp;
                    Application.DoEvents();

                    //log += string.Format("Denemesayisi_{0}", denemesayisi) + Environment.NewLine;
                    log_tmp= string.Format("Denemesayisi_{0}", denemesayisi) + Environment.NewLine;
                    log_tmp += log_satirArasi + Environment.NewLine;
                    opt_m.Log_Save_File("INFO", "DENEME", log_tmp);
                    txt_Log.Text += log_tmp;

                    Application.DoEvents();


                    if (denemesayisi <= _trycount)
                    {
                        try
                        {
                            tbl.Indexes[i].Rebuild();
                        }
                        catch (Exception ex)
                        {
                            //log += string.Format("Hata_{0}", ex.ToString()) + Environment.NewLine;
                            log_tmp = string.Format("Hata_{0}", ex.ToString()) + Environment.NewLine;
                            log_tmp += log_satirArasi + Environment.NewLine;
                            opt_m.Log_Save_File("HATA", "TABLES", log_tmp);

                            Application.DoEvents();
                            denemesayisi++;
                            Application.DoEvents();
                            Thread.Sleep(1000 * 30);
                            goto bas;
                        }

                        log_tmp = string.Format("Tablo adi : {2}, Index Adi : {0}, Bitiş Zaman : {1}", tbl.Indexes[i].ToString(), DateTime.Now.ToString(), tbl.Name.ToString()) + Environment.NewLine;
                        log_tmp += log_satirArasi + Environment.NewLine;
                        log_tmp += "-------OK--------TABLE--------INDEX" + Environment.NewLine;
                        txt_Log.Text += log_tmp;
                        opt_m.Log_Save_File("INFO", "OK", log_tmp);

                        Application.DoEvents();
                        Application.DoEvents();
                        Thread.Sleep(1000 * _waitsec);
                        Application.DoEvents();
                    }
                    else //2 kere deneyipte olmayan işlme 
                    {
                        log_tmp += string.Format("Yapılamayan Index Adi : {0}, Bitiş Zaman : {1}", tbl.Indexes[i].ToString(), DateTime.Now.ToString()) + Environment.NewLine;
                        log_tmp += log_satirArasi + Environment.NewLine;
                        log_tmp += "-----HATA------" + Environment.NewLine;
                        txt_Log.Text += log_tmp;
                        opt_m.Log_Save_File("INFO", "HATA", log_tmp);
                    }

                }
                //sw.WriteLine(log);
                //sw.Flush();
                ////Veriyi tampon bölgeden dosyaya aktardık.
                //sw.Close();
                //fs.Close();

                //txtLog.Text += log + Environment.NewLine;

                log_tmp = string.Format("Tablo adi : {0}, Bitiş Zaman : {1}", tbl.Name.ToString(), DateTime.Now.ToString()) + Environment.NewLine;
                log_tmp += log_satirArasi + Environment.NewLine;
                log_tmp += "-------OK--------TABLE" + Environment.NewLine;
                log_tmp += Environment.NewLine;
                log_tmp += Environment.NewLine;
                log_tmp += Environment.NewLine;

                txt_Log.Text += log_tmp;
                opt_m.Log_Save_File("INFO", "OK", log_tmp);
                Application.DoEvents();

            }

        }

        private void Timermethod()
        {
            int saat = 0;
            int.TryParse(txt_timeHour.Text, out saat);

            int dakika = 0;
            int.TryParse(txt_timeMinute.Text, out dakika);

            if ((DateTime.Now.Hour == saat) && (DateTime.Now.Minute == dakika))
            {
                timer1.Enabled = false;
                txt_Status.Text = "RebuildBaşladı";
                txt_Status.BackColor = Color.Yellow;

                RebuildAll();

                timer1.Enabled = true;
                txt_Status.Text = "İşlemde";
                txt_Status.BackColor = Color.Green;
            }


        }

        private void BaslatDurdur()
        {
            if (timer1.Enabled == false)
            {
                //Saatkontrol
                int saat = 0;
                int.TryParse(txt_timeHour.Text, out saat);
                if (saat == 0)
                {
                    MessageBox.Show("Saat bilgisi hatalı işlem yapılamadı");
                    return;
                }

                int dakika = 0;
                int.TryParse(txt_timeMinute.Text, out dakika);

                if (dakika == 0)
                {
                    MessageBox.Show("Dakika bilgisi hatali işlem yapılamadı");
                    return;
                }

                txt_timeMinute.ReadOnly = true;
                txt_timeHour.ReadOnly = true;
                txt_Status.Text = "İşlemde";
                txt_Status.BackColor = Color.Green;
                timer1.Enabled = true;
            }
            else
            {
                txt_timeMinute.ReadOnly = false;
                txt_timeHour.ReadOnly = false;
                txt_Status.Text = "Durdu";
                txt_Status.BackColor = Color.Red;
                timer1.Enabled = false;
            }

        }

        #endregion #region Method lar-------------------------------------------------------

        #region Event Ler -------------------------------------------------------



        private void MainV02_Load(object sender, EventArgs e)
        {
            FirstThigs();
        }
        private void btn_Kaydet_Click(object sender, EventArgs e)
        {
            Save();

        }

        private void btn_Test_Click(object sender, EventArgs e)
        {
            if (new methods().Connect_test(dbsrv: txt_dbServer.Text, dbname: txt_dbName.Text, user: txt_Username.Text, pass: txt_userpass.Text) == true)
            {
                MessageBox.Show("Bağlantı Sağlandı");
                Save();

            }
            else
                MessageBox.Show("HATA, Bağlantı sağlanamadı");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Timermethod();
        }
        private void btn_Basla_Click(object sender, EventArgs e)
        {
            BaslatDurdur();
        }

        #endregion #region Event Ler -------------------------------------------------------


    }
}
