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
    public partial class frmBackUpDatabase : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;
        ClassDB db = new ClassDB();

        public frmBackUpDatabase()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.getConnection();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Back Up Database", "BACK UP DATABASE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                            mb.ExportToFile(file);
                            cn.Close();
                            MessageBox.Show("Database Backup Completed...", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }
    }
}
