using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace TNS_AGR.Areas.ProductionUnitCode.Pages
{
    [IgnoreAntiforgeryToken]
   
    public class MapCommodityModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }


        #endregion
        public string _Province { get; private set; }
        public string _District { get; private set; }
        // public string locationId, monthFromValue, yearFromValue, monthToValue, yearToValue;
        public void OnGetParams(string Province,string District)
        {
          
            CheckAuth();
            _Province = Province;
            _District= District;
        }
        public IActionResult OnPostLoadLongLat([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            DataTable zListNotice = new DataTable();
            ItemView zItem = new ItemView();
            int zPageNumber = 1;
            int zTotal_Record = 0;
            int zTotal_PageNumber = 0;
            string[] zDateConvert;

            string zListField = "a.AutoKey,a.PUC,a.Surface,a.LongLat,a.Commodity ";
            //string zFieldName = "Username,Ngay Nhap Lieu, Status, Remark ";
              string zWhere = " WHERE " + request.LocationID + " ";
        
            string zSQL = "SELECT " + zListField + " FROM [dbo].[AGR_Plant_Rice_Area] a INNER JOIN [dbo].[AGR_PUC_Planting] b ON a.PUC=b.PUC " + zWhere + " ";

            zItem.FieldsName = new List<string>();
            zItem.FieldsNote = new List<string>();

            zItem.Info = new ItemInfo();
            zItem.Info.Name = "OK";


            //   zItem.DataOfTable = Models.AccessData.DataOfTable(zSQL, zPageNumber, _PageSize);
            zItem.DataOfPUC = MapCommodityData.DataOfLongLat(zSQL);


            return new JsonResult(zItem);
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
            string zWhere = "where Name='"+request.District+"' and ParentID in(select b.ID from SYS_Location b where Name='"+request.Province+"')";

            string zSQL = "SELECT " + zListField + " FROM [dbo].[SYS_Location] " + zWhere + " ";

            zItem.FieldsName = new List<string>();
            zItem.FieldsNote = new List<string>();



            string[] zTempName = zListField.Split(',');
            for (int i = 0; i < zTempName.Length; i++)
            {
                zItem.FieldsName.Add(zTempName[i]);
            }

            zItem.Info = new ItemInfo();
            zItem.Info.Name = "OK";

            zItem.DataOfPUC = MapCommodityData.DataOfPUC(zSQL);


            return new JsonResult(zItem);
        }
        public class ItemRequest
        {
            public string LocationID { get; set; }
            public string Country { get; set; }
            public string Commodity { get; set; }
            public string MonthFrom { get; set; }
            public string YearFrom { get; set; }
            public string MonthTo { get; set; }
            public string YearTo { get; set; }
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
            public List<string[]> DataOfPUC { get; set; }
            // public List<string[]> btnAction { get; set; }
            public string Code { get; set; }
            public ItemView() { }

        }
    }
    public class MapCommodityData
    {
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
                        Array.Resize(ref ll, ll.Length + 1);
                        ll[2] = zRow[4].ToString();
                        // Thêm cặp longlat vào List<string[]>
                        zResult.Add(ll);
                    }


                }
            }
            return zResult;
        }
    }
}
