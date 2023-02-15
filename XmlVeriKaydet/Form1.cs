using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace XmlVeriKaydetProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();          
        }
       
        void Yukle()
        {
            
            XmlDocument x = new XmlDocument();
            XmlReader xmlFile;
            xmlFile = XmlReader.Create(@"test.xml", new XmlReaderSettings());
            DataSet ds = new DataSet();
            ds.ReadXml(xmlFile);
            dataGridView1.DataSource = ds.Tables[0];
            xmlFile.Close();
            
        //xml dosyası okutuldu   
        }      

        private void Form1_Load(object sender, EventArgs e)
        {
            Yukle();
         
        }

        SqlConnection baglantı = new SqlConnection("Data Source=DESKTOP-66BEJ8D\\SQLEXPRESS;Initial Catalog=Kayitlar;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

         public void verilerigoster(string veriler)
        {
            SqlDataAdapter da = new SqlDataAdapter(veriler, baglantı);//verilerle bağlantıyı ilişkilendirdim
            DataSet ds  = new DataSet();//dataset oluşturdum
            da.Fill(ds);//dataseti doldurdum
            dataGridView1.DataSource = ds.Tables[0];//sıfırıncı indisten aldım
        }
        private void button2_Click(object sender, EventArgs e)
        {
            verilerigoster("Select * from Gelenler"); 
        }
        //INSERT yapıldı
        private void btnEkle_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            SqlCommand komut = new SqlCommand("insert into Gelenler (sgkkod, Tckn, Soyad, Ad, Babaadi, Tutar) values (@sgkkodu,@tcknsi,@soyadi,@adi,@babaadii,@tutari)", baglantı); //veriler sql e insert edildi

            komut.Parameters.AddWithValue("@sgkkodu", txtSgk.Text);//insert edilen veriler parametrelere aktarılıp değerler içine eklencek 
            komut.Parameters.AddWithValue("@tcknsi", txtTckn.Text);
            komut.Parameters.AddWithValue("@soyadi", txtSoyad.Text);
            komut.Parameters.AddWithValue("@adi", txtAd.Text);
            komut.Parameters.AddWithValue("@babaadii", txtBabaadi.Text);
            komut.Parameters.AddWithValue("@tutari", txtTutar.Text);
            komut.ExecuteNonQuery();//değişiklikten sonra göster
          //verilerigoster("select*from Gelenler");
            baglantı.Close();

            //xml kaydetme

            XDocument x = XDocument.Load(@"test.xml");
        x.Element("SGKIstekListe").Add(
                new XElement("SGKIstek",
                new XElement("SGKKOD", txtSgk.Text),
                new XElement("TCKN", txtTckn.Text),
                new XElement("Soyad", txtSoyad.Text),
                new XElement("Ad", txtAd.Text),
                new XElement("BabaAdi", txtBabaadi.Text),
                new XElement("Tutar", txtTutar.Text)
                ));

            x.Save(@"test.xml");
            Yukle();

            txtSgk.Clear();
            txtTckn.Clear();
            txtSoyad.Clear();
            txtAd.Clear();
            txtBabaadi.Clear();
            txtTutar.Clear();
        }

        //DELETE 
        private void btnSil_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            //girilen sgk koduna göre silinsin
            SqlCommand komut = new SqlCommand("delete from Gelenler where sgkkod=@sgkkodu", baglantı);
            komut.Parameters.AddWithValue("@sgkkodu", txtSgk.Text);
            komut.ExecuteNonQuery() ;
            //Yukle();
            baglantı.Close();

            //txtSgk.Clear();

            
            XDocument x = XDocument.Load(@"test.xml");
            x.Root.Elements().Where(a => a.Element("SGKKOD").Value == txtSgk.Text).Remove();
            x.Save(@"test.xml");
            Yukle();
            txtSgk.Clear();
          


        }
        //ara butonunda ada göre arama yapalım
        private void button1_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            SqlCommand komut = new SqlCommand("Select * from Gelenler where Ad like '%" + txtAra.Text + "%'", baglantı);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglantı.Close();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilialan = dataGridView1.SelectedCells[0].RowIndex; //SEÇİLİ HÜCREYİ TANIMLADIM
            string sgkkod = dataGridView1.Rows[secilialan].Cells[0].Value.ToString();
            string Tckn = dataGridView1.Rows[secilialan].Cells[1].Value.ToString();
            string Soyad = dataGridView1.Rows[secilialan].Cells[2].Value.ToString();
            string Ad = dataGridView1.Rows[secilialan].Cells[3].Value.ToString();
            string Babaadi = dataGridView1.Rows[secilialan].Cells[4].Value.ToString();
            string Tutar = dataGridView1.Rows[secilialan].Cells[5].Value.ToString();

            txtSgk.Text = sgkkod;
            txtTckn.Text = Tckn;
            txtSoyad.Text = Soyad;
            txtAd.Text = Ad;
            txtBabaadi.Text = Babaadi;
            txtTutar.Text = Tutar;
        }

        //UPDATE YAPILDI

        private void btnGuncelle_Click(object sender, EventArgs e)
    {
            //SQL UPDATE
           baglantı.Open();
            SqlCommand komut = new SqlCommand("update Gelenler set Tckn = '" + txtTckn.Text + "',Soyad = '" + txtSoyad.Text + "', Babaadi = '" + txtBabaadi.Text + "',Tutar = '" + txtTutar.Text + "' where Ad = '" + txtAd.Text + "'", baglantı);
            komut.ExecuteNonQuery();
            //verilerigoster("Select * from Gelenler");
             baglantı.Close();

            //XML UPDATE
            //kişinin adına göre düzenleme yaptım
            XDocument x = XDocument.Load(@"test.xml");
        XElement node = x.Element("SGKIstekListe").Elements("SGKIstek").FirstOrDefault(a => a.Element("Ad").Value.Trim() == txtAd.Text);
        if (node != null)
        {
            node.SetElementValue("SGKKOD", txtSgk.Text);
            node.SetElementValue("TCKN", txtTckn.Text);
            node.SetElementValue("Soyad", txtSoyad.Text);     
            node.SetElementValue("BabaAdi", txtBabaadi.Text);
            node.SetElementValue("Tutar", txtTutar.Text);

                x.Save(@"test.xml");

                Yukle();
                txtSgk.Clear();
                txtTckn.Clear();
                txtSoyad.Clear();
                txtAd.Clear();
                txtBabaadi.Clear();
                txtTutar.Clear();
            }
        }
      

      
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        
    }
}