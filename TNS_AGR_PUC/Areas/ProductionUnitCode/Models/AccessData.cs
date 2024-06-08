using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using Newtonsoft.Json.Linq;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TNS_AGR_PUC.Areas.ProductionUnitCode.Models;

namespace TNS_AGR.Areas.ProductionUnitCode.Models
{
    public class AccessData
    {
        public static List<string[]> GetCommodityByDistrict(string District)
        {
            string zMessage = "";
           // string zSQL = @" SELECT distinct Commodity, Country   
             //       FROM [dbo].[AGR_PUC_Planting] 
               //     WHERE District=@District
                 //   ORDER BY Commodity";
            string zSQL = @"SELECT Commodity, Country, COUNT (*)   
                    FROM [dbo].[AGR_PUC_Planting] 
                    WHERE LocationName=@LocationID
                    GROUP BY Commodity, Country
                    ORDER BY Commodity";
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@LocationID", SqlDbType.NVarChar).Value = District;
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
                    LEFT JOIN [dbo].[AGR_DragonFruit_Planting] B on A.ID=B.LocationID
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
        public static string DataOfTraining(string LONGLAT,string code)
        {
            //string filePath = code;
            DataTable tbl = new DataTable();

            for (int col = 0; col < 3; col++)
                tbl.Columns.Add(new DataColumn("Column" + (col + 1).ToString()));
            string filePath = "D:\\Thuctap_c#\\LocTroi\\New folder\\TNS_Satellite\\TNS_Satellite_WebApp\\";
            filePath = filePath + code;
            string[] lines = System.IO.File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                var cols = line.Split('|');

                DataRow dr = tbl.NewRow();
                for (int cIndex = 0; cIndex < 3; cIndex++)
                {
                    dr[cIndex] = cols[cIndex];
                }

                tbl.Rows.Add(dr);
            }
            int index = lines.Length - 1;
            StringBuilder csvContent = new StringBuilder();
            string longlat = LONGLAT;
            string[] parts = longlat.Trim().Split(',');
            if (parts.Length == 2 && double.TryParse(parts[0], out double latitude) && double.TryParse(parts[1], out double longitude))
            {
                string geoJson = "{\"type\":\"Point\",\"coordinates\":[" + latitude.ToString() + "," + longitude.ToString() + "]}";

                // Add the JSON object to the CSV content, ensuring it's properly quoted
                csvContent.AppendLine($"{index}|0|{geoJson}");
            }

            File.AppendAllText(filePath, csvContent.ToString());

            /*   File.AppendAllText(filePath, csvContent.ToString());

               StringBuilder csvContent = new StringBuilder();
               string filePath = "D:\\Thuctap_c#\\New folder (2)\\Water_csv.csv";
               csvContent.AppendLine("system:index|LC|.geo");

                int currentIndex = 0;
                foreach (string line in zItem)
                {
                    // Split each line into latitude and longitude
                    string[] parts = line.Trim().Split(',');
                    if (parts.Length == 2 && double.TryParse(parts[0], out double latitude) && double.TryParse(parts[1], out double longitude))
                    {
                        string geoJson = "{\"type\":\"Point\"coordinates\":["+longitude.ToString()+","+latitude.ToString()+"\"}";

                        // Add the JSON object to the CSV content, ensuring it's properly quoted
                        csvContent.AppendLine($"{currentIndex}|0|{geoJson}");
                        currentIndex++;
                    }
                }


               File.AppendAllText(filePath, csvContent.ToString());*/
            return null; // Return a success response
         //   return zResult;
        }
        public static List<string[]> DepartmentForReport()
        {
            string zMessage = "";
            string zSQL = @" SELECT DepartmentID, DepartmentName, TRCD   
                    FROM [dbo].[HRM_Department] where ForReport ='KPI' AND Class = 'DP' 
                    ORDER BY Rank ASC";
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
        public static List<string[]> DataOfPUC(string SQL)
        {
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
         //   SQL += " OFFSET @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY OPTION(RECOMPILE)";
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
               
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
            string[] zItem;
            int n = zTable.Columns.Count;// cột = 5



            foreach (DataRow zRow in zTable.Rows)
            {
                // Check if the values in columns 2 and 5 are not null or empty
                if (!string.IsNullOrEmpty(zRow[4].ToString()))
                {
                    zItem = new string[n];
                    for (int i = 0; i < n; i++)
                    {
                        string tam = zRow[i].ToString();
                        // You can add additional processing here if needed
                        zItem[i] = tam;
                    }
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        public static List<string[]> DataOfPolygon(string SQL)
        {
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            //   SQL += " OFFSET @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY OPTION(RECOMPILE)";
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);

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
            string[] zItem;
            int n = zTable.Columns.Count;// cột = 5



            foreach (DataRow zRow in zTable.Rows)
            {
                // Check if the values in columns 2 and 5 are not null or empty
                if (!string.IsNullOrEmpty(zRow[3].ToString()))
                {
                 //   string mang = zRow[4].ToString();
                 //   string[] longLat = mang.Split(';');
                    zItem = new string[n];
                    for (int i = 0; i < n; i++)
                    {
                        string tam = zRow[i].ToString();
                        // You can add additional processing here if needed
                        zItem[i] = tam;
                    }
                    zResult.Add(zItem);
                }
            }
            return zResult;
        }
        public static List<string[]> DataOfLongLat(string SQL)
        {
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            //   SQL += " OFFSET @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY OPTION(RECOMPILE)";
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);

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
            string[] zItem;
            int n = zTable.Columns.Count;// cột = 5


            foreach (DataRow zRow in zTable.Rows)
            {
                // Check if the values in columns 2 and 5 are not null or empty
                if (!string.IsNullOrEmpty(zRow[3].ToString()))
                {
                    string tam = zRow[3].ToString();
                    string[] longLat = tam.Split(';');
                    foreach (string pair in longLat)
                    {
                        // Tách cặp longlat thành long và lat bằng dấu ','
                        string[] ll = pair.Split(',');
                        Array.Resize(ref ll, ll.Length+1);
                        ll[2]= zRow[4].ToString();
                        // Thêm cặp longlat vào List<string[]>
                        zResult.Add(ll) ;
                    }

                  
                }
            }
            return zResult;
        }
        public static List<string[]> DataOfTable(string SQL, int PageNumber, int PageSize)
        {
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SQL += " OFFSET @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY OPTION(RECOMPILE)";
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                zCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = PageSize;
                zCommand.Parameters.Add("@PageNumber", SqlDbType.Int).Value = PageNumber;
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
            string[] zItem;
            int n = zTable.Columns.Count;// cột = 5



            foreach (DataRow zRow in zTable.Rows)
            {

                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    //if(zRow[i])
                    string tam = zRow[i].ToString();
                  //  if (tam.Length > 100)
                  //  {
                    //    tam = tam.Substring(0, 100);
                      //  tam = tam + "...";
                   // }
                    zItem[i] = tam;
                }

                zResult.Add(zItem);

            }
            return zResult;
        }
        public static int DataCount(string SQL)
        {
            int zQuantity = 0;
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                string zResult = zCommand.ExecuteScalar().ToString();
                int.TryParse(zResult, out zQuantity);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            return zQuantity;
        }

    }







}
