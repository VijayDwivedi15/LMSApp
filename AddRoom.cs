using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;


namespace LMSAPP
{
    public partial class AddRoom : Form
    {
        LMSDB2018Entities db = new LMSDB2018Entities();
        DataHelperRoom dhr;
        public AddRoom()
        {
            InitializeComponent();
        }

        private void AddRoom_Load(object sender, EventArgs e)
        {
            comboBoxRoom.Focus();

           // var room = db.Rooms.ToList();
            comboBoxRoom.DataSource = db.Rooms.Select(x=>x.RoomNo).ToList();
            comboBoxRoom.DisplayMember = "RoomNo";
           // comboBoxRoom.ValueMember = "RoomNo";

            Clear();
       
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxRoom.Text == "")
                {
                    MessageBox.Show("Enter Room No");
                    comboBoxRoom.Focus();
                }
                else
                    if (textprice.Text == "")
                    {
                        MessageBox.Show("Enter Room Price");
                        textprice.Focus();
                    }

                    else if (!rbAC.Checked && !rbNonAC.Checked)
                    {
                        MessageBox.Show("Please Select Room Type ");
                        return;
                    }


                    else
                    {

                       // Room room = new Room();
                       // room.RoomNo = comboBoxRoom.Text;
                       // if (rbAC.Checked)
                       // {
                       //     room.RoomType = rbAC.Text;
                       // }
                       // else
                       // {
                       //     room.RoomType = rbNonAC.Text;
                       // }
                        
                       // room.Rate = Convert.ToDecimal(textprice.Text);

                       // db.Rooms.Add(room);
                       // db.SaveChanges();

                       //MessageBox.Show("Room Added Successfully");
                       //var rooms = db.Rooms.ToList();
                       //comboBoxRoom.Text = textprice.Text = "";

                        try
                        {
                            bool res = false;
                            dhr = new DataHelperRoom();
                            //db = new LMSDBEntities();
                            Room room = new Room();
                            room.RoomNo = comboBoxRoom.Text;
                            if (rbAC.Checked)
                            {
                                room.RoomType = rbAC.Text;
                            }
                            else
                            {
                                room.RoomType = rbNonAC.Text;
                            }

                            room.Rate = Convert.ToDecimal(textprice.Text);


                            var existroom = db.Rooms.Find(comboBoxRoom.Text);
                            if(existroom==null)
                            {
                                
                                res = dhr.AddRoom(room);
                                if (res == true)
                                {
                                    MessageBox.Show("Data Saved successfully");

                                }
                                else
                                {
                                    MessageBox.Show("operation failed");

                                }
                            }
                            else
                            {
                                if (Login.Roll == "Admin")
                                {
                                    res = dhr.AddRoom(room);
                                    if (res == true)
                                    {
                                        MessageBox.Show("Data Saved successfully");

                                    }
                                    else
                                    {
                                        MessageBox.Show("operation failed");

                                    }
                                }
                                else
                                {
                                    MessageBox.Show("you are not authorized");
                                }
                       

                              
                             
                            }
                           

                            Clear();
                            comboBoxRoom.DataSource = db.Rooms.Select(x => x.RoomNo).ToList();
                            comboBoxRoom.DisplayMember = "RoomNo";
                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }              
                                            
                    }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void comboBoxRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxRoom.SelectedValue.ToString() != "" )
                {
                    
                    //var timeout = db.TimeTables.Find(comboVisitorName.SelectedValue);
                    var currentvi = db.Rooms.Find(comboBoxRoom.SelectedValue);

                    comboBoxRoom.Text = currentvi.RoomNo;
                 
                    if (currentvi.RoomType == "AC")
                    {
                        rbAC.Checked = true;
                    }

                    else
                    {
                        rbNonAC.Checked = false;
                    }
                    if (currentvi.RoomType == "Non-AC")
                    {
                        rbNonAC.Checked = true;
                    }

                    else
                    {
                        rbNonAC.Checked = false;
                    }

                    textprice.Text = currentvi.Rate.ToString();
                }
                else
                {
                   
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Clear()
        {
            comboBoxRoom.Text = textprice.Text = "";
            rbAC.Checked = true;
        }

    }
}


//--------------End of Code----------------