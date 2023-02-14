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
            dataGridView1.Columns[0].ReadOnly = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            




            
    
        }
    }
}
