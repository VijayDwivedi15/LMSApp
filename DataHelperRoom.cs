using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LMSAPP
{
    class DataHelperRoom
    {
        LMSDB2018Entities db = new LMSDB2018Entities();
        
        public bool AddRoom(Room room)
        {
            bool res = false;
            try
            {
                using (var context = new LMSDB2018Entities())
                {
                    var RoomNo = new SqlParameter("@RoomNo", room.RoomNo);
                    var RoomType = new SqlParameter("@RoomType",room.RoomType);
                    var Rate = new SqlParameter("@Rate", room.Rate);

                    int i = context.Database.ExecuteSqlCommand("AddRoom @RoomNo, @RoomType,@Rate  ", RoomNo, RoomType, Rate);
                    if (i == 1)
                        res = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return res;
        }


        public bool AddCustomer(Customer customer)
        {
            bool res = false;
            try
            {
                using (var context = new LMSDB2018Entities())
                {
                    var CustomerID = new SqlParameter("@CustomerID", customer.CustomerID);
                    var Address = new SqlParameter("@Address", customer.Address);
                    var MobileNo = new SqlParameter("@MobileNo", customer.MobileNo);
                    var Photo = new SqlParameter("@Photo", customer.Photo);
                    var IdCard = new SqlParameter("@IdCard", customer.IdCard);
                    var IdCardNo = new SqlParameter("@IdCardNo", customer.IdCardNo);
                    var CustomerName = new SqlParameter("@CustomerName", customer.CustomerName);

                    int i = context.Database.ExecuteSqlCommand("AddCustomer @CustomerID, @Address, @MobileNo, @Photo, @IdCard, @IdCardNo, @CustomerName  ", CustomerID, Address, MobileNo, Photo, IdCard, IdCardNo, CustomerName);
                    if (i == 1)
                        res = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return res;
        }



    }
}
