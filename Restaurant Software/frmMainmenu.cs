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
using System.Windows.Forms.DataVisualization.Charting;

namespace Restaurant_Software
{
    public partial class frmMainmenu : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;

        ClassDB db = new ClassDB();

        public frmMainmenu()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.getConnection();
        }

        void getStockOnHand()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT SUM(quantity) FROM tblproduct ", cn);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            lblStockOnHand.Text = num;
        }

        void getProductCount()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT COUNT(*) FROM tblproduct ", cn);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            lblProduct.Text = num;
        }

        void getCategoryCount()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT COUNT(*) FROM tblcategory ", cn);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            lblCategory.Text = num;
        }

        void getUserCount()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT COUNT(*) FROM tbluser ", cn);
            string num = cm.ExecuteScalar().ToString();
            cn.Close();

            lblUser.Text = num;
        }

        void getShopName()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblstore", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                lblRestaurantName.Text = dr["name"].ToString();
            }
            else
            {
                lblRestaurantName.Text = "Shop/Store Name";
            }
            dr.Close();
            cn.Close();
        }

        void getUserDetails()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tbluser WHERE id =@id", cn);
            cm.Parameters.AddWithValue("@id", frmLogin.id);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                lblName.Text = dr["fullname"].ToString();
                lblUsertype.Text = dr["usertype"].ToString();
            }
            dr.Close();
            cn.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToLongDateString() + "\n " + DateTime.Now.ToLongTimeString();

            getCategoryCount();
            getProductCount();
            getUserCount();
            getStockOnHand();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "LOGGING OUT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                frmLogin f1 = new frmLogin();
                f1.ShowDialog();
            }
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            frmCategory c1 = new frmCategory();
            c1.ShowDialog();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            frmProduct p1 = new frmProduct();
            p1.ShowDialog();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            frmUser u1 = new frmUser();
            u1.ShowDialog();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            frmSettings s1 = new frmSettings();
            s1.ShowDialog();
        }

        private void frmMainmenu_Load(object sender, EventArgs e)
        {
            getShopName();
            getUserDetails();
            loadTopSellingChart();
            loadWeeklyChart();

            decimal dailySales = getTotalSale("SELECT ifnull(SUM(total), 0) FROM tblpayment WHERE date = '" + DateTime.Now.ToShortDateString() + "'");
            decimal alltimeSales = getTotalSale("SELECT ifnull(SUM(total), 0) FROM tblpayment ");

            lblDailySales.Text = dailySales.ToString();
            lblAllTimeSales.Text = alltimeSales.ToString();
        }

        public void loadTopSellingChart()
        {
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT code, pname, SUM(qty) AS qty FROM tblcart GROUP BY code, pname LIMIT 5", cn);
            DataSet ds = new DataSet();

            da.Fill(ds, "Chart");
            chart2.DataSource = ds.Tables["Chart"];
            Series series1 = chart2.Series[0];
            series1.ChartType = SeriesChartType.Pie;

            series1.Name = "TOP 5 SELLING ITEMS";

            var chart = chart2;
            chart.Series[series1.Name].XValueMember = "pname";
            chart.Series[series1.Name].YValueMembers = "qty";

            chart.Series[0].IsValueShownAsLabel = true;
            chart.Series[0].LegendText = "#VALX (#PERCENT)";
        }

        public void loadWeeklyChart()
        {
            MySqlDataAdapter da = new MySqlDataAdapter("SELECT paymode,SUM(total) AS total, date FROM tblpayment GROUP BY paymode, total, date ORDER BY date DESC LIMIT 7", cn);
            DataSet ds = new DataSet();

            da.Fill(ds, "Chart");
            chart1.DataSource = ds.Tables["Chart"];
            Series series1 = chart1.Series[0];
            series1.ChartType = SeriesChartType.Pie;

            series1.Name = "WEEKLY SALES";

            var chart = chart1;
            chart.Series[series1.Name].XValueMember = "total";
            chart.Series[series1.Name].YValueMembers = "paymode";

            chart.Series[0].IsValueShownAsLabel = true;
            chart.Series[0].LegendText = "#VALX (#PERCENT)";
        }

        decimal getTotalSale(string sql)
        {
            cn.Open();
            cm = new MySqlCommand(sql, cn);
            decimal num = decimal.Parse(cm.ExecuteScalar().ToString());
            cn.Close();

            return num;
        }
    }
}
