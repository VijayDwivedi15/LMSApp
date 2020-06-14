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
    public partial class ViewReport : Form
    {
        DataSet ds;
        DataHelperReport dhr;
        CustomerReport CR;
        CustomerList CL;

        public string ReportType, custmername, custmer, date1, date2;
        public long timetableid;
        public ViewReport()
        {
            InitializeComponent();
        }

        private void ViewReport_Load(object sender, EventArgs e)
        {

            ds = new DataSet();
            dhr = new DataHelperReport();
           

            if (ReportType == "ShowReportbyCustomerName")
            {
                try
                {
                    CR = new CustomerReport();
                    ds = dhr.SelectName(custmername, timetableid);
                    //ds.WriteXml(@"E:\ShowReportbyCustomerName.xml", XmlWriteMode.WriteSchema);
                    CR.SetDataSource(ds);
                    crystalReportViewer1.ReportSource = CR;

                }
               
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
               
            }

            else if (ReportType=="ShowReportbyCustomerDate")
            {
                 try
                {
                    CL = new CustomerList();
                    ds = dhr.SelectbyDate(date1, date2);
                    //ds.WriteXml(@"E:\ShowReportbyCustomerDate.xml", XmlWriteMode.WriteSchema);
                    CL.SetDataSource(ds);
                    crystalReportViewer1.ReportSource = CL;

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Data not found");
                }
            }

            else if (ReportType == "ShowReportbyCustomerList")
            {
                try
                {
                    CL = new CustomerList();
                    ds = dhr.SelectbyCustomerName(custmer);
                    //ds.WriteXml(@"E:\ShowReportbyCustomerList.xml", XmlWriteMode.WriteSchema);
                    CL.SetDataSource(ds);
                    crystalReportViewer1.ReportSource = CL;

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


            else
            {
                MessageBox.Show("Something Wrong");
            }
        }
    }
}
