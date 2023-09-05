﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.Reporting.WinForms;

namespace Restaurant_Software
{
    public partial class frmSalesSummaryReport : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;
        MySqlDataAdapter da;

        ClassDB db = new ClassDB();

        string _Name, _Address, _Phone, _Email;

        public string dateFrom, dateTo;

        public decimal pTotal;

        public frmSalesSummaryReport()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.getConnection();
        }

        private void frmSalesSummaryReport_Load(object sender, EventArgs e)
        {
            LoadHeader();
            LoadPayment();
            this.reportViewer1.RefreshReport();
        }

        public void LoadPayment()
        {
            cn.Open();
            da = new MySqlDataAdapter("SELECT * FROM tblpayment WHERE date BETWEEN '" + dateFrom + "' AND '" + dateTo + "' ORDER BY invoiceno ASC", cn);
            DataSet1 ds = new DataSet1();
            da.Fill(ds, "dtSalesSummary");

            ReportDataSource datasource = new ReportDataSource("DataSet1", ds.Tables[3]);

            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(datasource);
            this.reportViewer1.RefreshReport();
            cn.Close();
        }

        public void LoadHeader()
        {
            ReportDataSource rptDS = new ReportDataSource();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblstore", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                _Name = dr["name"].ToString();
                _Phone = dr["phone"].ToString();
                _Email = dr["email"].ToString();
                _Address = dr["address"].ToString();

                dr.Close();
                cn.Close();

                reportViewer1.LocalReport.ReportPath = Application.StartupPath + "/Reports/SalesSummary.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds1 = new DataSet1();
                da = new MySqlDataAdapter();


                cn.Open();
                da.SelectCommand = new MySqlCommand("SELECT * FROM tblstore WHERE name = '" + _Name + "'", cn);
                da.Fill(ds1, "dtName");
                cn.Close();

                // get the total sum by date
                cn.Open();
                cm = new MySqlCommand("SELECT SUM(total) FROM tblpayment WHERE date BETWEEN '" + dateFrom + "' AND '" + dateTo + "'", cn);
                decimal num = Decimal.Parse(cm.ExecuteScalar().ToString());
                cn.Close();

                ReportParameter pName = new ReportParameter("pName", _Name);
                ReportParameter pEmail = new ReportParameter("pEmail", _Email);
                ReportParameter pPhone = new ReportParameter("pPhone", _Phone);
                ReportParameter pAddress = new ReportParameter("pAddress", _Address);
                ReportParameter dtFrom = new ReportParameter("dtFrom", dateFrom);
                ReportParameter dtTo = new ReportParameter("dtTo", dateTo);
                ReportParameter pTotal = new ReportParameter("pTotal", num.ToString()); // for total

                reportViewer1.LocalReport.SetParameters(pName);
                reportViewer1.LocalReport.SetParameters(pEmail);
                reportViewer1.LocalReport.SetParameters(pPhone);
                reportViewer1.LocalReport.SetParameters(pAddress);
                reportViewer1.LocalReport.SetParameters(dtFrom);
                reportViewer1.LocalReport.SetParameters(dtTo);
                reportViewer1.LocalReport.SetParameters(pTotal); // for total

                rptDS = new ReportDataSource("DataSet1", ("dtName"));
                reportViewer1.LocalReport.DataSources.Add(rptDS);
            }
            else
            {
                _Name = "";
                _Phone = "";
                _Email = "";
                _Address = "";
            }
            dr.Close();
            cn.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.reportViewer1.PrintDialog();
        }
    }
}
