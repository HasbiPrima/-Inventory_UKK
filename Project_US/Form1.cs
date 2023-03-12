using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Project_US
{
    public partial class Form1 : Form
    {
        MySqlConnection koneksi = Conection.getConnection();
        DataTable dataTable = new DataTable();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            filldataTable();
        }

        public DataTable getDataSiswa()
        {
            dataTable.Reset();
            dataTable = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM data_sepatu", koneksi))
            {
                koneksi.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                dataTable.Load(reader);
            }
            return dataTable;

        }

        // mengurutkan id
        public void resetIncrement()
        {
            MySqlScript script = new MySqlScript(koneksi, "SET @id :=0; Update data_sepatu SET id = @id := (@id+1); " + "ALTER TABLE data_sepatu AUTO_INCREMENT = 1;");
            script.Execute();
        }

        // fungsi mencari data
        public void searchData(string ValueToFind)
        {
            string searchQuery = "SELECT * FROM data_sepatu WHERE CONCAT (id, nama, harga, stock, ukuran) LIKE '%" + ValueToFind + "%'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(searchQuery, koneksi);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }

        public void clear()
        {
            id_barang.Clear();
            nama.Clear();
            harga.Clear();
            stock.Clear();
            ukuran.Clear();
        }

        public void filldataTable()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.RowTemplate.Height = 150;
            dataGridView1.DataSource = getDataSiswa();

            Column1.DataPropertyName = "id";
            Column2.DataPropertyName = "nama";
            Column3.DataPropertyName = "harga";
            Column4.DataPropertyName = "stock";
            Column5.DataPropertyName = "ukuran";
            Column6.DataPropertyName = "gambar";


        }


        private void button1_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd;

            try
            {
                resetIncrement();
                // Convert image to byte array
                byte[] imageData;
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    imageData = ms.ToArray();
                }
                cmd = koneksi.CreateCommand();
                cmd.CommandText = "insert into data_sepatu(nama, harga, stock, ukuran, gambar) value(@nama, @harga, @stock, @ukuran, @gambar)";
                cmd.Parameters.AddWithValue("@nama", nama.Text);
                cmd.Parameters.AddWithValue("@harga", harga.Text);
                cmd.Parameters.AddWithValue("@stock", stock.Text);
                cmd.Parameters.AddWithValue("@ukuran", ukuran.Text);
                cmd.Parameters.AddWithValue("@gambar", imageData);
                cmd.ExecuteNonQuery();
                koneksi.Close();
                dataTable.Clear();
                clear();
                filldataTable();
            }catch (Exception ex) { }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            searchData(textBox5.Text);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd;

            try
            {
                cmd = koneksi.CreateCommand();
                cmd.CommandText = "update data_sepatu set nama=@nama, harga=@harga, stock=@stock, ukuran=@ukuran where id = @id";
                cmd.Parameters.AddWithValue("@id", id_barang.Text);
                cmd.Parameters.AddWithValue("@nama", nama.Text);
                cmd.Parameters.AddWithValue("@harga", harga.Text);
                cmd.Parameters.AddWithValue("@stock", stock.Text);
                cmd.Parameters.AddWithValue("@ukuran", ukuran.Text);

                cmd.ExecuteNonQuery();
                koneksi.Close();
                dataTable.Clear();
                clear();
                filldataTable();
            }
            catch (Exception ex) { }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd;

            try
            {
                cmd = koneksi.CreateCommand();
                cmd.CommandText = "DELETE FROM data_sepatu where id = @id";
                cmd.Parameters.AddWithValue("@id", id_barang.Text);


                cmd.ExecuteNonQuery();
                koneksi.Close();
                resetIncrement();
                dataTable.Clear();
                clear();
                filldataTable();
            }
            catch (MySqlException ex) {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.CurrentCell.RowIndex.ToString());
            id_barang.Text = dataGridView1.Rows[id].Cells[0].Value.ToString();
            nama.Text = dataGridView1.Rows[id].Cells[1].Value.ToString();
            harga.Text = dataGridView1.Rows[id].Cells[2].Value.ToString();
            stock.Text = dataGridView1.Rows[id].Cells[3].Value.ToString();
            ukuran.Text = dataGridView1.Rows[id].Cells[4].Value.ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfd = new OpenFileDialog();
            if (openfd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openfd.FileName);
            }
        }
    }
}
