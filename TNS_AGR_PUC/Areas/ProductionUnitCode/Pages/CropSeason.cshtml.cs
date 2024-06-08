using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.SqlServer.Server;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using Google.Cloud.BigQuery.V2;
using System.Text.RegularExpressions;
using Google.Apis.Bigquery.v2.Data;
using Microsoft.AspNetCore.Http;
using System.Drawing.Printing;

namespace TNS_AGR.Areas.ProductionUnitCode.Pages
{
    [IgnoreAntiforgeryToken]
    public class CropSeasonModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion
        private int _PageSize = 25;
        public List<string[]>? DataOfProvinces = CropSeasonData.GetProvinces();
        public void OnGet(string styleView)
        {
            CheckAuth();
           
        }
      
        public IActionResult OnPostViewPUC([FromBody] ItemRequest request)
        {
             TempData["Message"] = request.Key;
            // return RedirectToPage("/MapRicePUC");
            return new JsonResult(new { redirectUrl = Url.Page("MapRicePUC" ) });
        }
        public IActionResult OnPostViewMap([FromBody] ItemRequest request)
        {
            // TempData["Message"] = request.Key;
            // return RedirectToPage("/MapRicePUC");
            return new JsonResult(new { redirectUrl = Url.Page("MapCommodity", "Params", new { Province=request.Province,District = request.District}) });
        }
        public IActionResult OnPostLoadUpdate([FromBody] ItemView request)
        {
            CheckAuth();
            string zMessageLog = "";
            zMessageLog = CropSeasonData.Update(request);
            return new JsonResult(zMessageLog);
        }
        public IActionResult OnPostLoadInfo([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            int zMonth, zYear;
            int zAmountRecordDisplay;
            DataTable zDataInfo = CropSeasonData.GetInfoOfPUC(request.Key);
            ItemInfo zItem = new ItemInfo();
            string[] zItemData;
            foreach (DataRow zRow in zDataInfo.Rows)
            {
                zItemData = new string[20];

                zItemData[0] = zRow["AutoKey"].ToString();
                zItemData[1] = zRow["LocationName"].ToString();
                zItemData[2] = zRow["Country"].ToString();
                zItemData[3] = zRow["Commodity"].ToString();
                zItemData[4] = zRow["District"].ToString();
                zItemData[5] = zRow["Commune"].ToString();
                zItemData[6] = zRow["NumberUnit"].ToString();
                zItemData[7] = zRow["NameOfUnit"].ToString();
                zItemData[8] = zRow["RandomNumber"].ToString();
                zItemData[9] = zRow["AddressOfUnit"].ToString();
                zItemData[10] = zRow["LngLat"].ToString();
                zItemData[11] = zRow["Surface"].ToString();
                zItemData[12] = zRow["Field"].ToString();
                zItemData[13] = zRow["PUC"].ToString();
                zItemData[14] = zRow["Status"].ToString();
                zItemData[15] = zRow["DATEOFAPPROVED"].ToString();
                zItemData[16] = zRow["DATEOFREAPPROVED"].ToString();
                zItemData[17] = zRow["DATEOFEXPIRATION"].ToString();
                zItem.Data.Add(zItemData);
            }
            return new JsonResult(zItem);
        }
        public IActionResult OnPostLoadData([FromBody] ItemRequest request)
        {
            CheckAuth();
            var slie = TNS.DBConnection.Connecting.client;
            var table = slie.GetTable("loctroiproject", "LocTroiDataSet", "BigTable");


            ItemView zItem = new ItemView();
            int zPageNumber = 1;
            int zTotal_Record = 0;
            int zTotal_PageNumber = 0;
            string[] zDateConvert;
            if (request.PageSelected.Trim().Length > 0)
            {
                zPageNumber = int.Parse(request.PageSelected.Substring(4, request.PageSelected.Length - 4));
            }
            string zListField = "autokey,puc,NameOfUnit,AddressOfUnit,commodityPUC,surfacePUC, fieldPUC,Status, DATEOFAPPROVED, Country ";
            //string zFieldName = "Username,Ngay Nhap Lieu, Status, Remark ";
          //  string zWhere = " ";
            string zWhere = " WHERE " + request.LocationID + " ";
            if(request.Commodity.Trim().Length > 0)
            {
                zWhere += " AND Commodity='" + request.Commodity + "'";
            }
            if (request.Country.Trim().Length > 0)
            {
                zWhere += " AND Country='" + request.Country + "'";
            }
            if (request.Status.Trim().Length > 0)
            {
                zWhere += " AND Status='" + request.Status + "'";
            }
            if (request.SearchContent.Trim().Length > 0)
                zWhere += " AND PUC='" + request.SearchContent + "'";
            string zSQL = "SELECT " + zListField + $" FROM {table} " + zWhere + " ORDER BY NameOfUnit DESC";
            string zSQL_Count = $"SELECT Count(*) FROM {table} " + zWhere;


            zItem.FieldsName = new List<string>();
            zItem.FieldsNote = new List<string>();



            // string[] zTempName = zListField.Split(',');
            string[] zTempName = new string[] { "AutoKey", "Mã Vùng Trồng", "Tên đại diện", "Địa chỉ", "Cây trồng", "Diện tích", "Sản lượng", "Tình trạng", "Ngày đăng ký", "Quốc gia xuất khẩu" };
            for (int i = 0; i < zTempName.Length; i++)
            {
                zItem.FieldsName.Add(zTempName[i]);
            }
            var resultCount=slie.CreateQueryJob(
                sql: zSQL_Count,
                parameters:null,
                options: new QueryOptions { UseQueryCache = false });
            resultCount = resultCount.PollUntilCompleted().ThrowOnAnyError();

            int n = slie.GetQueryResults(resultCount.Reference).Schema.Fields.Count;
            foreach (var row in slie.GetQueryResults(resultCount.Reference))
            {
                zTotal_Record = Convert.ToInt32(row["f0_"]);
            }




          //  zTotal_Record = CropSeasonData.DataCount(zSQL_Count);
            zTotal_PageNumber = zTotal_Record / _PageSize;
            if (zTotal_Record % _PageSize != 0)
                zTotal_PageNumber++;

            zItem.Info = new ItemInfo();
            zItem.Info.Name = "OK";
            zItem.Info.PageSize = _PageSize.ToString();
            zItem.Info.PageNumber = zPageNumber.ToString();
            zItem.Info.PageTotal = zTotal_PageNumber.ToString();


            zSQL += " limit " + _PageSize + " offset " + (zPageNumber - 1) * _PageSize + "";
        //    zSQL += " OFFSET "+ _PageSize + " * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY OPTION(RECOMPILE)";

            var resultPUC = slie.CreateQueryJob(
              sql: zSQL,
              parameters: null,
              options: new QueryOptions { UseQueryCache = false });
            resultPUC = resultPUC.PollUntilCompleted().ThrowOnAnyError();
            List<string[]> ztam=new List<string[]>();
            string[] zIte;
            int length = slie.GetQueryResults(resultPUC.Reference).Schema.Fields.Count;
            foreach (BigQueryRow row in slie.GetQueryResults(resultPUC.Reference))
            {
                zIte = new string[length];
                for (int i = 0; i < length; i++)
                {
                    zIte[i] = row[i].ToString();
                }
                ztam.Add(zIte);
            }


            zItem.DataOfTable = ztam;


            return new JsonResult(zItem);
        }
        public IActionResult OnPostCollectData([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            string zProvince = request.LocationID;
            List<string[]> zListDistrict = CropSeasonData.GetDistrictByProvince(zProvince);

            return new JsonResult(zListDistrict);
        }
        public IActionResult OnPostCollectCommodity([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            string zProvince = request.LocationID;
            List<string[]> zList = CropSeasonData.GetCommodityByDistrict(zProvince);

            return new JsonResult(zList);
        }
        public class ItemRequest
        {
            public string LocationID { get; set; }
            public string Province { get; set; }
            public string District { get; set; }
            public string Country { get; set; }
            public string Commodity { get; set; }
            public string Status { get; set; }
            public string SearchContent { get; set; }
            public string PageSelected { get; set; }
            public string Key { get; set; }
            public string StatusView { get; set; }
        }
        public class ItemInfo
        {
            public string Key { get; set; }
            public string Name { get; set; }
            public List<string[]> Data { get; set; }
            public string PageSize { get; set; }
            public string PageNumber { get; set; }
            public string PageTotal { get; set; }
            public string Content { get; set; }
            public ItemInfo() {
                Data = new List<string[]>();
            }

        }
        public class ItemView
        {
            public ItemInfo Info { get; set; }
            public string District { get; set; }
            public string Commune { get; set; }
            public string NameOfUnit { get; set; }
            public string AddressOfUnit { get; set; }
            public string LongLat { get; set; }
            public string Surface { get; set; }
            public string Field { get; set; }   
            public string Status { get; set; }
            public string DATEOFAPPROVED { get; set; }
            public string DATEOFREAPPROVED { get; set; }
            public string DATEOFEXPIRATION { get; set; }
            public string Country { get; set; }

            public List<string> FieldsName { get; set; }
            public List<string> FieldsNote { get; set; }
            public List<string[]> DataOfTable { get; set; }
            // public List<string[]> btnAction { get; set; }
            public string Code { get; set; }
            public ItemView() { }

        }

    }
    public class CropSeasonData
    {

        public static string Update(CropSeasonModel.ItemView tam)
        {
            string _Message = "";
            string zSQL =   "UPDATE AGR_PUC_Planting " +
                            "SET District = @District, Commune = @Commune, NameOfUnit=@NameOfUnit, AddressOfUnit = @AddressOfUnit ," +
                            "LongLat = @LongLat, Surface = @Surface, Field = @Field, Status = @Status, DATEOFAPPROVED = @DATEOFAPPROVED ," +
                            "DATEOFREAPPROVED = @DATEOFREAPPROVED, DATEOFEXPIRATION = @DATEOFEXPIRATION " +
                            "WHERE AutoKey = @AutoKey";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@District", SqlDbType.NVarChar).Value = tam.District;
                zCommand.Parameters.Add("@Commune", SqlDbType.NVarChar).Value = tam.Commune;
                zCommand.Parameters.Add("@NameOfUnit", SqlDbType.NVarChar).Value = tam.NameOfUnit;
                zCommand.Parameters.Add("@AddressOfUnit", SqlDbType.NVarChar).Value = tam.AddressOfUnit;
                zCommand.Parameters.Add("@LongLat", SqlDbType.NVarChar).Value = tam.LongLat;
                zCommand.Parameters.Add("@Surface", SqlDbType.NVarChar).Value = tam.Surface;
                zCommand.Parameters.Add("@Field", SqlDbType.NVarChar).Value = tam.Field;
                zCommand.Parameters.Add("@Status", SqlDbType.NVarChar).Value = tam.Status;
                zCommand.Parameters.Add("@DATEOFAPPROVED", SqlDbType.NVarChar).Value = tam.DATEOFAPPROVED;
                zCommand.Parameters.Add("@DATEOFREAPPROVED", SqlDbType.NVarChar).Value = tam.DATEOFREAPPROVED;
                zCommand.Parameters.Add("@DATEOFEXPIRATION", SqlDbType.NVarChar).Value = tam.DATEOFEXPIRATION;
                zCommand.Parameters.Add("@AutoKey", SqlDbType.NVarChar).Value = tam.Code;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                _Message = "201 Created";
            }
            catch (Exception Err)
            {
                 _Message = "501 " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return _Message;
        }
        public static DataTable Test()
        {
            DataTable zTable = new DataTable();
            var slie = TNS.DBConnection.Connecting.client;
            //var client = BigQueryClient.Create("loctroiproject");
            var table = slie.GetTable("loctroiproject", "LocTroiDataSet", "PUC"); 
            var sql = $"SELECT LocationID FROM {table} ";
            var results = slie.ExecuteQuery(sql, parameters: null);

            foreach (var row in results)
            {
               string tam=($"{row["LocationID"]}");
            }
            return zTable;
        }
        public static DataTable GetInfoOfPUC(string AutoKey)
        {
            string cleanedString = AutoKey.TrimStart('b', '\'').TrimEnd('\'');
            DataTable zTable = new DataTable();
            string zSQL =   "SELECT AutoKey,LocationName,Country,Commodity,District,Commune,NumberUnit,NameOfUnit,RandomNumber,AddressOfUnit" +
                            ",LngLat,Surface,Field,PUC,Status,DATEOFAPPROVED,DATEOFREAPPROVED,DATEOFEXPIRATION "
                          + " FROM AGR_PUC_Planting "
                          + " WHERE AutoKey = @AutoKey ";
            string zMessage = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@AutoKey", SqlDbType.UniqueIdentifier).Value = new System.Data.SqlTypes.SqlGuid(cleanedString);
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
       
        public static List<string[]> GetCommodityByDistrict(string District)
        {
            string zMessage = "";

            // string zSQL = @"SELECT Commodity, Country,Status,SUM(TRY_CONVERT (float, Surface))   
            //       FROM [dbo].[AGR_PUC_Planting] 
            //     WHERE LocationName=@LocationID
            //   GROUP BY Commodity, Country,Status
            // ORDER BY Commodity";
            var slie = TNS.DBConnection.Connecting.client;
            var table = slie.GetTable("loctroiproject", "LocTroiDataSet", "BigTable");

            string zSQL = $"SELECT CommodityPUC, Country, Status, SUM(CAST(surfacePUC AS FLOAT64)) " +
                          $"FROM {table} AS bt WHERE LocationId = '{District}' " +
                          $"GROUP BY CommodityPUC, Country,Status " +
                          $"ORDER BY commodityPUC";
            var results = slie.CreateQueryJob(
                sql:zSQL, 
                parameters: null,
                options: new QueryOptions { UseQueryCache = false });
            results = results.PollUntilCompleted().ThrowOnAnyError();
            List<string[]> ztam = new List<string[]>();
            string[] zItem;
            int n =slie.GetQueryResults(results.Reference).Schema.Fields.Count;
            foreach (BigQueryRow row in slie.GetQueryResults(results.Reference))
            {
                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {
                    zItem[i] = row[i].ToString();
                }
                ztam.Add(zItem);
            }
            
           
           
            DataTable zTable = new DataTable();
            //   zTable = results;
            /*
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
            }*/

            return ztam;
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
    }
}
