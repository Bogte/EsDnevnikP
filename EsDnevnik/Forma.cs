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

            if (tabela == "Predmet")
            {
                for (int i = 0; i < podaci.Rows.Count; i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells["ID"].Value = Convert.ToString(podaci.Rows[i]["id"]);
                    dataGridView1.Rows[i].Cells["Naziv"].Value = Convert.ToString(podaci.Rows[i]["naziv"]);
                    dataGridView1.Rows[i].Cells["Razred"].Value = Convert.ToString(podaci.Rows[i]["razred"]);
                }
                
            }
            else if (tabela == "Smer")
            {
                dataGridView1.Columns["Naziv"].Visible = false;
                dataGridView1.Columns["Razred"].Visible = false;

                for (int i = 0; i < podaci.Rows.Count; i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells["ID"].Value = Convert.ToString(podaci.Rows[i]["id"]);
                    dataGridView1.Rows[i].Cells["Smer"].Value = Convert.ToString(podaci.Rows[i]["naziv"]);
                }
            }
            else
            {
                dataGridView1.Columns["Naziv"].Visible = false;
                dataGridView1.Columns["Razred"].Visible = false;
                dataGridView1.Columns["Smer"].Visible = false;
                dataGridView1.Columns["ID"].Visible = false;
                dataGridView1.DataSource = podaci;
                dataGridView1.Columns["id"].ReadOnly = true;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Obrisi")
            {
                try
                {
                    if (MessageBox.Show("Da li ste sigurni da zelite da obrisete ove podatake?", "EsDnevnik", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int kurac = e.RowIndex;
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
                catch (Exception ex)
                {
                    MessageBox.Show("Ne mozete da obrisete ove podatake, druge tabele zahtevaju ove podatake! - " + ex.Source, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Menjaj")
            {
                try
                {
                    if (MessageBox.Show("Da li ste sigurni da zelite da izmenite ove podatke?", "EsDnevnik", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

                            podaci = new DataTable();
                            podaci = Konekcija.Unos("SELECT naziv, razred FROM " + tabela + " WHERE naziv = " + "'" + naziv + "' AND razred = " + "'" + razred + "'");
                            if (podaci.Rows.Count >= 1) throw new Exception();

                            menjanja.CommandText = ("UPDATE " + tabela + " SET naziv = " + "'" + naziv + "'" + " WHERE id = " + indeks +
                                " UPDATE " + tabela + " SET naziv = " + "'" + razred + "'" + " WHERE id = " + indeks);
                        }
                        else
                        {
                            podaci = new DataTable();
                            podaci = Konekcija.Unos("SELECT naziv FROM " + tabela + " WHERE naziv = " + "'" + naziv + "'");
                            if (podaci.Rows.Count >= 1) throw new Exception();

                            menjanja.CommandText = ("UPDATE " + tabela + " SET naziv = " + "'" + naziv + "'" + " WHERE id = " + indeks);
                        }

                        SqlConnection con = new SqlConnection(Konekcija.Veza());
                        con.Open();
                        menjanja.Connection = con;
                        menjanja.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch(Exception ex)
                {
                    podaci = new DataTable();
                    podaci = Konekcija.Unos("SELECT * FROM " + tabela);
                    dataGridView1.DataSource = podaci;
                    dataGridView1.Columns["id"].ReadOnly = true;
                    MessageBox.Show("Podatak vec postoji u tabeli - " + ex.Source, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Dodaj")
            {
                try
                {
                    if (MessageBox.Show("Da li ste sigurni da zelite da dodate ove podatke?", "EsDnevnik", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string naziv;
                        naziv = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["naziv"].Value);
                        menjanja = new SqlCommand();

                        if (tabela == "Predmet")
                        {
                            string razred;
                            razred = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["razred"].Value);

                            podaci = new DataTable();
                            podaci = Konekcija.Unos("SELECT naziv, razred FROM " + tabela + " WHERE naziv = " + "'" + naziv + "' AND razred = " + "'" + razred + "'");
                            if (podaci.Rows.Count >= 1) throw new Exception();

                            menjanja.CommandText = ("INSERT INTO " + tabela + " VALUES (" + "'" + naziv + "', " + "'" + razred + "')");
                        }
                        else
                        {
                            podaci = new DataTable();
                            podaci = Konekcija.Unos("SELECT naziv FROM " + tabela + " WHERE naziv = " + "'" + naziv + "'");
                            if (podaci.Rows.Count >= 1) throw new Exception();

                            menjanja.CommandText = ("INSERT INTO " + tabela + " VALUES (" + "'" + naziv + "')");
                        }

                        SqlConnection con = new SqlConnection(Konekcija.Veza());
                        con.Open();
                        menjanja.Connection = con;
                        menjanja.ExecuteNonQuery();
                        con.Close();

                        podaci = new DataTable();
                        podaci = Konekcija.Unos("SELECT * FROM " + tabela);
                        dataGridView1.DataSource = podaci;
                        dataGridView1.Columns["id"].ReadOnly = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ne mozete da dodate vec postojece podatke! - " + ex.Source, "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }
    }
}
