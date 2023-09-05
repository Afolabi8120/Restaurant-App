using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Restaurant_Software
{
    public partial class frmRestoreDatabase : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;
        ClassDB db = new ClassDB();

        public frmRestoreDatabase()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.getConnection();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Restore Database", "RESTORE DATABASE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string constring = "server=localhost;username=root;password=(Afolabi8120);database=restaurant;";
                string file = "C:restaurant.sql";
                using (MySqlConnection cn = new MySqlConnection(constring))
                {
                    using (MySqlCommand cm = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cm))
                        {
                            cm.Connection = cn;
                            cn.Open();
                            mb.ImportFromFile(file);
                            cn.Close();
                            MessageBox.Show("Database Restore Completed...", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }
    }
}
