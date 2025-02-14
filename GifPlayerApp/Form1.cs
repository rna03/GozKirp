using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32; // Windows başlangıcına eklemek için


namespace GifPlayerApp
{
    public partial class Form1 : Form
    {
        private Timer timerShow;
        private Timer timerHide;
        private PictureBox pictureBox;
        public Form1()
        {
            InitializeComponent();

            // Form Başlangıçta Görünmez
            this.Opacity = 0;  // Form'u tamamen şeffaf yap (başlangıçta görünmez)
            this.FormBorderStyle = FormBorderStyle.None;  // Kenarlıkları kaldır
            this.BackColor = Color.Lime;  // Arkaplan rengi (şeffaflık için kullanılacak)
            this.TransparencyKey = Color.Lime;  // Lime rengi şeffaf olacak
            this.TopMost = true;  // Her zaman en üstte tut

            // PictureBox oluştur
            pictureBox = new PictureBox
            {
                Image = Properties.Resources.blink, // Kaynaklardan eklediğin GIF
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 500, // İstediğin boyutları belirle
                Height = 500,
                BackColor = Color.Transparent
            };
            Controls.Add(pictureBox);


            // 🎯 **Ekranın tam ortasına yerleştir**
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(
                (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2,
                (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2
            );

            pictureBox.Location = new Point(0, 0);

            // 20 saniyede bir GIF'i göstermek için Timer
            timerShow = new Timer();
            timerShow.Interval = 1200000; // 20 saniye
            timerShow.Tick += timer1_Tick;
            timerShow.Start();

            // 5 saniye sonra GIF'i gizlemek için Timer
            timerHide = new Timer();
            timerHide.Interval = 5000; // 5 saniye
            timerHide.Tick += timer2_Tick;

            // Windows başlangıcına ekleme
            AddToStartup();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Opacity = 1;  // Form'u görünür yap
            pictureBox.Visible = true;  // GIF'i göster
            this.Show();
            timerHide.Start(); // 5 saniye sonra gizleme zamanlayıcısını başlat
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            pictureBox.Visible = false; // GIF'i gizle
            this.Opacity = 0;  // Form'u tekrar şeffaf yap
            this.Hide();
            timerHide.Stop(); // 5 saniyede bir tekrar etmemesi için durdur

        }
        // 📌 **Windows Başlangıcına Ekleme Fonksiyonu**
        private void AddToStartup()
        {
            try
            {
                string appName = "GifPlayerApp";
                string exePath = Application.ExecutablePath;

                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                if (key.GetValue(appName) == null)
                {
                    key.SetValue(appName, exePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Başlangıca eklenirken hata oluştu: " + ex.Message);
            }
        }
    }
}
