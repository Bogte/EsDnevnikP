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
            dataGridView1.Columns["id"].ReadOnly = true;

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
                    dataGridView1.Columns["id"].ReadOnly = true;
                }
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Menjaj")
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da izmenite ove podatke?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string naziv;
                    int indeks;
                    indeks = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                    naziv = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["naziv"].Value);
                    menjanja = new SqlCommand();

                    if (tabela == "Predmet")
                    {
                        string razred;
                        razred = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["razred"].Value);

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
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Dodaj")
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da dodate ove podatke?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        string naziv;
                        naziv = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["naziv"].Value);
                        menjanja = new SqlCommand();

                        if (tabela == "Predmet")
                        {
                            string razred;
                            razred = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["razred"].Value);

                            menjanja.CommandText = ("INSERT INTO " + tabela + " VALUES (" + "'" + naziv + "', " + "'" + razred + "')");
                        }
                        else
                        {
                            podaci = new DataTable();
                            podaci = Konekcija.Unos("SELECT naziv FROM " + tabela + " WHERE naziv = " + "'" + naziv + "'");
                            dataGridView2.DataSource = podaci;
                            if (dataGridView2.RowCount != 1) throw new Exception("Nesto nije uredu");

                            menjanja.CommandText = ("INSERT INTO " + tabela + " VALUES (" + "'" + naziv + "')");

                            SqlConnection con = new SqlConnection(Konekcija.Veza());
                            con.Open();
                            menjanja.Connection = con;
                            menjanja.ExecuteNonQuery();
                            con.Close();
                        }

                        podaci = new DataTable();
                        podaci = Konekcija.Unos("SELECT * FROM " + tabela);
                        dataGridView1.DataSource = podaci;
                        dataGridView1.Columns["id"].ReadOnly = true;
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Ne mozete da dodate vec postojeci podatak! " + ex.Message + " - " + ex.Source);
                    }
                }
            }


        }
    }
}
