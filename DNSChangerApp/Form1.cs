using Microsoft.Win32; 
using System;
using System.Diagnostics;
using System.Drawing; 
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DNSChangerApp
{
    public partial class Form1 : Form
    {

        private bool gercekCikis = false;
        private string hedefKlasor = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "MyGoodbyeDPI");
        private string exeYolu;
        private string uygulamaAdi = "GoodbyeDPI_Manager";
        private string gorevAdi = "DNSChangerApp_Startup";
        private bool formYukleniyor = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                if (!Directory.Exists(hedefKlasor)) Directory.CreateDirectory(hedefKlasor);

                DosyaCikar("goodbyedpi.exe", Path.Combine(hedefKlasor, "goodbyedpi.exe"));
                DosyaCikar("WinDivert.dll", Path.Combine(hedefKlasor, "WinDivert.dll"));
                DosyaCikar("WinDivert64.sys", Path.Combine(hedefKlasor, "WinDivert64.sys"));

                exeYolu = Path.Combine(hedefKlasor, "goodbyedpi.exe");

                BaslangicDurumunuKontrolEt();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Başlangıç hatası: " + ex.Message);
            }

            formYukleniyor = false;


        }





        private void KomutCalistir(string komut)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + komut);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.Verb = "runas";

            Process process = Process.Start(processInfo);
            process.WaitForExit();
        }

        private void DosyaCikar(string dosyaAdi, string hedefYol)
        {
            if (File.Exists(hedefYol)) return;

            var assembly = Assembly.GetExecutingAssembly();
            string[] butunKaynaklar = assembly.GetManifestResourceNames();
            string bulunanKaynak = Array.Find(butunKaynaklar, x => x.EndsWith(dosyaAdi, StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(bulunanKaynak))
                throw new Exception($"Kritik Hata: '{dosyaAdi}' bulunamadı.");

            using (Stream stream = assembly.GetManifestResourceStream(bulunanKaynak))
            using (FileStream fileStream = new FileStream(hedefYol, FileMode.Create))
            {
                stream.CopyTo(fileStream);
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("goodbyedpi");

            if (processes.Length > 0)
            {
                lblDurum.Text = "DURUM: AKTİF 🟢";
                lblDurum.ForeColor = Color.SpringGreen; 
            }
            else
            {
                lblDurum.Text = "DURUM: PASİF 🔴";
                lblDurum.ForeColor = Color.Tomato; 
            }
        }


        private void BaslangicDurumunuKontrolEt()
        {
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", $"/c schtasks /query /tn \"{gorevAdi}\"");
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;

                Process process = Process.Start(processInfo);
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    chkBaslangic.Checked = true;
                }
                else
                {
                    chkBaslangic.Checked = false;
                }
            }
            catch
            {
                chkBaslangic.Checked = false;
            }
        }

        private void chkBaslangic_CheckedChanged(object sender, EventArgs e)
        {

            if (formYukleniyor) return;

            try
            {
                string exePath = Application.ExecutablePath;

                if (chkBaslangic.Checked)
                {

                    string komut = $"schtasks /create /tn \"{gorevAdi}\" /tr \"\\\"{exePath}\\\"\" /sc onlogon /rl highest /f";
                    KomutCalistir(komut);
                }
                else
                {
                    // Görevi sil
                    string komut = $"schtasks /delete /tn \"{gorevAdi}\" /f";
                    KomutCalistir(komut);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Başlangıç ayarı değiştirilemedi: " + ex.Message);
            }
        }

        private void ServisleriTemizle()
        {
            KomutCalistir("sc stop \"GoodbyeDPI\"");
            KomutCalistir("sc delete \"GoodbyeDPI\"");
            KomutCalistir("sc stop \"WinDivert\"");
            KomutCalistir("sc delete \"WinDivert\"");

            KomutCalistir("taskkill /F /IM goodbyedpi.exe 2>nul");
        }




        private void btnAc_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(exeYolu) || !File.Exists(exeYolu)) return;

            KomutCalistir("sc stop \"GoodbyeDPI\" && sc delete \"GoodbyeDPI\"");

            string parametreler = "-5 --set-ttl 5 --dns-addr 77.88.8.8 --dns-port 1253 --dnsv6-addr 2a02:6b8::feed:0ff --dnsv6-port 1253";
            string createCommand = $"sc create \"GoodbyeDPI\" binPath= \"\\\"{exeYolu}\\\" {parametreler}\" start= \"demand\"";

            KomutCalistir(createCommand);
            KomutCalistir("sc start \"GoodbyeDPI\"");

        }

        private void btnKapa_Click(object sender, EventArgs e)
        {
            KomutCalistir("sc stop \"GoodbyeDPI\" && sc delete \"GoodbyeDPI\"");
            KomutCalistir("sc stop \"WinDivert\" && sc delete \"WinDivert\"");
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!gercekCikis && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                notifyIcon1.Visible = true;

                notifyIcon1.ShowBalloonTip(3000, "GoodbyeDPI Manager", "Uygulama arka planda çalışmaya devam ediyor.\nTamamen kapatmak için sağ tıklayıp 'Çıkış' diyebilirsin.", ToolTipIcon.Info);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void açToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAc_Click(sender, e);
        }

        private void kapaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServisleriTemizle();
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ServisleriTemizle();

            gercekCikis = true;
            Application.Exit();
        }

    }
}
