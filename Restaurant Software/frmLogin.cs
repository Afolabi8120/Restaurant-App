using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Restaurant_Software
{
    public partial class frmLogin : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;

        ClassDB db = new ClassDB();

        bool found;
        public static string username, id, fullname, password, email, usertype;

        public frmLogin()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.getConnection();
        }

         bool getConnectionStatus(string conn)
        {
            bool result;
            MySqlConnection cnn = new MySqlConnection();

            try
            {
                cnn.Open();
                result = true;
                cnn.Close();
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
           
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == String.Empty ||  txtUsername.Text == String.Empty)
            {
                MessageBox.Show("All Fields Are Required!", "PASSWORD MATCH FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tbluser WHERE username=@username AND password =@password", cn);
                cm.Parameters.AddWithValue("@username", txtUsername.Text);
                cm.Parameters.AddWithValue("@password", txtPassword.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    id = dr["id"].ToString();
                    username = dr["username"].ToString();
                    fullname = dr["fullname"].ToString();
                    email = dr["email"].ToString();
                    password = dr["password"].ToString();
                    usertype = dr["usertype"].ToString();

                    found = true;
                }
                else
                {
                    cn.Close();
                    MessageBox.Show("Invalid Details Provide", "INVALID DATAILS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (found == true)
                {
                    if (usertype == "Administrator")
                    {
                        MessageBox.Show("Welcome, " + fullname, "LOGIN SUCCESSFUL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        frmMainmenu m1 = new frmMainmenu();
                        m1.ShowDialog();
                    }
                    else if (usertype == "Cashier")
                    {
                        MessageBox.Show("Welcome, " + fullname, "LOGIN SUCCESSFUL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        frmCashier c1 = new frmCashier();
                        c1.ShowDialog();
                    }
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
