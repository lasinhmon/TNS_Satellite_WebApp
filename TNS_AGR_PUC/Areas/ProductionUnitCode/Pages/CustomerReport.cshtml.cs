using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace TNS_KPI.Areas.KPI_BankAccount.Pages
{
    [IgnoreAntiforgeryToken]
    public class CustomerReportModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion
        public void OnGet()
        {
            CheckAuth();
        }
        public IActionResult OnPostLoadHistory([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMaKH = request.CustomerID;
            DataTable zData = CustomerReportData.GetHistoryOfCustomer(zMaKH);
            ItemResponse zItem = new ItemResponse();
          
            foreach(DataRow zRow in zData.Rows)
            {
                string[] zItemData = new string[15];

                zItemData[0] = zRow["AutoKey"].ToString();
                zItemData[1] = zRow["InMonth"].ToString().PadLeft(2, '0') + "-" + zRow["InYear"].ToString();
                zItemData[2] = zRow["SO_TAI_KHOAN"].ToString();
                double zMoney = double.Parse(zRow["CURRENT_BALANCE"].ToString());
                zItemData[3] = zMoney.ToString("#,###,##0");
                zItemData[4] = zRow["CCY"].ToString();
                double zTyGia = double.Parse(zRow["TYGIA"].ToString());
                zItemData[5] = zTyGia.ToString("#,###,##0");
                zItemData[6] = (zMoney * zTyGia).ToString("#,###,##0");
                zItemData[7] = zRow["TEN_PGD"].ToString();
                zItemData[8] = zRow["RecordStatus"].ToString();
                zItemData[9] = zRow["DepartmentOwner"].ToString();
               
                zItem.Data.Add(zItemData);
            }
            return new JsonResult(zItem);
        }
        public class ItemRequest
        {
            public string MonthView { get; set; }
            public string YearView { get; set; }
            public string CustomerID { get; set; }
        }
        public class ItemResponse
        {
            public string ID { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public string Status { get; set; }
            public List<string[]> Data { get; set; }
            public ItemResponse()
            {
                Data = new List<string[]>();
            }
        }

    }
    public class CustomerReportData
    {
        public static DataTable GetHistoryOfCustomer(string CustomerID)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT AutoKey, InMonth,InYear,SO_TAI_KHOAN,CURRENT_BALANCE,CCY,TYGIA, TEN_PGD, DepartmentOwner, RecordStatus "
                          + " FROM KPI_BankAccount_History "
                          + " WHERE MA_KH = @MA_KH "
                          + " ORDER BY InYear DESC,InMonth DESC";
            string zMessage = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@MA_KH", SqlDbType.NVarChar).Value = CustomerID;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();

            }
            catch (Exception ex)
            {
                string strMessage = ex.ToString();
            }
            return zTable;
        }
    }
}
