using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebCam_Capture;

namespace LMSAPP
{
    public partial class AddCustomer : Form
    {
        LMSDB2018Entities db = new LMSDB2018Entities();
        long customerId = 0;
        long timetableid = 0;

        byte[] image = null, card = null;
        string imgPath = "";

        WebCam webcam;
        public AddCustomer()
        {
            InitializeComponent();
        }

        public void ClearCustomerDetail()
        {
            txtmobile.Text = "";
            txtIDnumber.Text = "";
            txtAddress.Text = "";
            textState.Text = "";
            comboCustomerName.Text = "";
            textPartyGST.Text = "";

        }

        private void AddCustomer_Load(object sender, EventArgs e)
        {
            var customer = db.Customers.ToList();
            comboCustomerName.DataSource = customer;
            comboCustomerName.DisplayMember = "CustomerName";
            comboCustomerName.ValueMember = "CustomerID";

            comboCustomerName.Focus();

            var room = db.Rooms.ToList();
            textRoomNo.DataSource = room;
            textRoomNo.DisplayMember = "RoomNo";
            textRoomNo.ValueMember = "RoomNo";

            webcam = new WebCam();
            webcam.InitializeWebCam(ref pictureBox1);

            if (Login.Roll != "Admin")
            {
                btnUpdate.Enabled = false;
                buttonDelete.Enabled = false;
            }

            Clear();


        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {


            if (comboCustomerName.Text == "")
            {
                MessageBox.Show("Please enter a Name !");
                return;
            }

            else if (txtAddress.Text == "")
            {
                MessageBox.Show("Please enter a Address !");
                return;
            }

            else if (txtmobile.Text == "")
            {
                MessageBox.Show("Please enter a Mobile No!");
                return;
            }

            else if (txtIDnumber.Text == "")
            {
                MessageBox.Show("Please enter a ID No!");
                return;
            }

            else
            {
                try
                {
                    var esistcus = db.Customers.Where(x => x.CustomerName == comboCustomerName.Text).FirstOrDefault();
                    if (esistcus == null)
                    {
                        Customer cust = new Customer();
                        cust.CustomerName = comboCustomerName.Text;
                        cust.Address = txtAddress.Text;
                        cust.MobileNo = Convert.ToInt64(txtmobile.Text);
                        cust.IdCardNo = txtIDnumber.Text;
                        cust.Photo = image;
                        cust.IdCard = card;
                        cust.PartyGST = textPartyGST.Text;
                        cust.State = textState.Text;


                        db.Customers.Add(cust);
                        db.SaveChanges();

                        MessageBox.Show("Data Saved successfully");
                        ClearCustomerDetail();


                        if (customerId == 0)
                        {
                            var currentvi = db.Customers.OrderByDescending(x => x.CustomerID).FirstOrDefault();
                            if (currentvi != null)
                            {
                                customerId = currentvi.CustomerID;
                            }
                        }
                    }
                    else
                    {
                        if (Login.Roll == "Admin")
                        {
                            esistcus.Address = txtAddress.Text;
                            esistcus.MobileNo = Convert.ToInt64(txtmobile.Text);
                            esistcus.IdCardNo = txtIDnumber.Text;
                            esistcus.Photo = image;
                            esistcus.IdCard = card;
                            esistcus.PartyGST = textPartyGST.Text;
                            esistcus.State = textState.Text;
                            esistcus.CustomerName = comboCustomerName.Text;
                            db.SaveChanges();
                            customerId = esistcus.CustomerID;
                            MessageBox.Show("Data Updated successfully");
                            ClearCustomerDetail();
                        }
                        else
                        {
                            MessageBox.Show("you are not authorized");
                        }


                    }


                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }

            }
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;

        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }


        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Jpeg images(*.jpeg)|Bitmap Images(*.png)|All Files(*.*)|*.*";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                imgPath = openFileDialog1.FileName;
                pictureBox1.ImageLocation = imgPath;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            webcam.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            webcam.Stop();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            webcam.Continue();
        }

        // button capture Image
        private void btnCapture_Click(object sender, EventArgs e)
        {

            pictureBoxImage.Image = pictureBox1.Image;
            image = ImageToByte(pictureBox1.Image);
        }


        // button capture Image ID Card
        private void btnCpture_Click(object sender, EventArgs e)
        {

            pictureBoxIDCard.Image = pictureBox1.Image;

            card = ImageToByte(pictureBox1.Image);
        }


        class WebCam
        {
            private WebCamCapture webcam;
            private System.Windows.Forms.PictureBox _FrameImage;
            private int FrameNumber = 30;
            public void InitializeWebCam(ref System.Windows.Forms.PictureBox ImageControl)
            {
                webcam = new WebCamCapture();
                webcam.FrameNumber = ((ulong)(0ul));
                webcam.TimeToCapture_milliseconds = FrameNumber;
                webcam.ImageCaptured += new WebCamCapture.WebCamEventHandler(webcam_ImageCaptured);
                _FrameImage = ImageControl;
            }

            void webcam_ImageCaptured(object source, WebcamEventArgs e)
            {
                _FrameImage.Image = e.WebCamImage;
            }
            public void Start()
            {
                webcam.TimeToCapture_milliseconds = FrameNumber;
                webcam.Start(0);
            }

            public void Stop()
            {
                webcam.Stop();
            }

            public void Continue()
            {
                // change the capture time frame.
                webcam.TimeToCapture_milliseconds = FrameNumber;

                // resume the video capture from the stop.
                webcam.Start(this.webcam.FrameNumber);
            }
            public static void SaveImageCapture(System.Drawing.Image image)
            {
                SaveFileDialog s = new SaveFileDialog();
                s.FileName = "Image";   // Default file name.
                s.DefaultExt = ".Jpg";  // Default file extension.
                s.Filter = "Image (.jpg)|*.jpg";    // Filter files by extension.

                // Show save file dialog box.
                if (s.ShowDialog() == DialogResult.OK)
                {
                    // Save Image.
                    string filename = s.FileName;
                    FileStream fstream = new FileStream(filename, FileMode.Create);
                    image.Save(fstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    fstream.Close();
                }
            }
        }

        private void btnChkIn_Click(object sender, EventArgs e)
        {
            try
            {
                TimeTable tt = new TimeTable();
                var currentvi = db.Customers.Find(customerId);

                if (textRoomNo.Text == "")
                {
                    MessageBox.Show("Enter Room No.");
                }
                else if (textprice.Text == "")
                {
                    MessageBox.Show("Enter Room Price");
                }

                else if (currentvi != null)
                {
                    if (textAdvance.Text == "")
                    {
                        textAdvance.Text = "0";
                    }

                    var inviceno = db.TimeTables.OrderByDescending(t => t.TimeTableID).FirstOrDefault();
                    Int64 invoicenumber = inviceno.InvoiceNo + 1;

                    tt.CheckInTime = Convert.ToDateTime(InTimePicker.Text);
                    //tt.Date = datePicker1.Value.Date;
                    tt.CustomerID = currentvi.CustomerID;
                    tt.RoomNo = textRoomNo.Text;
                    tt.Rate = Convert.ToDecimal(textprice.Text);
                    tt.Advance = Convert.ToDecimal(textAdvance.Text);
                    tt.Remark = textRemark.Text;
                    tt.Bevrages = Convert.ToDecimal(textBevrages.Text);
                    tt.Laundry = Convert.ToDecimal(textlaundry.Text);
                    tt.PickUpandDrop = Convert.ToDecimal(textPickupnDrop.Text);
                    tt.Extra = Convert.ToDecimal(textExtra.Text);
                    tt.Food = Convert.ToDecimal(textFood.Text);
                    tt.MiniBite = Convert.ToDecimal(textMiniBite.Text);
                    tt.SessionName = "2019-20";
                    tt.InvoiceNo = invoicenumber;

                    if (checkBoxGST.Checked == true)
                    {
                        tt.GST = true;
                    }
                    else
                    {
                        tt.GST = false;
                    }

                        db.TimeTables.Add(tt);
                        db.SaveChanges();

                        var Customertime = db.TimeTables.Where(x => x.CustomerID == currentvi.CustomerID).ToList();

                        dataGridView1.DataSource = Customertime;

                        MessageBox.Show("Successfully Checked In");

                        var visitortable = db.Customers.ToList();
                        comboCustomerName.DataSource = visitortable;
                        comboCustomerName.DisplayMember = "CustomerName";
                        comboCustomerName.ValueMember = "CustomerID";
                        Clear();
                    }
               


                else
                {
                    MessageBox.Show("Select Customer From List");
                    comboCustomerName.Focus();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnChkOut_Click(object sender, EventArgs e)
        {
            try
            {

                //var inviceno = db.TimeTables.OrderByDescending(t => t.TimeTableID).FirstOrDefault();
                //Int64 invoicenumber = inviceno.InvoiceNo + 1;

                if (timetableid == 0)
                {
                    MessageBox.Show("Select Time Table from list");
                    dataGridView1.Focus();
                }

                else
                {
                    var timetablefind = db.TimeTables.Find(timetableid);
                    if (timetablefind != null)
                    {

                        if (timetablefind.CheckOutTime == null && Login.Roll != "Admin")
                        {

                            timetablefind.CheckOutTime = Convert.ToDateTime(OutTimePicker.Text);
                            timetablefind.CheckOutTime = Convert.ToDateTime(OutTimePicker.Text);
                            timetablefind.Bevrages = Convert.ToDecimal(textBevrages.Text);
                            timetablefind.Laundry = Convert.ToDecimal(textlaundry.Text);
                            timetablefind.PickUpandDrop = Convert.ToDecimal(textPickupnDrop.Text);
                            timetablefind.Extra = Convert.ToDecimal(textExtra.Text);
                            timetablefind.Food = Convert.ToDecimal(textFood.Text);
                            timetablefind.MiniBite = Convert.ToDecimal(textMiniBite.Text);
                            timetablefind.Remark = textRemark.Text;
                            //timetablefind.InvoiceNo = invoicenumber;


                            db.SaveChanges();

                            Int64 FindcustomerId = Convert.ToInt64(comboCustomerName.SelectedValue);

                            var customertime = db.TimeTables.Where(x => x.CustomerID == FindcustomerId).ToList();
                            MessageBox.Show("Successfully Checked Out");
                            dataGridView1.DataSource = customertime;

                        }
                        else if (Login.Roll == "Admin")
                        {
                            timetablefind.CheckOutTime = Convert.ToDateTime(OutTimePicker.Text);
                            timetablefind.Bevrages = Convert.ToDecimal(textBevrages.Text);
                            timetablefind.Laundry = Convert.ToDecimal(textlaundry.Text);
                            timetablefind.PickUpandDrop = Convert.ToDecimal(textPickupnDrop.Text);
                            timetablefind.Extra = Convert.ToDecimal(textExtra.Text);
                            timetablefind.Food = Convert.ToDecimal(textFood.Text);
                            timetablefind.MiniBite = Convert.ToDecimal(textMiniBite.Text);
                            timetablefind.Remark = textRemark.Text;
                            //timetablefind.InvoiceNo = invoicenumber;


                            db.SaveChanges();

                            Int64 FindcustomerId = Convert.ToInt64(comboCustomerName.SelectedValue);

                            var customertime = db.TimeTables.Where(x => x.CustomerID == FindcustomerId).ToList();
                            MessageBox.Show("Successfully Checked Out");
                            dataGridView1.DataSource = customertime;

                        }
                        else
                        {
                            MessageBox.Show("you are not authorized");
                        }

                        // var visitortable = db.Visitors.ToList();
                        // comboVisitorName.DataSource = visitortable;
                        // comboVisitorName.DisplayMember = "VisitorName";
                        // comboVisitorName.ValueMember = "VisitorId";
                        // Clear();
                    }

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            try
            {

                if (timetableid == 0)
                {
                    MessageBox.Show("Select Visitor from list");
                    comboCustomerName.Focus();
                }

                else
                {
                    if (Login.Roll == "Admin")
                    {
                        var timetablefind = db.TimeTables.Find(timetableid);
                        if (timetablefind != null)
                        {
                            //timetablefind.Date = datePicker1.Value.Date;
                            timetablefind.CheckInTime = Convert.ToDateTime(InTimePicker.Text);
                            if (timetablefind.CheckOutTime != null)
                                timetablefind.CheckOutTime = Convert.ToDateTime(OutTimePicker.Text);
                            timetablefind.RoomNo = textRoomNo.Text;
                            timetablefind.Rate = Convert.ToDecimal(textprice.Text);
                            timetablefind.Advance = Convert.ToDecimal(textAdvance.Text);
                            timetablefind.Remark = textRemark.Text;
                            timetablefind.Bevrages = Convert.ToDecimal(textBevrages.Text);
                            timetablefind.Laundry = Convert.ToDecimal(textlaundry.Text);
                            timetablefind.PickUpandDrop = Convert.ToDecimal(textPickupnDrop.Text);
                            timetablefind.Extra = Convert.ToDecimal(textExtra.Text);
                            timetablefind.Food = Convert.ToDecimal(textFood.Text);
                            timetablefind.MiniBite = Convert.ToDecimal(textMiniBite.Text);

                            if (checkBoxGST.Checked == true)
                            {
                                timetablefind.GST = true;
                            }
                            else
                            {
                                timetablefind.GST = false;
                            }

                            db.SaveChanges();

                            Int64 FindcustomerId = Convert.ToInt64(comboCustomerName.SelectedValue);

                            var customertime = db.TimeTables.Where(x => x.CustomerID == FindcustomerId).ToList();

                            dataGridView1.DataSource = customertime;
                            MessageBox.Show("Data Updated successfully");
                        }
                    }
                    else
                    {
                        MessageBox.Show("you are not authorized");
                    }
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            ClearDateTime();
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {

            try
            {
                ViewReport rc = new ViewReport();
                rc.custmername = comboCustomerName.Text;
                rc.ReportType = "ShowReportbyCustomerName";
                rc.timetableid = timetableid;
                rc.Show();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (timetableid == 0)
                {
                    MessageBox.Show("Select Time Table from list");
                    dataGridView1.Focus();
                }

                else
                {
                    var timetablefind = db.TimeTables.Find(timetableid);
                    if (timetablefind != null)
                    {
                        db.TimeTables.Remove(timetablefind);
                        db.SaveChanges();

                        Int64 FindcustomerId = Convert.ToInt64(comboCustomerName.SelectedValue);

                        var visitortime = db.TimeTables.Where(x => x.CustomerID == FindcustomerId).ToList();
                        MessageBox.Show("Successfully Removed");
                        dataGridView1.DataSource = visitortime;

                        // var visitortable = db.Visitors.ToList();
                        // comboVisitorName.DataSource = visitortable;
                        //  comboVisitorName.DisplayMember = "VisitorName";
                        // comboVisitorName.ValueMember = "VisitorId";
                        // Clear();

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {


                var timetbl = db.TimeTables.Find(timetableid);
                if (timetbl != null)
                {
                    //datePicker1.Text = timetbl.Date.ToString();
                    InTimePicker.Text = timetbl.CheckInTime.ToString();
                    textRoomNo.Text = timetbl.RoomNo;
                    textprice.Text = Convert.ToString(timetbl.Rate);
                    textAdvance.Text = timetbl.Advance.ToString();
                    textRemark.Text = timetbl.Remark;
                    textBevrages.Text = timetbl.Bevrages.ToString();
                    textlaundry.Text = timetbl.Laundry.ToString();
                    textPickupnDrop.Text = timetbl.PickUpandDrop.ToString();
                    textExtra.Text = timetbl.Extra.ToString();
                    textMiniBite.Text = timetbl.MiniBite.ToString();
                    textFood.Text = timetbl.Food.ToString();

                    if (timetbl.GST == true)
                    {
                        checkBoxGST.Checked = true;
                    }
                    else
                    {
                        checkBoxGST.Checked = false;
                    }

                    if (timetbl.CheckOutTime != null)
                    {
                        OutTimePicker.Text = timetbl.CheckOutTime.ToString();
                    }

                    else
                    {
                        OutTimePicker.Text = null;
                    }

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void ClearDateTime()
        {
            //datePicker1.Text = null;
            InTimePicker.Text = null;
            OutTimePicker.Text = null;
            textRoomNo.Text = null;
            textprice.Text = null;
            textAdvance.Text = "";
            textRemark.Text = "";
            textBevrages.Text = "";
            textlaundry.Text = "";
            textPickupnDrop.Text = "";
            textExtra.Text = "";
            textFood.Text = "";
            textMiniBite.Text = "";

        }


        private void Clear()
        {

            timetableid = 0;
            customerId = 0;
            comboCustomerName.Text = "";
            txtmobile.Text = "";
            txtAddress.Text = "";
            //datePicker1.Text = null;
            InTimePicker.Text = null;
            OutTimePicker.Text = null;
            textRoomNo.Text = "";
            txtIDnumber.Text = "";
            textprice.Text = null;
            textAdvance.Text = "";
            textRemark.Text = "";
            textBevrages.Text = "";
            textlaundry.Text = "";
            textPickupnDrop.Text = "";
            textExtra.Text = "";
            textPartyGST.Text = "";
            textMiniBite.Text = "";
            textState.Text = "";
            textFood.Text = "";
            checkBoxGST.Checked = false;
            pictureBoxImage.Image = global::LMSAPP.Properties.Resources.blank_user;
            pictureBoxIDCard.Image = global::LMSAPP.Properties.Resources.blank_id;
            image = ImageToByte(pictureBoxImage.Image);
            card = ImageToByte(pictureBoxIDCard.Image);
            dataGridView1.DataSource = null;
            pictureBox1.Image = null;
            comboCustomerName.Focus();
        }

        private void comboCustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboCustomerName.SelectedValue.ToString() != null && comboCustomerName.SelectedValue.ToString() != "")
                {
                    customerId = Convert.ToInt64(comboCustomerName.SelectedValue);
                    //var timeout = db.TimeTables.Find(comboVisitorName.SelectedValue);
                    var currentCust = db.Customers.Find(comboCustomerName.SelectedValue);

                    comboCustomerName.Text = currentCust.CustomerName;
                    txtmobile.Text = currentCust.MobileNo.ToString();
                    txtAddress.Text = currentCust.Address;
                    txtIDnumber.Text = currentCust.IdCardNo;
                    textPartyGST.Text = currentCust.PartyGST;
                    textState.Text = currentCust.State;


                    if (currentCust.Photo != null)
                    {


                        pictureBoxImage.Image = byteArrayToImage(currentCust.Photo);
                        image = currentCust.Photo;

                    }

                    else
                    {
                        pictureBoxImage.Image = null;
                        image = null;
                    }

                    if (currentCust.IdCard != null)
                    {
                        pictureBoxIDCard.Image = byteArrayToImage(currentCust.IdCard);
                        card = currentCust.IdCard;

                    }

                    else
                    {
                        pictureBoxIDCard.Image = null;
                        card = null;
                    }

                    //datePicker1.Text = timeout.Date.ToShortDateString();


                    var customertime = db.TimeTables.Where(x => x.CustomerID == currentCust.CustomerID).ToList();
                    dataGridView1.DataSource = customertime;

                }
                else
                {
                    Clear();
                }

            }
            catch (Exception ex)
            {
                Clear();
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void txtmobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && Convert.ToInt32(e.KeyChar) != 8)
            {
                MessageBox.Show("Enter numerics only");
                e.Handled = true;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                int sri = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow sr = dataGridView1.Rows[sri];
                //outtime = Convert.ToString(sr.Cells[3].Value);
                timetableid = Convert.ToInt64(sr.Cells[0].Value);

                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;

            }
            catch (Exception ex)
            {

            }
        }

        private void textRoomNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (textRoomNo.SelectedValue.ToString() != "")
                {
                    //var timeout = db.TimeTables.Find(comboVisitorName.SelectedValue);
                    var currentvi = db.Rooms.Find(textRoomNo.SelectedValue);

                    textprice.Text = currentvi.Rate.ToString();

                }

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
        }

        private void textprice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && Convert.ToInt32(e.KeyChar) != 8)
            {
                MessageBox.Show("Enter numerics only");
                e.Handled = true;
            }
        }

        private void textAdvance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && Convert.ToInt32(e.KeyChar) != 8)
            {
                MessageBox.Show("Enter numerics only");
                e.Handled = true;
            }
        }

        private void textPickupnDrop_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && Convert.ToInt32(e.KeyChar) != 8)
            {
                MessageBox.Show("Enter numerics only");
                e.Handled = true;
            }
        }

        private void textlaundry_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && Convert.ToInt32(e.KeyChar) != 8)
            {
                MessageBox.Show("Enter numerics only");
                e.Handled = true;
            }
        }

        private void textBevrages_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && Convert.ToInt32(e.KeyChar) != 8)
            {
                MessageBox.Show("Enter numerics only");
                e.Handled = true;
            }
        }

        private void textExtra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && Convert.ToInt32(e.KeyChar) != 8)
            {
                MessageBox.Show("Enter numerics only");
                e.Handled = true;
            }
        }

        private void textFood_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && Convert.ToInt32(e.KeyChar) != 8)
            {
                MessageBox.Show("Enter numerics only");
                e.Handled = true;
            }
        }

        private void textMiniBite_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && Convert.ToInt32(e.KeyChar) != 8)
            {
                MessageBox.Show("Enter numerics only");
                e.Handled = true;
            }
        }



    }
}
