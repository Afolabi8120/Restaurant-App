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
    public partial class frmChangePassword : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;
        ClassDB db = new ClassDB();

        public frmChangePassword()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.getConnection();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtCPassword.Text == String.Empty || txtNewPassword.Text == String.Empty || txtOldPassword.Text == String.Empty)
            {
                MessageBox.Show("All fields are required!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (txtOldPassword.Text != frmLogin.password) // check if old password match with the one provided
            {
                MessageBox.Show("Old password is invalid", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (txtNewPassword.Text != txtCPassword.Text) // check if new password match
            {
                MessageBox.Show("Both new password do not match", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("Update Password? Click Yes to continue", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cn.Open();
                cm = new MySqlCommand("UPDATE tbluser SET password=@password WHERE username=@username ", cn);
                cm.Parameters.AddWithValue("@username", frmLogin.username);
                cm.Parameters.AddWithValue("@password", txtNewPassword.Text);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Password has been changed successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCPassword.Clear();
                txtNewPassword.Clear();
                txtOldPassword.Clear();
            }
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        private void frmChangePassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
