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

namespace hastaneProjesi
{
    public partial class frmDoktorPaneli : Form
    {
        public frmDoktorPaneli()
        {
            InitializeComponent();
        }
        sqlBaglantisi bgl = new sqlBaglantisi();
        private void doktorGetir()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select *from doktorlar", bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void frmDoktorPaneli_Load(object sender, EventArgs e)
        {
           doktorGetir();

            //branş çekme
            SqlCommand komut2 = new SqlCommand("select BransAd from branslar", bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                cmbBrans.Items.Add(dr2[0]);
            }
            bgl.baglanti().Close();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            SqlCommand kontrolKomut = new SqlCommand("SELECT COUNT(*) FROM doktorlar WHERE DoktorTC = @p1", bgl.baglanti());
            kontrolKomut.Parameters.AddWithValue("@p1", mskTC.Text);
            int kayitSayisi = Convert.ToInt32(kontrolKomut.ExecuteScalar());

            if (kayitSayisi > 0)
            {
                MessageBox.Show("Bu TC kimlik numarasına sahip bir doktor zaten kayıtlı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Eğer TC yoksa yeni kaydı ekle
                SqlCommand komut = new SqlCommand("INSERT INTO doktorlar (DoktorAd, DoktorSoyad, DoktorBrans, DoktorTC, DoktorSifre) VALUES (@p1, @p2, @p3, @p4, @p5)",bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", txtAd.Text);
                komut.Parameters.AddWithValue("@p2", txtSoyad.Text);
                komut.Parameters.AddWithValue("@p3", cmbBrans.Text);
                komut.Parameters.AddWithValue("@p4", mskTC.Text);
                komut.Parameters.AddWithValue("@p5", txtSifre.Text);
                komut.ExecuteNonQuery();
                MessageBox.Show("Doktor Bilgisi Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                doktorGetir();
            }
            
            txtAd.Clear();
            txtSoyad.Clear();
            cmbBrans.Items.Clear();
            mskTC.Clear();
            txtSifre.Clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            txtAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            txtSoyad.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            cmbBrans.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            mskTC.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            txtSifre.Text = dataGridView1.Rows[secilen].Cells[5].Value.ToString();

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("delete from doktorlar where DoktorTC=@p1",bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", mskTC.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Doktor Bilgisi Sildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("update doktorlar set DoktorAd=@p1,DoktorSoyad=@p2,DoktorBrans=@p3, DoktorSifre=@p5 where DoktorTC=@p4", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", txtAd.Text);
            komut.Parameters.AddWithValue("@p2", txtSoyad.Text);
            komut.Parameters.AddWithValue("@p3", cmbBrans.Text);
            komut.Parameters.AddWithValue("@p4", mskTC.Text);
            komut.Parameters.AddWithValue("@p5", txtSifre.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Doktor Bilgisi Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);



        }

        private void frmDoktorPaneli_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmSekreterDetay fr = new frmSekreterDetay();
            fr.Show();
            this.Hide();
        }
    }
}
