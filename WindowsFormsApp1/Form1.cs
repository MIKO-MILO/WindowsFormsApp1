using MySql.Data;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string cnnString = "server=127.0.0.1;database=programbarang;uid=usergatot;pwd=2108;";
        MySqlConnection myConnection;

        public Form1()
        {
            InitializeComponent();
        }

        private bool MySqlConnect()
        {
            myConnection = new MySqlConnection(cnnString);
            try
            {
                myConnection.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi Kesalahan" + ex);
                return false;
            }
        }

        private void MySqlDisconnect()
        {
            myConnection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MySqlConnect())
                MessageBox.Show("Terkoneksi dengan MySql");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (MySqlConnect())
            {
                string query = "SELECT * FROM barang_data";
                var cmd = new MySqlCommand(query, myConnection);
                var reader = cmd.ExecuteReader();

                lstResult.Items.Clear();

                while (reader.Read())
                {
                    uint barang_id = reader.GetUInt32(0);
                    string barang_nama = reader.GetString(1);
                    uint barang_berat = reader.GetUInt32(2);
                    uint barang_harga = reader.GetUInt32(3);
                    DateTime barang_tgl_masuk = reader.GetDateTime(4);
                    DateTime barang_tgl_edit = reader.GetDateTime(5);
                    lstResult.Items.Add(barang_id + " \t| " + barang_nama + " \t| " + barang_berat + " \t| " + barang_harga + " \t| " + barang_tgl_masuk + " \t| " + barang_tgl_edit + " \t| ");
                }
                MySqlDisconnect();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MySqlConnect())
                MessageBox.Show("Terkoneksi dengan MySql");
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            string getNama = textBox6.Text;
            uint getBerat = uint.Parse(textBox7.Text);
            uint getHarga = uint.Parse(textBox8.Text);
            if (MySqlConnect())
            {
                string query = string.Format("INSERT INTO barang_data(barang_nama, barang_berat, barang_harga, barang_tgl_masuk, barang_tgl_edit) VALUES ('{0}' ,'{1}' ,'{2}' ,'{3}' ,'{4}')",
                    (object)getNama,
                    (object)getBerat,
                    (object)getHarga,
                    (object)DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    (object)DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                try
                {
                    var cmd = new MySqlCommand(query, myConnection);
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

                MySqlDisconnect();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            uint getBrgID = (uint)barangID.Value;
            string getNama = textBox10.Text;
            uint getBerat = uint.Parse(textBox11.Text);
            uint getHarga = uint.Parse(textBox9.Text);
            bool barangFound = false;
            if (MySqlConnect())
            {
                string queryCheck = "SELECT id_barang FROM barang_data WHERE id_barang='" + getBrgID.ToString() + "'";
                try
                {
                    var cmdCheck = new MySqlCommand(queryCheck, myConnection);
                    var reader = cmdCheck.ExecuteReader();
                    if (reader.HasRows)
                    {
                        barangFound = true;
                    }
                    else
                    {
                        MessageBox.Show("Barang tidak ditemukan");
                    }
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

                MySqlDisconnect();
            }


            if (MySqlConnect())
            {
                string query = string.Format("UPDATE barang_data SET barang_nama='{1}', barang_berat='{2}', barang_harga='{3}', barang_tgl_edit='{4}' WHERE id_barang='{0}';",
                    (object)getBrgID,
                    (object)getNama,
                    (object)getBerat,
                    (object)getHarga,
                    (object)DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                try
                {
                    var cmd = new MySqlCommand(query, myConnection);
                    cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

                MySqlDisconnect();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            if (lstResult.SelectedItem != null)
            {
                string selectedItem = lstResult.SelectedItem.ToString();
                string[] data = selectedItem.Split('|');

                if (data.Length > 0)
                {
                    uint id = uint.Parse(data[0].Trim());
                    barangID.Value = id;
                }
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            uint id = (uint)barangID.Value;

            DialogResult result = MessageBox.Show(
                $"Yakin ingin menghapus barang dengan ID {id}?",
                "Konfirmasi Hapus",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                if (MySqlConnect())
                {
                    string query = "DELETE FROM barang_data WHERE id_barang = @id";

                    try
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query, myConnection))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                                MessageBox.Show("Data berhasil dihapus!");
                            else
                                MessageBox.Show("Data tidak ditemukan atau sudah dihapus.");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Gagal menghapus data: " + ex.Message);
                    }

                    MySqlDisconnect();
                }
            }
        }
    }
}

