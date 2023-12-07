using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Warehouse_Project.ashx
{
    /// <summary>
    /// supplier_order 的摘要说明
    /// </summary>
    public class supplier_order : IHttpHandler
    {
        string result = string.Empty;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request.Form["action"];
            switch (action)
            {
                case "get_supplier_order":
                    result = get_supplier_order(context);
                    break;
                case "add_transport":
                    result = add_transport(context);
                    break;
                case "count_transport":
                    result = count_transport(context);
                    break;
                case "ys_billno":
                    result = ys_billno(context);
                    break;
                case "max_change":
                    result = max_change(context);
                    break;
                case "get_supplier_transport":
                    result = get_supplier_transport(context);
                    break;
            }
            context.Response.Write(result);
        }

        private string get_supplier_order(HttpContext context)
        {
            string FSupplierID = context.Request.Form["FSupplierID"];
            string sql = string.Format(@"select distinct * from IT_MES..get_supplier_POOrder('{0}') order by 日期 desc", FSupplierID);
            DataTable dt = MES_DAL.DbHelperSQL.QueryDataTable(sql);
            string json_dt = JsonConvert.SerializeObject(dt);
            return json_dt;
        }

        private string add_transport(HttpContext context)
        {
            string FBillNo = context.Request.Form["FBillNo"];
            string FOrderBillNo = context.Request.Form["FOrderBillNo"];
            string FEntryID = context.Request.Form["FEntryID"];
            string FDate = context.Request.Form["FDate"];

            string FItemID = context.Request.Form["FItemID"];
            string FNumber = context.Request.Form["FNumber"];
            string FQty = context.Request.Form["FQty"];
            string FUnit = context.Request.Form["FUnit"];
            string FSupplierID = context.Request.Form["FSupplierID"];
            string FArea = context.Request.Form["FArea"];
            string FBatchNo = context.Request.Form["FBatchNo"];
            string FRemark = context.Request.Form["FRemark"];
            string sql = string.Format(@"insert into IT_MES..t_Transport select '{0}','{1}','{2}','{3}','','{4}','{5}','{6}','{7}','{8}','{9}','{10}','','{11}'"
                                        ,FBillNo,FOrderBillNo,FEntryID,FDate, FItemID, FNumber,FQty,FUnit,FSupplierID,FArea,FBatchNo,FRemark);
            int result = MES_DAL.DbHelperSQL.ExecuteSql(sql);
            return result.ToString();
        }
        private string count_transport(HttpContext context)
        {
            string sql = string.Format(@"select FOrderBillNo as 订单编号,FEntryID as 分录号,SUM(FQty) as 已送数量,COUNT(0) as 已送批次 from IT_MES..t_Transport group by FOrderBillNo,FEntryID");
            DataTable dt = MES_DAL.DbHelperSQL.QueryDataTable(sql);
            string json_dt = JsonConvert.SerializeObject(dt);
            return json_dt;
        }
        private string ys_billno(HttpContext context)
        {
            string sql = string.Format(@"declare @FMonth varchar(100) set @FMonth = (select substring(convert( nvarchar , datepart ( year , getdate() ) * 100 + datepart(month , getdate() )) ,0 , 7))
                        declare @TWSDNNo varchar(100) set @TWSDNNo =  (select RIGHT('000' + CONVERT(varchar, max_value),3) from IT_MES..Max_Number where key_name = 'TWSDN' )
                        declare @FBillNo varchar(100) set @FBillNo = 'TWSDN' + @FMonth + @TWSDNNo
                        select @FBillNo as FBillNo;");
            DataTable dt = MES_DAL.DbHelperSQL.QueryDataTable(sql);
            string json_dt = JsonConvert.SerializeObject(dt);
            return json_dt;
        }
        private string max_change(HttpContext context)
        {
            string sql = string.Format(@"update IT_MES..Max_Number set max_value = max_value + 1 where key_name = 'TWSDN';");
            int result = MES_DAL.DbHelperSQL.ExecuteSql(sql);
            return result.ToString();
        }
        private string get_supplier_transport(HttpContext context)
        {
            string FSupplierID = context.Request.Form["FSupplierID"];
            string sql = string.Format(@"select * from t_Transport where FSupplierID = '{0}' order by FDate desc",FSupplierID);
            DataTable dt = MES_DAL.DbHelperSQL.QueryDataTable(sql);
            string json_dt = JsonConvert.SerializeObject(dt);
            return json_dt;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}