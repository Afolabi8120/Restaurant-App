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
    public partial class frmStoreSettings : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;
        ClassDB db = new ClassDB();

        public frmStoreSettings()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.getConnection();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void getStoreDetails()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblstore", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                txtName.Text = dr["name"].ToString();
                txtPhone.Text = dr["phone"].ToString();
                txtEmail.Text = dr["email"].ToString();
                txtAddress.Text = dr["address"].ToString();
            }
            else
            {
                txtName.Text = "";
                txtPhone.Text = "";
                txtEmail.Text = "";
                txtAddress.Text = "";
            }
            dr.Close();
            cn.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == String.Empty || txtEmail.Text == String.Empty || txtPhone.Text == String.Empty || txtPhone.Text == String.Empty)
            {
                MessageBox.Show("All input field are required!", "FILL ALL FIELDS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (MessageBox.Show("Add Store Information?", "STORE INFORMATION", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new MySqlCommand("DELETE FROM tblstore ", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("INSERT INTO tblstore (name,phone,email,address) VALUES(@name,@phone,@email,@address) ", cn);
                    cm.Parameters.AddWithValue("@name", txtName.Text);
                    cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cm.Parameters.AddWithValue("@email", txtEmail.Text);
                    cm.Parameters.AddWithValue("@address", txtAddress.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Store details has been saved successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    getStoreDetails();
                }
            }
        }

        private void frmStoreSettings_Load(object sender, EventArgs e)
        {
            getStoreDetails();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete store information?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cn.Open();
                cm = new MySqlCommand("DELETE FROM tblstore ", cn);
                cn.Close();
                MessageBox.Show("Store details has been deleted successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                getStoreDetails();
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
