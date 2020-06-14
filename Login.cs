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
    public partial class Login : Form
    {
        LMSDB2018Entities db = new LMSDB2018Entities();
        public static string Roll;
        public Login()
        {
            InitializeComponent();
        }

        //private void btnlogin_Click(object sender, EventArgs e)
        //{             
        //}

        //private void btnchngPwd_Click(object sender, EventArgs e)
        //{         
        //}

        //private void btncancel_Click(object sender, EventArgs e)
        //{            
        //}
        private void btncancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnlogin_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txtUserid.Text == "" || txtPwd.Text == "")
                {
                    MessageBox.Show("Please Enter UserId and Password");
                    return;
                }
                else
                {
                    var exuser = db.Users.Where(x => x.UserID == txtUserid.Text && x.Password == txtPwd.Text).FirstOrDefault();
                    if (exuser != null)
                    {
                        Form1 fs = new Form1();
                        fs.Show();
                        Roll = exuser.Role;
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect UserId or Password");
                        txtUserid.Focus();
                        txtUserid.Text = ""; txtPwd.Text = "";
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnchngPwd_Click_1(object sender, EventArgs e)
        {
            try
            {
                var userchng = db.Users.Where(x => x.UserID == textuserchng.Text && x.Password == txtoldpass.Text).FirstOrDefault();

                if (txtnewpass.Text != txtcnfmpass.Text)
                {
                    MessageBox.Show("confirm password not matching with new passsword");
                    txtnewpass.Text = "";
                    txtcnfmpass.Text = "";
                    txtnewpass.Focus();
                }

                else if (userchng == null)
                {
                    MessageBox.Show("Incorrect UserId or Password");
                    textuserchng.Focus();
                    textuserchng.Text = txtoldpass.Text = txtnewpass.Text = txtcnfmpass.Text = "";
                }

                else
                {
                    if (userchng != null)
                    {

                        userchng.Password = txtcnfmpass.Text;
                        db.SaveChanges();

                        MessageBox.Show("Password Changed Successfully");
                        textuserchng.Text = txtoldpass.Text = txtnewpass.Text = txtcnfmpass.Text = "";

                    }

                    else
                    {
                        MessageBox.Show("Something Wrong !!");
                    }

                }



            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
