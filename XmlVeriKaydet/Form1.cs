using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;


namespace XmlVeriKaydet
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
            DataSet ds = new DataSet();
            XmlReader xmlFile;
            xmlFile = XmlReader.Create(@"test.xml", new XmlReaderSettings());
            ds.ReadXml(xmlFile);
            dataGridView1.DataSource = ds.Tables[0];
            xmlFile.Close();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Yukle();

        }

        private void btnEkle_Click(object sender, EventArgs e)
        {

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

        private void btnGuncelle_Click(object sender, EventArgs e)
        {

        //kişinin koduna göre düzenleme yaptım
            XDocument x = XDocument.Load(@"test.xml");
            XElement node = x.Element("SGKIstekListe").Elements("SGKIstek").FirstOrDefault(a => a.Element("SGKKOD").Value.Trim() == txtSgk.Text);
            if (node != null)
            {
                node.SetElementValue("TCKN", txtTckn.Text);
                node.SetElementValue("Soyad", txtSoyad.Text);
                node.SetElementValue("Ad", txtAd.Text);
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

        private void btnSil_Click(object sender, EventArgs e)
        {
            //girilen sgk koduna göre silinsin
            XDocument x = XDocument.Load(@"test.xml");
            x.Root.Elements().Where(a => a.Element("SGKKOD").Value == txtSgk.Text).Remove();
            x.Save(@"test.xml");
            Yukle();
            txtSgk.Clear();
            txtTckn.Clear();
            txtSoyad.Clear();
            txtAd.Clear();
            txtBabaadi.Clear();
            txtTutar.Clear();

        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }
    }
}