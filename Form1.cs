using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMSAPP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void customerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCustomer ac = new AddCustomer();
            ac.MdiParent = this;
            ac.Show();
        }

        private void roomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRoom ar = new AddRoom();
            ar.MdiParent = this;
            ar.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult res;
            res = MessageBox.Show("DO YOU WANT TO BACKUP DATABASE", "CONFERMATION TO BACKUP DATABASE", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                try
                {

                    // read connectionstring from config file
                    var connectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;

                    // read backup folder from config file ("C:/temp/")
                    var backupFolder = ConfigurationManager.AppSettings["BackupFolder"];

                    var sqlConStrBuilder = new SqlConnectionStringBuilder(connectionString);

                    // set backupfilename (you will get something like: "C:/temp/MyDatabase-2013-12-07.bak")
                    var backupFileName = String.Format("{0}{1}-{2}.bak",
                        backupFolder, sqlConStrBuilder.InitialCatalog,
                        DateTime.Now.ToString("yyyy-MM-dd"));

                    using (var connection = new SqlConnection(sqlConStrBuilder.ConnectionString))
                    {
                        var query = String.Format("BACKUP DATABASE {0} TO DISK='{1}'",
                            sqlConStrBuilder.InitialCatalog, backupFileName);

                        using (var command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                    // FileSystem.CopyDirectory(@"C:\Shyam Salution\Data", @"D:\CanteenBATABASE", true);
                    // FileSystem.CopyDirectory(@"C:\Shyam Salution\Data", @"E:\CanteenBATABASE", true);

                    this.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); this.Close(); }
            }
            else
            {
                this.Close();
            }
            //if (MessageBox.Show("Are you sure you want to close the form?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            //{
            //    this.Close();
            //}
           
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report rp = new Report();                        
            rp.MdiParent = this;
            rp.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult res;
            res = MessageBox.Show("DO YOU WANT TO BACKUP DATABASE", "CONFERMATION TO BACKUP DATABASE", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                try
                {

                    // read connectionstring from config file
                    var connectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;

                    // read backup folder from config file ("C:/temp/")
                    var backupFolder = ConfigurationManager.AppSettings["BackupFolder"];

                    var sqlConStrBuilder = new SqlConnectionStringBuilder(connectionString);

                    // set backupfilename (you will get something like: "C:/temp/MyDatabase-2013-12-07.bak")
                    var backupFileName = String.Format("{0}{1}-{2}.bak",
                        backupFolder, sqlConStrBuilder.InitialCatalog,
                        DateTime.Now.ToString("yyyy-MM-dd"));

                    using (var connection = new SqlConnection(sqlConStrBuilder.ConnectionString))
                    {
                        var query = String.Format("BACKUP DATABASE {0} TO DISK='{1}'",
                            sqlConStrBuilder.InitialCatalog, backupFileName);

                        using (var command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                   // FileSystem.CopyDirectory(@"C:\Shyam Salution\Data", @"D:\CanteenBATABASE", true);
                   // FileSystem.CopyDirectory(@"C:\Shyam Salution\Data", @"E:\CanteenBATABASE", true);


                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }
    }
}
