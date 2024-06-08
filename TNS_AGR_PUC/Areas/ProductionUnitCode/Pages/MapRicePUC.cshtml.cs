using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using Google.Cloud.BigQuery.V2;
namespace TNS_AGR.Areas.ProductionUnitCode.Pages
{
    [IgnoreAntiforgeryToken]
    public class MapRicePUCModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion
        public string Message { get; private set; }
        private int _PageSize = 25;
        public void OnGet(string styleView)
        {
          //  string tam = styleView;
            CheckAuth();
            if (TempData.ContainsKey("Message"))
            {
                Message = TempData.Peek("Message").ToString();
       // If you want to remove the value from TempData after retrieving it

             //   TempData.Remove("Message");
           }
        }
       
        public IActionResult OnPostViewGrowth([FromBody] ItemRequest request)
        {
            TempData["Message"] = request.Key;
            // return RedirectToPage("/MapRicePUC");
            return new JsonResult(new { redirectUrl = Url.Page("GrowthOfRice") });
        }
        ItemView zItemtam = new ItemView();
        public IActionResult OnPostLoadData([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            ItemView zItem = new ItemView();
            if (TempData.ContainsKey("Message"))
            {
                zMessageLog = TempData.Peek("Message").ToString();
                TempData.Remove("Message");
                var slie = TNS.DBConnection.Connecting.client;
                var table = slie.GetTable("loctroiproject", "LocTroiDataSet", "BigTable");

                int zPageNumber = 1;
                int zTotal_Record = 0;
                int zTotal_PageNumber = 0;
                string[] zDateConvert;
                if (request.PageSelected.Trim().Length > 0)
                {
                    zPageNumber = int.Parse(request.PageSelected.Substring(4, request.PageSelected.Length - 4));
                }
                string zListField = "bt.PUC,a.commodity,a.polygon,a.LongLat,a.surface,a.field,a.PlantAreaID ";
                string zWhere = " WHERE PUC = '" + zMessageLog + "'";
                string zSQL = "SELECT " + zListField + $" FROM {table} bt JOIN UNNEST(bt.PlantArea) AS a" + zWhere + " ORDER BY PlantAreaID DESC";
                string zSQL_Count = $"SELECT Count(*) FROM {table} " + zWhere;


                zItem.FieldsName = new List<string>();
                zItem.FieldsNote = new List<string>();



                string[] zTempName = new string[] { "Mã Vùng Trồng", "Cây trồng", "Polygon", "Vị trí", "Diện tích", "Sản lượng", "PlantAreaID" };

                for (int i = 0; i < zTempName.Length; i++)
                {
                    zItem.FieldsName.Add(zTempName[i]);
                }

                var resultCount = slie.CreateQueryJob(
                sql: zSQL_Count,
                parameters: null,
                options: new QueryOptions { UseQueryCache = false });
                resultCount = resultCount.PollUntilCompleted().ThrowOnAnyError();

                int n = slie.GetQueryResults(resultCount.Reference).Schema.Fields.Count;
                foreach (var row in slie.GetQueryResults(resultCount.Reference))
                {
                    zTotal_Record = Convert.ToInt32(row["f0_"]);
                }

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
                List<string[]> ztam = new List<string[]>();
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

            }

            return new JsonResult(zItem);
        }

        public IActionResult OnPostViewMap([FromBody] ItemRequest request)
        {
            // TempData["Message"] = request.Key;
            // return RedirectToPage("/MapRicePUC");
            return new JsonResult(new { redirectUrl = Url.Page("CropSeasonTest", "Params", new { PUC = request.Key}) });
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
            public ItemInfo()
            {
                Data = new List<string[]>();
            }

        }
        public class ItemView
        {
            public ItemInfo Info { get; set; }
            public string LongLat { get; set; }
            public string Polygon { get; set; }
            public string Surface { get; set; }
            public string Field { get; set; }
            public string Status { get; set; }
            public string Country { get; set; }

            public List<string> FieldsName { get; set; }
            public List<string> FieldsNote { get; set; }
            public List<string[]> DataOfTable { get; set; }
            // public List<string[]> btnAction { get; set; }
            public string Code { get; set; }
            public ItemView() { }

        }

    }
    public class MapRicePUCData
    {
       
    }
}
