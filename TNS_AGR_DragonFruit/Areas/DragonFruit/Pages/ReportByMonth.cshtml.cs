using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Security.Claims;
using TNS.Auth;


namespace TNS_KPI.Areas.KPI_BankAccount.Pages
{
    [IgnoreAntiforgeryToken]
    public class ReportByMonthModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion

        public readonly List<string[]> Departments = Models.AccessData.DepartmentForReport();
        public void OnGet()
        {
            CheckAuth();
        }

        #region [ POST ]
        public IActionResult OnPostLoadData([FromBody] ItemRequest request)
        {
            CheckAuth();
            int zMonthView = int.Parse(request.MonthView);
            int zYearView = int.Parse(request.YearView);
            ItemResponse zResult = new ItemResponse();
            zResult.Data = ReportByMonthData.GetReport(zMonthView, zYearView);

            return new JsonResult(zResult);
        }
        public IActionResult OnPostLoadConflict([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            int zMonth, zYear;
            int.TryParse(request.MonthView, out zMonth);
            int.TryParse(request.YearView, out zYear);

            DataTable zDataConflic = ReportByMonthData.GetConflic(zMonth, zYear, request.SearchContent);

            ItemResponse zItem = new ItemResponse();
            string[] zItemData;

            foreach (DataRow zRow in zDataConflic.Rows)
            {
                zItemData = new string[10];

                zItemData[0] = zRow["AutoKey"].ToString();
                zItemData[1] = zRow["MA_KH"].ToString();
                zItemData[2] = zRow["TEN_KH"].ToString();
                zItemData[3] = zRow["SO_TAI_KHOAN"].ToString();
                double zMoney = double.Parse(zRow["CURRENT_BALANCE"].ToString());
                zItemData[4] = zMoney.ToString("#,###,##0");
                zItemData[5] = zRow["CCY"].ToString();
                double zTyGia = double.Parse(zRow["TYGIA"].ToString());
                zItemData[6] = zTyGia.ToString("#,###,##0");
                zItemData[7] = (zMoney * zTyGia).ToString("#,###,##0");
                zItemData[8] = zRow["TEN_PGD"].ToString();
                zItemData[9] = zRow["Departments"].ToString();

                zItem.Data.Add(zItemData);

            }

            return new JsonResult(zItem);
        }
        public IActionResult OnPostUpdateConflic([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            zMessageLog = ReportByMonthData.ProcessConflict(request.AutoKey, request.DepartmentID);
            return new JsonResult(zMessageLog);
        }
        public IActionResult OnPostChangeDepartmentOwner([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            zMessageLog = ReportByMonthData.ProcessDepartmentOwner(request.AutoKey, request.DepartmentID);
            return new JsonResult(zMessageLog);
        }

        public IActionResult OnPostLoadHistoryOfDepartment([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            int zMonth, zYear;
            int zAmountRecordDisplay;
            int.TryParse(request.MonthView, out zMonth);
            int.TryParse(request.YearView, out zYear);
            int.TryParse(request.AmountRecordDisplay, out zAmountRecordDisplay);
            DataTable zDataHistory = ReportByMonthData.GetHistory(zAmountRecordDisplay,zMonth, zYear, request.DepartmentID, request.SearchContent);
            ItemResponse zItem = new ItemResponse();
            string[] zItemData;

            foreach (DataRow zRow in zDataHistory.Rows)
            {
                zItemData = new string[15];

                zItemData[0] = zRow["AutoKey"].ToString();
                zItemData[1] = zRow["MA_KH"].ToString();
                zItemData[2] = zRow["TEN_KH"].ToString();
                zItemData[3] = zRow["SO_TAI_KHOAN"].ToString();
                double zMoney = double.Parse(zRow["CURRENT_BALANCE"].ToString());
                zItemData[4] = zMoney.ToString("#,###,##0");
                zItemData[5] = zRow["CCY"].ToString();
                double zTyGia = double.Parse(zRow["TYGIA"].ToString());
                zItemData[6] = zTyGia.ToString("#,###,##0");
                zItemData[7] = (zMoney * zTyGia).ToString("#,###,##0");
                zItemData[8] = zRow["TEN_PGD"].ToString();
                zItemData[9] = zRow["RecordStatus"].ToString();
                zItemData[10] = zRow["DepartmentOwner"].ToString();
                zItem.Data.Add(zItemData);

            }

            return new JsonResult(zItem);
        }
        public IActionResult OnPostLoadHistory([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            int zMonth, zYear;
            int zAmountRecordDisplay;
            int.TryParse(request.MonthView, out zMonth);
            int.TryParse(request.YearView, out zYear);
            ItemResponse zItem = new ItemResponse();
            string[] zItemData;
            if (request.SearchContent.Length > 0)
            {
                zAmountRecordDisplay = 0;
                DataTable zDataResult = ReportByMonthData.GetHistory(zAmountRecordDisplay, zMonth, zYear, request.SearchContent);

                foreach (DataRow zRow in zDataResult.Rows)
                {
                    zItemData = new string[15];

                    zItemData[0] = zRow["AutoKey"].ToString();
                    zItemData[1] = zRow["MA_KH"].ToString();
                    zItemData[2] = zRow["TEN_KH"].ToString();
                    zItemData[3] = zRow["SO_TAI_KHOAN"].ToString();
                    double zMoney = double.Parse(zRow["CURRENT_BALANCE"].ToString());
                    zItemData[4] = zMoney.ToString("#,###,##0");
                    zItemData[5] = zRow["CCY"].ToString();
                    double zTyGia = double.Parse(zRow["TYGIA"].ToString());
                    zItemData[6] = zTyGia.ToString("#,###,##0");
                    zItemData[7] = (zMoney * zTyGia).ToString("#,###,##0");
                    zItemData[8] = zRow["TEN_PGD"].ToString();
                    zItemData[9] = zRow["RecordStatus"].ToString();
                    zItemData[10] = zRow["DepartmentOwner"].ToString();

                    zItem.Data.Add(zItemData);

                }
            }
            return new JsonResult(zItem);
        }

        public IActionResult OnPostCaculatorStep([FromBody] ItemRequest request)
        {
            CheckAuth();
            int zMonthView = int.Parse(request.MonthView);
            int zYearView = int.Parse(request.YearView);
            ItemResponse zResult = new ItemResponse();
            string zMessage;
            if (request.Step == 1)
            {
                zMessage = ReportByMonthData.Caculator_Step1(zMonthView, zYearView);
                if (zMessage != "OK")
                {
                    zResult.Message = zMessage;
                    zResult.Status = "ERR";
                }
                else
                    zResult.Status = "OK";
            }
            if (request.Step == 2)
            {
                zMessage = ReportByMonthData.Caculator_Step2(zMonthView, zYearView);
                if (zMessage != "OK")
                {
                    zResult.Message = zMessage;
                    zResult.Status = "ERR";
                }
                else
                    zResult.Status = "OK";
            }

            if (request.Step == 3)
            {
                zResult.Message = ReportByMonthData.Caculator_Step3_1(zMonthView, zYearView);
                zResult.Message = ReportByMonthData.Caculator_Step3_2(zMonthView, zYearView);
                zResult.Message = ReportByMonthData.Caculator_Step3_3(zMonthView, zYearView);

            }

            return new JsonResult(zResult);
        }
        #endregion

        #region [ Request Respon ]
        public class ItemRequest
        {
            public string MonthView { get; set; }
            public string YearView { get; set; }
            public string AutoKey { get; set; }
            public string DepartmentID { get; set; }
            public string AccountNumber { get; set; }
            public string AmountRecordDisplay { get; set; }
            public string SearchContent { get; set; }
            public int Step { get; set; }
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

        #endregion

    }
    public class ReportByMonthData
    {
        public static List<string[]> GetReport(int InMonth, int InYear)
        {
            List<string[]> zResult = new List<string[]>();

            DataTable zTable = GetResult(InMonth, InYear);
            DataTable zTable_Analytic = GetAnalytic(InMonth, InYear);
            string[] zDepartmentInfo;
            double zMoney;
            int zAmount;
            foreach (DataRow zRow in zTable.Rows)
            {
                zDepartmentInfo = new string[15];

                zDepartmentInfo[0] = zRow["DepartmentID"].ToString();
                zDepartmentInfo[1] = zRow["DepartmentName"].ToString();

                zAmount = int.Parse(zRow["AmountVND"].ToString());
                zMoney = double.Parse(zRow["MoneyVND"].ToString());
                zDepartmentInfo[2] = zAmount.ToString("#,###,##0");
                zDepartmentInfo[3] = zMoney.ToString("#,###,##0");

                zAmount = int.Parse(zRow["AmountUSD"].ToString());
                zMoney = double.Parse(zRow["MoneyUSD"].ToString());
                zDepartmentInfo[4] = zAmount.ToString("#,###,##0");
                zDepartmentInfo[5] = zMoney.ToString("#,###,##0");

                zAmount = int.Parse(zRow["AmountEUR"].ToString());
                zMoney = double.Parse(zRow["MoneyEUR"].ToString());
                zDepartmentInfo[6] = zAmount.ToString("#,###,##0");
                zDepartmentInfo[7] = zMoney.ToString("#,###,##0");

                zAmount = int.Parse(zRow["AmountJPY"].ToString());
                zMoney = double.Parse(zRow["MoneyJPY"].ToString());
                zDepartmentInfo[8] = zAmount.ToString("#,###,##0");
                zDepartmentInfo[9] = zMoney.ToString("#,###,##0");

                zAmount = int.Parse(zRow["AmountTotal"].ToString());
                zMoney = double.Parse(zRow["MoneyTotal"].ToString());
                zDepartmentInfo[10] = zAmount.ToString("#,###,##0");
                zDepartmentInfo[11] = zMoney.ToString("#,###,##0");

                zResult.Add(zDepartmentInfo);

            }
            foreach (DataRow zRow in zTable_Analytic.Rows)
            {
                zDepartmentInfo = new string[15];

                zDepartmentInfo[0] = "";
                zDepartmentInfo[1] = zRow["ID"].ToString();

                zAmount = int.Parse(zRow["AmountVND"].ToString());
                zMoney = double.Parse(zRow["MoneyVND"].ToString());
                zDepartmentInfo[2] = zAmount.ToString("#,###,##0");
                zDepartmentInfo[3] = zMoney.ToString("#,###,##0");

                zAmount = int.Parse(zRow["AmountUSD"].ToString());
                zMoney = double.Parse(zRow["MoneyUSD"].ToString());
                zDepartmentInfo[4] = zAmount.ToString("#,###,##0");
                zDepartmentInfo[5] = zMoney.ToString("#,###,##0");

                zAmount = int.Parse(zRow["AmountEUR"].ToString());
                zMoney = double.Parse(zRow["MoneyEUR"].ToString());
                zDepartmentInfo[6] = zAmount.ToString("#,###,##0");
                zDepartmentInfo[7] = zMoney.ToString("#,###,##0");

                zAmount = int.Parse(zRow["AmountJPY"].ToString());
                zMoney = double.Parse(zRow["MoneyJPY"].ToString());
                zDepartmentInfo[8] = zAmount.ToString("#,###,##0");
                zDepartmentInfo[9] = zMoney.ToString("#,###,##0");

                zAmount = int.Parse(zRow["AmountTotal"].ToString());
                zMoney = double.Parse(zRow["MoneyTotal"].ToString());
                zDepartmentInfo[10] = zAmount.ToString("#,###,##0");
                zDepartmentInfo[11] = zMoney.ToString("#,###,##0");


                zResult.Add(zDepartmentInfo);

            }
            return zResult;
        }
        public static DataTable GetResult(int InMonth, int InYear)
        {
            string zSQL = @"[dbo].[KPI_BankAccount_GetResult]";

            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.StoredProcedure;
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            zTable.Columns.Add("AmountTotal", typeof(int));
            zTable.Columns.Add("MoneyTotal", typeof(Double));

            double zMoney_Total;
            int zAmount_Total;
            foreach (DataRow zRow in zTable.Rows)
            {
                zMoney_Total = double.Parse(zRow["MoneyVND"].ToString());
                zMoney_Total += double.Parse(zRow["MoneyUSD"].ToString());
                zMoney_Total += double.Parse(zRow["MoneyEUR"].ToString());
                zMoney_Total += double.Parse(zRow["MoneyJPY"].ToString());

                zAmount_Total = int.Parse(zRow["AmountVND"].ToString());
                zAmount_Total += int.Parse(zRow["AmountUSD"].ToString());
                zAmount_Total += int.Parse(zRow["AmountEUR"].ToString());
                zAmount_Total += int.Parse(zRow["AmountJPY"].ToString());

                zRow["AmountTotal"] = zAmount_Total;
                zRow["MoneyTotal"] = zMoney_Total;

            }
            return zTable;
        }
        public static DataTable GetAnalytic(int InMonth, int InYear)
        {
            string zSQL = @"KPI_BankAccount_Analytic";

            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.StoredProcedure;
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
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

        public static DataTable GetConflic(int InMonth, int InYear, string SearchContent)
        {
            DataTable zTable = new DataTable();

            string zSQL = @" SELECT AutoKey,MA_KH,TEN_KH,SO_TAI_KHOAN,CURRENT_BALANCE,CCY,TYGIA, "
                            + " [dbo].[KPI_BankAccount_GetConflict](@InMonth,@InYear,SO_TAI_KHOAN)  AS 'Departments' ,TEN_PGD "
                            + " FROM KPI_BankAccount_History "
                            + " WHERE InMonth = @InMonth AND InYear = @InYear  AND RecordStatus = 10 "
                            + " AND (MA_KH LIKE @SearchContent OR SO_TAI_KHOAN LIKE @SearchContent) "
                            + " ORDER BY MA_KH ";
            string zMessage = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
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
            return zTable;
        }
        public static string ProcessConflict(string AutoKey, string DepartmentOwner)
        {
            string zSQL = @"[dbo].[KPI_BankAccount_ConflictUpdate]";

            string zReslut = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.StoredProcedure;
                zCommand.Parameters.Add("@AutoKey", SqlDbType.NVarChar).Value = AutoKey;
                zCommand.Parameters.Add("@DepartmentID_Change", SqlDbType.NVarChar).Value = DepartmentOwner;
                zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zConnect.Close();
                zReslut = "OK";
            }
            catch (Exception ex)
            {
                zReslut = ex.ToString();
            }
            return zReslut;
        }
        public static string ProcessDepartmentOwner(string AutoKey, string DepartmentOwner)
        {
            string zSQL = @"[dbo].[KPI_BankAccount_ChangeOwner]";

            string zReslut = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.StoredProcedure;
                zCommand.Parameters.Add("@AutoKey", SqlDbType.NVarChar).Value = AutoKey;
                zCommand.Parameters.Add("@DepartmentID_Change", SqlDbType.NVarChar).Value = DepartmentOwner;
                zReslut = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zReslut = ex.ToString();
            }
            return zReslut;
        }

        public static DataTable GetHistory(int AmountRecordDisplay, int InMonth, int InYear, string DepartmentID, string SearchContent)
        {
            DataTable zTable = new DataTable();

            string zSQL = "SELECT TOP " + AmountRecordDisplay.ToString();
            if (AmountRecordDisplay == 0)
                zSQL = "SELECT ";

            zSQL += " AutoKey,MA_KH,TEN_KH,SO_TAI_KHOAN,CURRENT_BALANCE,CCY,TYGIA, TEN_PGD, DepartmentOwner,RecordStatus "
                            + " FROM KPI_BankAccount_History "
                            + " WHERE InMonth = @InMonth AND InYear = @InYear  AND DepartmentOwner = @DepartmentID "
                            + " AND CURRENT_BALANCE > 0 AND (MA_KH LIKE @SearchContent OR SO_TAI_KHOAN LIKE @SearchContent) "
                            + " ORDER BY MA_KH ";
            string zMessage = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
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
            return zTable;
        }
        public static DataTable GetHistory(int AmountRecordDisplay, int InMonth, int InYear, string SearchContent)
        {
            DataTable zTable = new DataTable();
            string zSQL = "SELECT TOP " + AmountRecordDisplay.ToString();
            if (AmountRecordDisplay == 0)
                zSQL = "SELECT ";

            zSQL += @" AutoKey,MA_KH,TEN_KH,SO_TAI_KHOAN,CURRENT_BALANCE,CCY,TYGIA, TEN_PGD, DepartmentOwner, RecordStatus "
                            + " FROM KPI_BankAccount_History "
                            + " WHERE InMonth = @InMonth AND InYear = @InYear "
                            + " AND CURRENT_BALANCE > 0 AND (MA_KH LIKE @SearchContent OR SO_TAI_KHOAN LIKE @SearchContent) "
                            + " ORDER BY MA_KH ";
            string zMessage = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
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
            return zTable;
        }
        public static string Caculator_Step1(int InMonth, int InYear)
        {
            string zResult = "";
            //---------- String SQL Access Database ---------------
            string zSQL = "[dbo].[KPI_BankAccount_Step1]";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.StoredProcedure;

                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zResult = "OK";
            }
            catch (Exception Err)
            {
                zResult = Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
        public static string Caculator_Step2(int InMonth, int InYear)
        {
            string zResult = "";
            //---------- String SQL Access Database ---------------
            string zSQL = "[dbo].[KPI_BankAccount_Step2]";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.StoredProcedure;

                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zResult = "OK";
            }
            catch (Exception Err)
            {
                zResult = Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
        public static string Caculator_Step3_1(int InMonth, int InYear)
        {
            string zResult = "";
            //---------- String SQL Access Database ---------------
            string zSQL = "[dbo].[KPI_BankAccount_Step3_1]";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.StoredProcedure;

                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zResult = "OK";
            }
            catch (Exception Err)
            {
                zResult = Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
        public static string Caculator_Step3_2(int InMonth, int InYear)
        {
            string zResult = "";
            //---------- String SQL Access Database ---------------
            string zSQL = "[dbo].[KPI_BankAccount_Step3_2]";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.StoredProcedure;

                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zResult = "OK";
            }
            catch (Exception Err)
            {
                zResult = Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
        public static string Caculator_Step3_3(int InMonth, int InYear)
        {
            string zResult = "";
            //---------- String SQL Access Database ---------------
            string zSQL = "[dbo].[KPI_BankAccount_Step3_3]";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.StoredProcedure;

                zCommand.Parameters.Add("@InMonth", SqlDbType.Int).Value = InMonth;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                zResult = "OK";
            }
            catch (Exception Err)
            {
                zResult = Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
    }
}
