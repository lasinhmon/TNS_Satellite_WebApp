using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Claims;

namespace TNS_KPI.Areas.KPI_BankAccount.Pages
{
    [IgnoreAntiforgeryToken]
    public class CustomersOfDepartmentModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion

        public readonly List<string[]> Departments = Models.AccessData.DepartmentForReport();

        public void OnGet(string styleView)
        {
            CheckAuth();

        }

        #region [ POST ]
        public IActionResult OnPostLoadData([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            ItemResponse zResult = new ItemResponse();
            int zMonthView = int.Parse(request.MonthView);
            int zYearView = int.Parse(request.YearView);
            int zAmountRecordDisplay = int.Parse(request.AmountRecordDisplay);
            int zTotal_Record = 0;
            string[] zDateConvert;

            zTotal_Record = CustomersOfDepartmentData.GetHistory_Count(zMonthView, zYearView, request.DepartmentID, request.SearchContent);
            zResult.Title = "Số lượng record tìm thấy : " + zTotal_Record.ToString("#,##0");
            zResult.Status = "OK";

            zResult.Data = CustomersOfDepartmentData.GetReport(zAmountRecordDisplay, zMonthView, zYearView, request.DepartmentID, request.SearchContent);

            return new JsonResult(zResult);
        }
        public IActionResult OnPostLoadCustomer([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            int zMonth, zYear;
            int zAmountRecordDisplay;
            int.TryParse(request.MonthView, out zMonth);
            int.TryParse(request.YearView, out zYear);
            DataTable zDataHistory = CustomersOfDepartmentData.GetHistoryOfCustomer(zMonth, zYear, request.CustomerID);
            ItemResponse zItem = new ItemResponse();
            string[] zItemData;

            foreach (DataRow zRow in zDataHistory.Rows)
            {
                zItemData = new string[15];

                zItemData[0] = zRow["AutoKey"].ToString();
                zItemData[1] = zRow["SO_TAI_KHOAN"].ToString();
                double zMoney = double.Parse(zRow["CURRENT_BALANCE"].ToString());
                zItemData[2] = zMoney.ToString("#,###,##0");
                zItemData[3] = zRow["CCY"].ToString();
                double zTyGia = double.Parse(zRow["TYGIA"].ToString());
                zItemData[4] = zTyGia.ToString("#,###,##0");
                zItemData[5] = (zMoney * zTyGia).ToString("#,###,##0");
                zItemData[6] = zRow["TEN_PGD"].ToString();
                zItemData[7] = zRow["RecordStatus"].ToString();
                zItemData[8] = zRow["DepartmentOwner"].ToString();
                zItem.Data.Add(zItemData);

            }

            return new JsonResult(zItem);
        }
        public IActionResult OnPostUpdateData([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            ItemResponse zItem = new ItemResponse();

            return new JsonResult(zItem);
        }
        #endregion

        #region[Request Respon]
        public class ItemRequest
        {
            public string MonthView { get; set; }
            public string YearView { get; set; }
            public string DepartmentID { get; set; }
            public string CustomerID { get; set; }
            public string SearchContent { get; set; }
            public string AmountRecordDisplay { get; set; }
            public string Key { get; set; }
        }

        public class ItemResponse
        {
            public string ID { get; set; }
            public string Title { get; set; }
            public List<string[]> Data { get; set; }
            public string Status { get; set; }
            public string Message { get; set; }
            public ItemResponse() {
                Data = new List<string[]>();
            }

        }

        #endregion
    }
    public class CustomersOfDepartmentData
    {
        public static List<string[]> GetReport(int AmountRecordDisplay, int InMonth, int InYear, string DepartmentID, string SearchConent)
        {
            List<string[]> zResult = new List<string[]>();

            DataTable zTable = GetHistory(AmountRecordDisplay, InMonth, InYear, DepartmentID, SearchConent);

            string[] zDepartmentInfo;
            double zMoney;
            int zAmount;
            foreach (DataRow zRow in zTable.Rows)
            {
                zDepartmentInfo = new string[10];

                zDepartmentInfo[0] = zRow["MA_KH"].ToString();
                zDepartmentInfo[1] = zRow["TEN_KH"].ToString();

                zMoney = double.Parse(zRow["VND"].ToString());
                zDepartmentInfo[2] = zMoney.ToString("#,###,##0");

                zMoney = double.Parse(zRow["USD"].ToString());
                zDepartmentInfo[3] = zMoney.ToString("#,###,##0");

                zMoney = double.Parse(zRow["EUR"].ToString());
                zDepartmentInfo[4] = zMoney.ToString("#,###,##0");

                zMoney = double.Parse(zRow["JPN"].ToString());
                zDepartmentInfo[5] = zMoney.ToString("#,###,##0");

                zMoney = double.Parse(zRow["MoneyTotal"].ToString());
                zDepartmentInfo[6] = zMoney.ToString("#,###,##0");

                zResult.Add(zDepartmentInfo);

            }
            if (SearchConent.Length == 0)
            {
                DataTable zTable_Analytic = GetResultOfDepartment(InMonth, InYear, DepartmentID);
                zDepartmentInfo = new string[10];
                zDepartmentInfo[0] = "";
                zDepartmentInfo[1] = "TOTAL";
                for(int i = 2;i<10;i++)
                {
                    zDepartmentInfo[i] = "0";
                }
                double zMoneyTotal = 0;
                foreach (DataRow zRow in zTable_Analytic.Rows)
                {
                    zMoney = double.Parse(zRow["MoneyCurrent"].ToString());
                    switch (zRow["LoaiTienTe"].ToString().Trim())
                    {
                        case "VND":
                            zDepartmentInfo[2] = zMoney.ToString("#,###,##0");
                            break;
                        case "USD":
                            zDepartmentInfo[3] = zMoney.ToString("#,###,##0");
                            break;
                        case "EUR":
                            zDepartmentInfo[4] = zMoney.ToString("#,###,##0");
                            break;
                        case "JPN":
                            zDepartmentInfo[5] = zMoney.ToString("#,###,##0");
                            break;
                    }
                    zMoneyTotal += zMoney;
                }
                zDepartmentInfo[6] = zMoneyTotal.ToString("#,###,##0");
                zResult.Insert(0,zDepartmentInfo);
            }
            return zResult;
        }
        public static int GetHistory_Count(int InMonth, int InYear, string DepartmentID, string SearchContent)
        {

            string zSQL = @"SELECT COUNT(*) FROM [dbo].[KPI_BankAccount_History] "
                        + " WHERE InMonth = @InMonth AND InYear = @InYear AND DepartmentOwner = @DepartmentID AND CURRENT_BALANCE > 0 "
                        + " AND (MA_KH LIKE @SearchContent OR SO_TAI_KHOAN LIKE @SearchContent OR TEN_KH LIKE @SearchContent) ";
            string zMessage = "";
            int zResult = 0;
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zCommand.Parameters.Add("@DepartmentID", SqlDbType.NVarChar).Value = DepartmentID;
                zCommand.Parameters.Add("@SearchContent", SqlDbType.NVarChar).Value = "%" + SearchContent + "%";
                zResult = int.Parse(zCommand.ExecuteScalar().ToString());

                zCommand.Dispose();
                zConnect.Close();

            }
            catch (Exception ex)
            {
                string strMessage = ex.ToString();
            }
            return zResult;
        }
        public static DataTable GetHistory(int AmountRecordDisplay, int InMonth, int InYear, string DepartmentID, string SearchContent)
        {
            DataTable zTable = new DataTable();

            string zSQL = @"SELECT * FROM  (";
            if (AmountRecordDisplay > 0)
                zSQL = @"SELECT TOP " + AmountRecordDisplay.ToString() + " * FROM  (";

            zSQL += " SELECT MA_KH,TEN_KH,CCY,CURRENT_BALANCE,TYGIA FROM [dbo].[KPI_BankAccount_History] "
                        + " WHERE DepartmentOwner = @DepartmentID AND InMonth = @InMonth AND InYear = @InYear AND CURRENT_BALANCE > 0 "
                        + " AND (MA_KH LIKE @SearchContent OR SO_TAI_KHOAN LIKE @SearchContent OR TEN_KH LIKE @SearchContent) "
                        + ") t PIVOT( SUM(CURRENT_BALANCE)"
                        + " FOR CCY IN ([VND], [USD], [EUR], [JPN]) ) AS pivot_table";
            string zMessage = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zCommand.Parameters.Add("@DepartmentID", SqlDbType.NVarChar).Value = DepartmentID;
                zCommand.Parameters.Add("@SearchContent", SqlDbType.NVarChar).Value = "%" + SearchContent + "%";
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string strMessage = ex.ToString();
            }
            zTable.Columns.Add("MoneyTotal", typeof(Double));

            double zMoneyVND, zMoneyUSD, zMoneyEUR, zMoneyJPN;
            double zMoneyTotal;
            double zTyGia = 0;
            foreach (DataRow zRow in zTable.Rows)
            {
                zTyGia = double.Parse(zRow["TYGIA"].ToString());
                double.TryParse(zRow["VND"].ToString(), out zMoneyVND);
                double.TryParse(zRow["USD"].ToString(), out zMoneyUSD);
                double.TryParse(zRow["EUR"].ToString(), out zMoneyEUR);
                double.TryParse(zRow["JPN"].ToString(), out zMoneyJPN);

                zMoneyVND = zMoneyVND * zTyGia;
                zMoneyUSD = zMoneyUSD * zTyGia;
                zMoneyEUR = zMoneyEUR * zTyGia;
                zMoneyJPN = zMoneyJPN * zTyGia;

                zRow["VND"] = zMoneyVND;
                zRow["USD"] = zMoneyUSD;
                zRow["EUR"] = zMoneyEUR;
                zRow["JPN"] = zMoneyJPN;

                zRow["MoneyTotal"] = zMoneyVND + zMoneyUSD + zMoneyEUR + zMoneyJPN;

            }
            return zTable;
        }
        public static DataTable GetHistoryOfCustomer(int InMonth, int InYear, string CustomerID)
        {
            DataTable zTable = new DataTable();
              string  zSQL = "SELECT AutoKey,SO_TAI_KHOAN,CURRENT_BALANCE,CCY,TYGIA, TEN_PGD, DepartmentOwner, RecordStatus "
                            + " FROM KPI_BankAccount_History "
                            + " WHERE InMonth = @InMonth AND InYear = @InYear "
                            + " AND CURRENT_BALANCE > 0 AND MA_KH = @MA_KH "
                            + " ORDER BY SO_TAI_KHOAN ";
            string zMessage = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
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
        public static DataTable GetResultOfDepartment(int InMonth, int InYear, string DepartmentID)
        {
            string zSQL = @"SELECT LoaiTienTe,TyGia*Account_TotalMoney AS MoneyCurrent "
                           + " FROM [dbo].[KPI_BankAccount_Department_Result] "
                           + " WHERE InMonth = @InMonth AND InYear = @InYear AND DepartmentID = @DepartmentID";

            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zCommand.Parameters.Add("@DepartmentID", SqlDbType.NVarChar).Value = DepartmentID;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }

            return zTable;
        }
    }
}
