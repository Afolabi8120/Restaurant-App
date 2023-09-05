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
    public partial class frmSystemLock : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;

        ClassDB db = new ClassDB();

        public frmSystemLock()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.getConnection();
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == String.Empty)
            {
                MessageBox.Show("Please Input Yor Password!", "PASSWORD MATCH FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if(txtPassword.Text.Trim() == frmLogin.password)
            {
                this.Dispose();
            }else if (txtPassword.Text.Trim() != frmLogin.password)
            {
                MessageBox.Show("Invalid Password Provided!", "PASSWORD MATCH FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
