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
    public partial class frmSalesHistory : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;
        ClassDB db = new ClassDB();

        public frmSalesHistory()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.getConnection();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        string myTotal(string sql)
        {
            cn.Open();
            cm = new MySqlCommand(sql + " WHERE user=@user AND status='COMPLETED'", cn);
            cm.Parameters.AddWithValue("@user", frmLogin.username);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            return num;
        }

        string myTotal2(string sql)
        {
            cn.Open();
            cm = new MySqlCommand(sql + " WHERE user=@user AND date BETWEEN '" + dtFrom.Text + "' AND '" + dtTo.Text + "' ORDER BY invoiceno ASC", cn);
            cm.Parameters.AddWithValue("@user", frmLogin.username);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            return num;
        }

        void getSalesHistory()
        {
            datagridSalesHistory.Rows.Clear();
            int i = 0;
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblpayment WHERE user=@user ORDER BY invoiceno ASC", cn);
            cm.Parameters.AddWithValue("@user", frmLogin.username);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                datagridSalesHistory.Rows.Add(i, dr["invoiceno"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["mchange"].ToString(), dr["paymode"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["user"].ToString(), dr["status"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void frmSalesHistory_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            getSalesHistory();

            lblSalesTotal.Text = myTotal("SELECT SUM(total) FROM tblpayment");
        }

        private void frmSalesHistory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }

        private void datagridSalesHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = datagridSalesHistory.Columns[e.ColumnIndex].Name;
            string invoiceno = datagridSalesHistory.Rows[e.RowIndex].Cells[1].Value.ToString();

            if (ColName == "ColPrint")
            {
                if (MessageBox.Show("Print Receipt?", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    frmReceipt fpay = new frmReceipt();
                    fpay.invoiceno = invoiceno;
                    fpay.ShowDialog();
                }
            }
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            datagridSalesHistory.Rows.Clear();
            int i = 0;
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblpayment WHERE user=@user AND date BETWEEN '" + dtFrom.Text + "' AND '" + dtTo.Text + "' ORDER BY invoiceno ASC", cn);
            cm.Parameters.AddWithValue("@user", frmLogin.username);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                datagridSalesHistory.Rows.Add(i, dr["invoiceno"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["mchange"].ToString(), dr["paymode"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["user"].ToString(), dr["status"].ToString());
            }
            dr.Close();
            cn.Close();

            lblSalesTotal.Text = myTotal2("SELECT ifnull(SUM(total), 0.0) FROM tblpayment");
        }
    }
}
