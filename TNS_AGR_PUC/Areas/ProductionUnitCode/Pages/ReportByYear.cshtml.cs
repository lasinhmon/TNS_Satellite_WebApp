using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Claims;
using TNS.Auth;

namespace TNS_KPI.Areas.KPI_BankAccount.Pages
{
    [IgnoreAntiforgeryToken]
    public class ReportByYearModel : PageModel
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
        public IActionResult OnPostLoadData([FromBody] ItemRequest request)
        {
            CheckAuth();
            int zYearView = int.Parse(request.YearView);
            List<string[]> zData = ReportByYearData.GetReport(zYearView);
            return new JsonResult(zData);
        }
        public class ItemRequest
        {
            public string YearView { get; set; }
            public string DepartmentID { get; set; }
        }
    }
    public class ReportByYearData
    {
        public static List<string[]> GetReport(int InYear)
        {
            List<string[]> zResult = new List<string[]>();
            List<string[]> zDepartments = Models.AccessData.DepartmentForReport();

            foreach (string[] zInfo in zDepartments)
            {
                string[] zDepartmentInfo = new string[15];
                double zTotal = 0;
                zDepartmentInfo[0] = zInfo[0];
                zDepartmentInfo[1] = zInfo[1];
                double[] zMoneyTotal = ResultOfDepartment(InYear, zInfo[0]);
                for (int i = 0; i < 12; i++)
                {
                    zDepartmentInfo[i + 2] = zMoneyTotal[i].ToString("#,###,##0");
                    zTotal += zMoneyTotal[i];
                }
                zDepartmentInfo[14] = zTotal.ToString("#,###,##0");
                zResult.Add(zDepartmentInfo);
            }
            return zResult;
        }
        public static double[] ResultOfDepartment(int InYear, string DepartmentID)
        {
            string zSQL = @" SELECT InMonth, Sum(Account_TotalMoney * TyGia) AS MoneyTotal "
                           + " FROM [KPI_BankAccount_Department_Result] "
                           + " WHERE InYear = @InYear AND DepartmentID = @DepartmentID  "
                           + " GROUP BY InMonth ORDER BY InMonth";

            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
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
            double[] zResult = new double[12];
            foreach (DataRow zRow in zTable.Rows)
            {
                int zMonth = int.Parse(zRow["InMonth"].ToString());
                zResult[zMonth - 1] = double.Parse(zRow["MoneyTotal"].ToString());
            }
            return zResult;
        }


    }
}
