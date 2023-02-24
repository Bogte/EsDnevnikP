using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EsDnevnik
{
    public partial class Forma : Form
    {
        DataTable podaci;
        SqlCommand menjanja;
        string tabela;

        public Forma(string ime)
        {
            tabela = ime;            

            InitializeComponent();
        }

        private void Skolska_Godina_Load(object sender, EventArgs e)
        {
            podaci = new DataTable();
            podaci = Konekcija.Unos("SELECT * FROM " + tabela);
            dataGridView1.DataSource = podaci;
            dataGridView1.Columns[1].ReadOnly = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Obrisi")
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da obrisete ove podatak?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int indeks;
                    indeks = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                    menjanja = new SqlCommand();
                    menjanja.CommandText = ("DELETE FROM " + tabela + " WHERE id = " + indeks);
                    SqlConnection con = new SqlConnection(Konekcija.Veza());
                    con.Open();
                    menjanja.Connection = con;
                    menjanja.ExecuteNonQuery();
                    con.Close();

                    dataGridView1.Rows.RemoveAt(e.ColumnIndex);
                    podaci = new DataTable();
                    podaci = Konekcija.Unos("SELECT * FROM " + tabela);
                    dataGridView1.DataSource = podaci;
                    dataGridView1.Columns[1].ReadOnly = true;
                }
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Menjaj")
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da izmenite ove podatke?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string naziv;
                    int indeks;
                    string razred;
                    razred = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["razred"].Value);
                    indeks = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                    naziv = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["naziv"].Value);
                    menjanja = new SqlCommand();

                    if (tabela == "Predmet")
                    {
                        menjanja.CommandText = ("UPDATE " + tabela + " SET naziv = " + "'" + naziv + "'" + " WHERE id = " + indeks +
                            " UPDATE " + tabela + " SET naziv = " + "'" + razred + "'" + " WHERE id = " + indeks);
                    }
                    else
                    {
                        menjanja.CommandText = ("UPDATE " + tabela + " SET naziv = " + "'" + naziv + "'" + " WHERE id = " + indeks);
                    }
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
