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
    public partial class frmOverAllReport : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;

        ClassDB db = new ClassDB();

        public frmOverAllReport()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.getConnection();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        void getSalesSummary()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT SUM(total) FROM tblpayment", cn);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            datagridSalesSummary.Rows.Clear();
            int i = 0;
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblpayment ORDER BY invoiceno ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                datagridSalesSummary.Rows.Add(i, dr["invoiceno"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["mchange"].ToString(), dr["paymode"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["user"].ToString(), dr["status"].ToString());
            }
            datagridSalesSummary.Rows.Add("", "", "", "", "", "", "", "", "", "", "");
            datagridSalesSummary.Rows.Add("", "", "", "", "", "", "", "", "", "", "");
            datagridSalesSummary.Rows.Add("", "TOTAL SALES", num, "", "", "", "", "", "", "", "");
            dr.Close();
            cn.Close();
        }

        private void btnSalesSummary_Click(object sender, EventArgs e)
        {
            cn.Open();
            cm = new MySqlCommand("SELECT SUM(total) FROM tblpayment WHERE date BETWEEN '" + dtFromSalesSummary.Text + "' AND '" + dtToSalesSummary.Text + "'", cn);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            datagridSalesSummary.Rows.Clear();
            int i = 0;
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblpayment WHERE date BETWEEN '" + dtFromSalesSummary.Text + "' AND '" + dtToSalesSummary.Text + "' ORDER BY id ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                datagridSalesSummary.Rows.Add(i, dr["invoiceno"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["mchange"].ToString(), dr["paymode"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["user"].ToString(), dr["status"].ToString());
            }
            datagridSalesSummary.Rows.Add("", "", "", "", "", "", "", "", "", "", "");
            datagridSalesSummary.Rows.Add("", "", "", "", "", "", "", "", "", "", "");
            datagridSalesSummary.Rows.Add("", "TOTAL SALES", num, "", "", "", "", "", "", "", "");
            dr.Close();
            cn.Close();
        }

        private void frmOverAllReport_Load(object sender, EventArgs e)
        {
            getSalesSummary();
        }

        private void btnSalesSummaryPrint_Click(object sender, EventArgs e)
        {
            frmSalesSummaryReport sales = new frmSalesSummaryReport();
            sales.dateFrom = dtFromSalesSummary.Text;
            sales.dateTo = dtToSalesSummary.Text;
            sales.ShowDialog();
        }
    }
}
