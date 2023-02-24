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
using System.Configuration;

namespace EsDnevnik
{
    public partial class Skolska_Godina : Form
    {
        DataTable podaci;
        SqlCommand menjanja;


        public Skolska_Godina()
        {
            InitializeComponent();
        }

        private void Skolska_Godina_Load(object sender, EventArgs e)
        {
            podaci = new DataTable();
            podaci = Konekcija.Unos("SELECT * FROM Skolska_godina");
            dataGridView1.DataSource = podaci;
            dataGridView1.Columns[1].ReadOnly = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Obrisi")
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da obrisete ovaj podatak?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int indeks;
                    indeks = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                    menjanja = new SqlCommand();
                    menjanja.CommandText = ("DELETE FROM Skolska_godina WHERE id = " + indeks);
                    SqlConnection con = new SqlConnection(Konekcija.Veza());
                    con.Open();
                    menjanja.Connection = con;
                    menjanja.ExecuteNonQuery();
                    con.Close();

                    dataGridView1.Rows.RemoveAt(e.ColumnIndex);
                    podaci = new DataTable();
                    podaci = Konekcija.Unos("SELECT * FROM Skolska_godina");
                    dataGridView1.DataSource = podaci;
                    dataGridView1.Columns[1].ReadOnly = true;
                }
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Menjaj")
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da izmenite podatke?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string naziv;
                    int indeks;
                    indeks = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                    naziv = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["naziv"].Value);
                    menjanja = new SqlCommand();
                    menjanja.CommandText = ("UPDATE Skolska_godina SET naziv = " + "'" + naziv + "'" + "WHERE id = " + indeks);
                    SqlConnection con = new SqlConnection(Konekcija.Veza());
                    con.Open();
                    menjanja.Connection = con;
                    menjanja.ExecuteNonQuery();
                    con.Close();
                }
            }


        }
    }
}
