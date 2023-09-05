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
    public partial class frmUser : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;

        ClassDB db = new ClassDB();

        public frmUser()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.getConnection();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmUser_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }

        void LoadRecord()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tbluser", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["id"].ToString(), dr["username"].ToString(), dr["fullname"].ToString(), dr["email"].ToString(), dr["usertype"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        void Clear()
        {
            txtUsername.Clear();
            txtEmail.Clear();
            txtFullname.Clear();
            txtPassword.Clear();
            txtCPassword.Clear();
            txtUsername.Focus();
            cboUsertype.ResetText();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == String.Empty || txtFullname.Text == String.Empty || txtEmail.Text == String.Empty || txtPassword.Text == String.Empty || txtCPassword.Text == String.Empty)
            {
                MessageBox.Show("All input field are required!", "FILL ALL FIELDS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txtPassword.Text != txtCPassword.Text)
            {
                MessageBox.Show("Both Password Provided DO Not Match!", "PASSWORD MATCH FAILED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (MessageBox.Show("Create User Account?", "SAVE ACCOUNT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new MySqlCommand("SELECT username FROM tbluser WHERE username=@username ", cn);
                    cm.Parameters.AddWithValue("@username", txtUsername.Text);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        cn.Close();
                        MessageBox.Show("Username Already In Use", "USERNAME IN USE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        cn.Close();
                        cn.Open();
                        cm = new MySqlCommand("INSERT INTO tbluser (username,fullname,email,usertype,password) VALUES(@username,@fullname,@email,@usertype,@password) ", cn);
                        cm.Parameters.AddWithValue("@username", txtUsername.Text);
                        cm.Parameters.AddWithValue("@fullname", txtFullname.Text);
                        cm.Parameters.AddWithValue("@email", txtEmail.Text);
                        cm.Parameters.AddWithValue("@usertype", cboUsertype.Text);
                        cm.Parameters.AddWithValue("@password", txtPassword.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("User Account Created Successfully", "Account Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Clear();
                        LoadRecord();
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            string id = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

            if (ColName == "ColRemove")
            {
                if (MessageBox.Show("Remove this User?", "REMOVE USER", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new MySqlCommand("DELETE FROM tbluser WHERE id=@id", cn);
                    cm.Parameters.AddWithValue("@id", id);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("User Removed Successfully", "USER REMOVED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecord();
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tbluser WHERE username LIKE '" + txtSearch.Text + "%' OR fullname LIKE '" + txtSearch.Text + "%' OR email LIKE '" + txtSearch.Text + "%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["id"].ToString(), dr["username"].ToString(), dr["fullname"].ToString(), dr["email"].ToString(), dr["usertype"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void txtEmailaddress_Validating(object sender, CancelEventArgs e)
        {
            System.Text.RegularExpressions.Regex rEmail = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");

            if (txtEmail.Text.Length > 0 && txtEmail.Text.Trim().Length != 0)
            {
                if (!rEmail.IsMatch(txtEmail.Text.Trim()))
                {
                    MessageBox.Show("Invalid Email Address", "EMAIL NOT VALID", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }

        }

    }
}
