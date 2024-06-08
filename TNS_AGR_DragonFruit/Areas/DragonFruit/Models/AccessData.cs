using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace TNS_AGR.Areas.DragonFruit.Models
{
    public class AccessData
    {
        public static List<string[]> GetLocationInfo(string ID)
        {
          
            string zSQL = "SELECT ID,Name,ValuePath,ParentID,LocationType,ROI  FROM [dbo].[SYS_Location]" +
                "WHERE ID=@ID";

            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem = new string[7];
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                
                zItem[0] = zRow["ID"].ToString();
                zItem[1] = zRow["Name"].ToString();
                zItem[2] = zRow["ValuePath"].ToString();
                zItem[3] = zRow["ParentID"].ToString();
                zItem[4] = zRow["LocationType"].ToString();
                zItem[5] = zRow["ROI"].ToString();
                zResult.Add(zItem);
            }

            return zResult;
        }
        public static List<string[]> GetWardByDistrict(int InYear, string ParentID)
        {
            string zMessage = "";
            string zSQL = @" SELECT distinct ID,Name,ParentID,LocationType   
                    FROM [dbo].[SYS_Location] A
                    RIGHT JOIN [dbo].[AGR_DragonFruit_Planting] B on A.ID=B.LocationID
                    WHERE LocationType='ward' AND ParentID=@ParentID AND InYear=@InYear
                    ORDER BY ID";
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@ParentID", SqlDbType.NVarChar).Value = ParentID;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            if (zMessage.Length > 0)
            {
                zItem = new string[4];
                zItem[0] = "ERROR";
                zItem[1] = "zMessage";
                zResult.Add(zItem);
            }
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                zResult.Add(zItem);
            }

            return zResult;
        }
        public static List<string[]> GetProcessByWard(int InYear, string ParentID)
        {
            string zMessage = "";
            string zSQL = @" SELECT distinct ID,Name,ParentID,LocationType   
                    FROM [dbo].[SYS_Location] A
                    RIGHT JOIN [dbo].[AGR_DragonFruit_Planting] B on A.ID=B.LocationID
                    WHERE LocationType='ward' AND ParentID=@ParentID AND InYear=@InYear
                    ORDER BY ID";
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@ParentID", SqlDbType.NVarChar).Value = ParentID;
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            if (zMessage.Length > 0)
            {
                zItem = new string[4];
                zItem[0] = "ERROR";
                zItem[1] = "zMessage";
                zResult.Add(zItem);
            }
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                zResult.Add(zItem);
            }

            return zResult;
        }
        public static List<string[]> GetWardByDistrict(string ParentID)
        {
            string zMessage = "";
            string zSQL = @" SELECT distinct ID,Name,ParentID,LocationType   
                    FROM [dbo].[SYS_Location] A
                    RIGHT JOIN [dbo].[AGR_DragonFruit_Planting] B on A.ID=B.LocationID
                    WHERE LocationType='ward' AND ParentID=@ParentID 
                    ORDER BY ID";
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@ParentID", SqlDbType.NVarChar).Value = ParentID;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            if (zMessage.Length > 0)
            {
                zItem = new string[4];
                zItem[0] = "ERROR";
                zItem[1] = "zMessage";
                zResult.Add(zItem);
            }
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                zResult.Add(zItem);
            }

            return zResult;
        }
        public static List<string[]> GetDistrictByProvince(string ParentID)
        {
            string zMessage = "";
            string zSQL = @" SELECT ID,Name,ParentID,LocationType   
                    FROM [dbo].[SYS_Location] A
                    RIGHT JOIN [dbo].[AGR_DragonFruit_Planting] B on A.ID=B.LocationID
                    WHERE LocationType='district' AND ParentID=@ParentID
                    ORDER BY ID";
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@ParentID", SqlDbType.NVarChar).Value = ParentID;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            if (zMessage.Length > 0)
            {
                zItem = new string[4];
                zItem[0] = "ERROR";
                zItem[1] = "zMessage";
                zResult.Add(zItem);
            }
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                zResult.Add(zItem);
            }

            return zResult;
        }
        public static List<string[]> GetProvinces()
        {
            string zMessage = "";
            string zSQL = @" SELECT ID,Name,ParentID,LocationType   
                    FROM [dbo].[SYS_Location] WHERE LocationType='province'
                    ORDER BY ID";
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            if (zMessage.Length > 0)
            {
                zItem = new string[4];
                zItem[0] = "ERROR";
                zItem[1] = "zMessage";
                zResult.Add(zItem);
            }
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                zResult.Add(zItem);
            }

            return zResult;
        }
        public static List<string[]> GetDistricts()
        {
            string zMessage = "";
            string zSQL = @" SELECT ID,Name,ParentID,LocationType   
                    FROM [dbo].[SYS_Location] WHERE LocationType='district'
                    ORDER BY ID";
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                zMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            if (zMessage.Length > 0)
            {
                zItem = new string[4];
                zItem[0] = "ERROR";
                zItem[1] = "zMessage";
                zResult.Add(zItem);
            }
            int n = zTable.Columns.Count;
            foreach (DataRow zRow in zTable.Rows)
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = zRow[i].ToString();
                }
                zResult.Add(zItem);
            }

            return zResult;
        }
        public static List<string[]> DataReportByWard(int InYear, string District)
        {
            List<string[]> zResult = new List<string[]>();
            List<string[]> zWards = GetWardByDistrict(InYear,District);

            foreach (string[] zInfo in zWards)
            {
                string[] zWardInfo = new string[15];
                double zTotal = 0;
                zWardInfo[0] = zInfo[0];
                zWardInfo[1] = zInfo[1];
                double[] zQuantity = HarvestOfWard(InYear, zInfo[0]);
                for (int i = 0; i < 12; i++)
                {
                    zWardInfo[i + 2] = zQuantity[i].ToString("#,###,##0");
                    zTotal += zQuantity[i];
                }
                zWardInfo[14] = zTotal.ToString("#,###,##0");
                zResult.Add(zWardInfo);
            }
            return zResult;
        }
        public static List<string[]> DataAcreageByWard(int InYear, string District)
        {
            List<string[]> zResult = new List<string[]>();
            List<string[]> zWards = GetWardByDistrict(InYear, District);

            foreach (string[] zInfo in zWards)
            {
                string[] zWardInfo = new string[15];
                double zTotal = 0;
                zWardInfo[0] = zInfo[0];
                zWardInfo[1] = zInfo[1];
                double[] zQuantity = AcreageOfWard(InYear, zInfo[0]);
                for (int i = 0; i < 12; i++)
                {
                    zWardInfo[i + 2] = zQuantity[i].ToString("#,###,##0");
                    zTotal += zQuantity[i];
                }
                zWardInfo[14] = zTotal.ToString("#,###,##0");
                zResult.Add(zWardInfo);
            }
            return zResult;
        }
        
        public static List<string[]> GetDevelopProcessByWard(int InYear, string Ward)
        {
            List<string[]> zResult = new List<string[]>();
            List<string[]> zProcess = GetProcessByWard(InYear, Ward);
            string zSQL = @"SELECT InMonth, PlantingPeriod1_GeoJson, PlantingPeriod2_GeoJson, 
                            PlantingPeriod3_GeoJson, PlantingPeriod4_GeoJson
                            FROM[dbo].[AGR_DragonFruit_Planting]
                            WHERE InYear = @InYear AND LocationID = @Ward ";

            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@InYear", SqlDbType.Int).Value = InYear;
                zCommand.Parameters.Add("@Ward", SqlDbType.NVarChar).Value = Ward;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            foreach (DataRow row in zTable.Rows)
            {
                // Tạo một mảng chuỗi để lưu trữ dữ liệu của mỗi dòng
                string[] rowData = new string[zTable.Columns.Count];

                // Duyệt qua từng cột trong dòng và gán giá trị vào mảng rowData
                for (int i = 0; i < zTable.Columns.Count; i++)
                {
                    rowData[i] = row[i].ToString();
                }

                // Thêm mảng chuỗi này vào danh sách kết quả
                zResult.Add(rowData);
            }
            return zResult;
        }
        public static double[] HarvestOfWard(int InYear, string DepartmentID)
        {
            string zSQL = @" SELECT InMonth, Sum(Harvest) AS Harvest  FROM [dbo].[AGR_DragonFruit_Planting]
                             WHERE InYear = @InYear AND LocationID = @DepartmentID  
                              GROUP BY InMonth ORDER BY InMonth";

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
                zResult[zMonth - 1] = double.Parse(zRow["Harvest"].ToString());
            }
            return zResult;
        }
        public static double[] AcreageOfWard(int InYear, string DepartmentID)
        {
            string zSQL = @" SELECT InMonth, Sum(Planting_Area) AS Planting_Area  FROM [dbo].[AGR_DragonFruit_Planting]
                             WHERE InYear = @InYear AND LocationID = @DepartmentID  
                              GROUP BY InMonth ORDER BY InMonth";

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
                zResult[zMonth - 1] = double.Parse(zRow["Planting_Area"].ToString());
            }
            return zResult;
        }

    }







}
