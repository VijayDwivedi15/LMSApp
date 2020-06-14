using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace LMSAPP
{
    class DataHelperReport
    {
        OleDbConnection con;
        OleDbCommand cmd;
        OleDbDataAdapter da;
        DataSet ds;

        public string ConStr = System.Configuration.ConfigurationManager.AppSettings.Get("sqlCon");// + Form1.DataBaseName + ".mdb;Jet OLEDB:Database Password=shyamsir";


        public DataSet SelectbyDate(string da1, string da2)
        {

            DateTime date1 = Convert.ToDateTime(da1);
            int d1 = date1.Day;
            int m1 = date1.Month;
            int y1 = date1.Year;

            DateTime date2 = Convert.ToDateTime(da2);
            int d2 = date2.Day;
            int m2 = date2.Month;
            int y2 = date2.Year;
            ds = new DataSet();
            con = new OleDbConnection(ConStr);
            cmd = new OleDbCommand("select * from view_getbill where CAST(CheckInTime as date) between  '" + y1 + "-" + m1 + "-" + d1 + "'  and  '" + y2 + "-" + m2 + "-" + d2 + "' ", con);
            da = new OleDbDataAdapter(cmd);



            da.Fill(ds);
            //da.Fill(ds, "");
            return ds;
        }


        public DataSet SelectName(string custmername, long timetableid)
        {

            ds = new DataSet();
            con = new OleDbConnection(ConStr);
            cmd = new OleDbCommand("select * from view_getbill where CustomerName= '" + custmername + "' and TimeTableId=" + timetableid + "", con);
            da = new OleDbDataAdapter(cmd);
            da.Fill(ds);
            //da.Fill(ds, "");
            return ds;
        }


        public DataSet SelectbyCustomerName(string custmer)
        {

            ds = new DataSet();
            con = new OleDbConnection(ConStr);
            cmd = new OleDbCommand("select * from view_getbill where CustomerName= '" + custmer + "'", con);
            da = new OleDbDataAdapter(cmd);
            da.Fill(ds);
            //da.Fill(ds, "");
            return ds;
        }


    }

}
