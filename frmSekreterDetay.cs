using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace hastaneProjesi
{
    public partial class frmSekreterDetay : Form
    {
        public frmSekreterDetay()
        {
            InitializeComponent();
        }
        sqlBaglantisi bgl = new sqlBaglantisi();    
        public string tck;
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmDoktorPaneli fr = new frmDoktorPaneli();
            fr.Show();
            this.Hide();
        }

        private void frmSekreterDetay_Load(object sender, EventArgs e)
        {
            LblTC.Text = tck;
            SqlCommand komut = new SqlCommand("select SekreterAdSoyad from sekreterler where SekreterTC=@p1",bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", LblTC.Text);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                LblAdSoyad.Text = dr[0].ToString();
            }
            bgl.baglanti().Close();

            //branşları çekme
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select *from branslar",bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            //Doktorları çekme
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter("select DoktorAd, DoktorSoyad, DoktorBrans from doktorlar",bgl.baglanti());
            da2.Fill(dt2);
            dataGridView2.DataSource = dt2;

            //branşları combobox a aktarma
            SqlCommand komut2 = new SqlCommand("select BransAd from branslar",bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                cmbBrans.Items.Add(dr2[0]);
            }
            bgl.baglanti().Close();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void btnRandevuOlustur_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("insert into randevular (RandevuTarih, RandevuSaat, RandevuBrans,RandevuDoktor) " +
                "values (@p1,@p2,@p3,@p4)",bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", mskTarih.Text);
            komut.Parameters.AddWithValue("@p2", mskSaat.Text);
            komut.Parameters.AddWithValue("@p3", cmbBrans.Text);
            komut.Parameters.AddWithValue("@p4", cmbDoktor.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Randevu Oluşturuldu", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information );
            mskTarih.Clear();
            mskSaat.Clear();
            
        }

        private void cmbBrans_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDoktor.Items.Clear();
            SqlCommand komut = new SqlCommand("select DoktorAd, DoktorSoyad from doktorlar where DoktorBrans=@p1",bgl.baglanti());
            komut.Parameters.AddWithValue("@p1",cmbBrans.Text);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                cmbDoktor.Items.Add(dr[0] + " " + dr[1]);
            }
            bgl.baglanti().Close();
        }

        private void btnDuyuru_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(rchDuyuru.Text))
            {
                SqlCommand komut = new SqlCommand("insert into duyurular (Duyuru) values (@p1)", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", rchDuyuru.Text);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Duyuru Oluşturuldu", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rchDuyuru.Clear();
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir duyuru metni girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void btnBrans_Click(object sender, EventArgs e)
        {
            frmBrans fr = new frmBrans();
            fr.Show();
            this.Hide();
        }

        private void btnRandevuListe_Click(object sender, EventArgs e)
        {
            frmRandevuListesi fr = new frmRandevuListesi();
            fr.Show();
            
        }

        private void btnDuyuruListe_Click(object sender, EventArgs e)
        {
            frmDuyurular frmDuyurular = new frmDuyurular();
            frmDuyurular.Show();
            
        }

        private void frmSekreterDetay_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void chkDurum_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
