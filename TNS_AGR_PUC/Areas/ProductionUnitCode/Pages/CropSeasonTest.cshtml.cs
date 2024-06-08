using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Text;



namespace TNS_AGR.Areas.ProductionUnitCode.Pages
{
    [IgnoreAntiforgeryToken]
    public class CropSeasonTestModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }


        #endregion
        public string _PUC { get; private set; }
        // public string locationId, monthFromValue, yearFromValue, monthToValue, yearToValue;
        public void OnGetParams(string PUC)
        {

            CheckAuth();
            _PUC = PUC;
        }
        public IActionResult OnPostAddModel([FromBody] ItemView request)
        {
            CheckAuth();
            string zMessageLog = "";
            zMessageLog = CropSeasonTestData.Save(request);
            return new JsonResult(zMessageLog);
        }
        public IActionResult OnPostExportTrainingModel([FromBody] ItemView request)
        {
            CheckAuth();
            string zMessageLog = "";

            zMessageLog = Models.AccessData.DataOfTraining(request.LongLat,request.Code);

            return new JsonResult(zMessageLog);
        }
        public IActionResult OnPostLoadData([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            DataTable zListNotice = new DataTable();
            ItemView zItem = new ItemView();
            string[] zDateConvert;
            //    string tam = goR_Model.LocationId;
            string zListField = "ID,Name,ValuePath,ParentID,GeoJson,LocationType,ROI ";
            //string zFieldName = "Username,Ngay Nhap Lieu, Status, Remark ";
            //  string zWhere = " ";
            string zWhere = "where Name='" + request.District + "' and ParentID in(select b.ID from SYS_Location b where Name='" + request.Province + "')";

            string zSQL = "SELECT " + zListField + " FROM [dbo].[SYS_Location] " + zWhere + " ";

            zItem.FieldsName = new List<string>();
            zItem.FieldsNote = new List<string>();



            string[] zTempName = zListField.Split(',');
            for (int i = 0; i < zTempName.Length; i++)
            {
                zItem.FieldsName.Add(zTempName[i]);
            }


            //  select*
            //   from SYS_Location
            //  where Name = 'Châu Thành' and ParentID in(select b.ID from SYS_Location b where Name = 'Long An')

            zItem.Info = new ItemInfo();
            zItem.Info.Name = "OK";

            zItem.DataOfPUC = Models.AccessData.DataOfPUC(zSQL);


            return new JsonResult(zItem);
        }
        public class ItemRequest
        {
            public string LocationID { get; set; }
            public string Country { get; set; }
            public string Commodity { get; set; }
          
            public string SearchContent { get; set; }
            public string Province { get; set; }
            public string District { get; set; }
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
            public ItemInfo()
            {
                Data = new List<string[]>();
            }

        }
        public class ItemView
        {
            public ItemInfo Info { get; set; }
            public string LongLat { get; set; }
            public string Surface { get; set; }
            public string Field { get; set; }
            public string Status { get; set; }
            public string PUC { get; set; }
            public string Polygon { get; set; }
            public string Commodity { get; set; }

            public List<string> FieldsName { get; set; }
            public List<string> FieldsNote { get; set; }
            public List<string[]> DataOfPUC { get; set; }
            // public List<string[]> btnAction { get; set; }
            public string Code { get; set; }
            public ItemView() { }

        }

    }
    public class CropSeasonTestData
    {
        public static string Save(CropSeasonTestModel.ItemView tam)
        {
            string _Message = "";
            string zSQL = "INSERT INTO AGR_Plant_Rice_Area (AutoKey,PUC,Commodity,Polygon,LongLat,Surface,Field) VALUES (newid(),@PUC,'Rice',@Polygon,@LongLat,'','')";
                           ;
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@PUC", SqlDbType.NVarChar).Value = tam.PUC;
                zCommand.Parameters.Add("@Polygon", SqlDbType.NVarChar).Value = tam.Polygon;
                zCommand.Parameters.Add("@LongLat", SqlDbType.NVarChar).Value = tam.LongLat;
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
        public static DataTable GetInfoOfPUC(string AutoKey)
        {
            DataTable zTable = new DataTable();
            string zSQL =   "SELECT AutoKey,LocationID,Country,Commodity,District,Commune,NumberUnit,NameOfUnit,RandomNumber,AddressOfUnit" +
                            ",LongLat,Surface,Field,PUC,Status,DATEOFAPPROVED,DATEOFREAPPROVED,DATEOFEXPIRATION "
                          + " FROM AGR_PUC_Planting "
                          + " WHERE AutoKey = @AutoKey ";
            string zMessage = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.Parameters.Add("@AutoKey", SqlDbType.NVarChar).Value = AutoKey;
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
