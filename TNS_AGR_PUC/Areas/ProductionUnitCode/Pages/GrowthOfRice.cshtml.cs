using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace TNS_AGR.Areas.ProductionUnitCode.Pages
{
    [IgnoreAntiforgeryToken]
    public class GrowthOfRiceModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion
        public string Message { get; set; }
        private int _PageSize = 25;
        public void OnGet(string styleView)
        {
            //  string tam = styleView;
            CheckAuth();
            if (TempData.ContainsKey("Message"))
            {
                Message = TempData.Peek("Message").ToString();
            }
        }
       
        public IActionResult OnPostLoadData([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            ItemView zItem = new ItemView();
            if (TempData.ContainsKey("Message"))
            {
                zMessageLog = TempData.Peek("Message").ToString();
                //     Message = zMessageLog;
                //    TempData.Remove("Message");

                int zPageNumber = 1;
                int zTotal_Record = 0;
                int zTotal_PageNumber = 0;
                string[] zDateConvert;
                if (request.PageSelected.Trim().Length > 0)
                {
                    zPageNumber = int.Parse(request.PageSelected.Substring(4, request.PageSelected.Length - 4));
                }
                string zListField = "PlantAreaID,Commodity,DATESOFGROWTH,Surface,Field,Status ";
                string zWhere = " WHERE PlantAreaID = '" + zMessageLog + "'";
                string zSQL = "SELECT " + zListField + " FROM [dbo].[AGR_Growth_Of_Rice] " + zWhere + " ORDER BY DATESOFGROWTH ASC";
                string zSQL_Count = "SELECT Count(*) FROM [dbo].[AGR_Growth_Of_Rice] " + zWhere;


                zItem.FieldsName = new List<string>();
                zItem.FieldsNote = new List<string>();



                string[] zTempName = zListField.Split(',');
                for (int i = 0; i < zTempName.Length; i++)
                {
                    zItem.FieldsName.Add(zTempName[i]);
                }

                zTotal_Record = Models.AccessData.DataCount(zSQL_Count);
                zTotal_PageNumber = zTotal_Record / _PageSize;
                if (zTotal_Record % _PageSize != 0)
                    zTotal_PageNumber++;

                zItem.Info = new ItemInfo();
                zItem.Info.Name = "OK";
                zItem.Info.PageSize = _PageSize.ToString();
                zItem.Info.PageNumber = zPageNumber.ToString();
                zItem.Info.PageTotal = zTotal_PageNumber.ToString();



                zItem.DataOfTable = Models.AccessData.DataOfTable(zSQL, zPageNumber, _PageSize);
            }
            return new JsonResult(zItem);
        }

        public IActionResult OnPostSave([FromBody] ItemRequest request)
        {
            string zMessageLog = "";
            if (TempData.ContainsKey("Message"))
            {
                zMessageLog = TempData.Peek("Message").ToString();
            }
                Models.GrowthOfRiceModel goR_Model = new Models.GrowthOfRiceModel();
            goR_Model.PlantAreaID = zMessageLog;
            goR_Model.Commodity = request.Commodity;
            goR_Model.DATESOFGROWTH = request.DATESOFGROWTH;
            goR_Model.Surface = request.Surface;
            goR_Model.Field = request.Field;
            goR_Model.Status = request.Status;
            zMessageLog = goR_Model.Save();
            request.Key = zMessageLog;
            return new JsonResult(request);
        }
        public class ItemRequest
        {
            
            public string Commodity { get; set; }
            public string Surface { get; set; }
            public string Field { get; set; }
            public string Status { get; set; }
            public string DATEBEGINSEASON { get; set; }
            public string DATESOFGROWTH { get; set; }
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
            public string Surface { get; set; }
            public string Field { get; set; }
            
            public string DATEBEGINSEASON { get; set; }
            public string DATESOFGROWTH { get; set; }
        
            public List<string> FieldsName { get; set; }
            public List<string> FieldsNote { get; set; }
            public List<string[]> DataOfTable { get; set; }
            // public List<string[]> btnAction { get; set; }
            public string Code { get; set; }
            public ItemView() { }

        }
    }
}
