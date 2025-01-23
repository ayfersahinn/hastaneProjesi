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
    public partial class HastaDetay : Form
    {
        public HastaDetay()
        {
            InitializeComponent();
        }
        sqlBaglantisi bgl = new sqlBaglantisi();
        public string tc;

        private void randevuGetir()
        {
            //datagridview randevu geçmişi bunun için bağlantı açıp kapamaya gerek yok
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select *from randevular where HastaTC=" + tc, bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void HastaDetay_Load(object sender, EventArgs e)
        {
            //ad soyad çekme
            LblTC.Text = tc;
            SqlCommand komut = new SqlCommand("select HastaAd, HastaSoyad from hastalar where HastaTC = @p1",bgl.baglanti());
            komut.Parameters.AddWithValue("@p1",LblTC.Text);   
            SqlDataReader rd = komut.ExecuteReader();
            while (rd.Read()) {
                LblAdSoyad.Text = rd[0] + " " + rd[1];
            }
            bgl.baglanti().Close();

            randevuGetir();

            //branş çekme
            SqlCommand komut2 = new SqlCommand("select BransAd from branslar ",bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                cmbBrans.Items.Add(dr2[0]);
            }
            bgl.baglanti().Close();
        }

        private void cmbBrans_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDoktor.Items.Clear();
            SqlCommand komut = new SqlCommand("select DoktorAd,DoktorSoyad from doktorlar where DoktorBrans=@p1",bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", cmbBrans.Text);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                cmbDoktor.Items.Add(dr[0] + " " + dr[1]);
            }
            bgl.baglanti().Close();
        }

        private void cmbDoktor_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select  *from randevular where RandevuBrans='"+cmbBrans.Text+"'"+" and RandevuDoktor='"+cmbDoktor.Text+"' and RandevuDurum=0",bgl.baglanti());
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void lnkBilgiDuzenle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmBilgiDuzenle fr = new frmBilgiDuzenle();
            fr.TcNo = LblTC.Text;
            fr.Show();
            this.Hide();
        }

        private void btnRandevuAl_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richSikayet.Text))
            {
                SqlCommand komut = new SqlCommand("update randevular set RandevuDurum=1, HastaTC=@p1, HastaSikayet=@p2 where Randevuid=@p3", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", LblTC.Text);
                komut.Parameters.AddWithValue("@p2", richSikayet.Text);
                komut.Parameters.AddWithValue("@p3", txtID.Text);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Randevu Oluşturuldu", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                richSikayet.Clear();
                randevuGetir();
            }
            else
            {
                MessageBox.Show("Lütfen şikayetinizi yazınız!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView2.SelectedCells[0].RowIndex;
            txtID.Text = dataGridView2.Rows[secilen].Cells[0].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void HastaDetay_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
