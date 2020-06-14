using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMSAPP
{
    public partial class Report : Form
    {

        LMSDB2018Entities db = new LMSDB2018Entities();
        public Report()
        {
            InitializeComponent();
        }

        private void btnPrintbyDate_Click(object sender, EventArgs e)
        {
            try
            {
                ViewReport rv = new ViewReport();
                rv.date1 = FromdateTimePicker.Text;
                rv.date2 = TodateTimePicker.Text;
                rv.ReportType = "ShowReportbyCustomerDate";
                rv.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Report_Load(object sender, EventArgs e)
        {
            var customer = db.Customers.ToList();
            comboNameFilter.DataSource = customer;
            comboNameFilter.DisplayMember = "CustomerName";
            comboNameFilter.ValueMember = "CustomerID";
        }

        private void btnprintbyName_Click(object sender, EventArgs e)
        {
            try
            {
                ViewReport rn = new ViewReport();
                rn.custmer = comboNameFilter.Text;

                rn.ReportType = "ShowReportbyCustomerList";
                rn.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
