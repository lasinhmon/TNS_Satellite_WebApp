using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using static TNS_AGR.Areas.DragonFruit.Pages.CropSeasonModel;

namespace TNS_AGR.Areas.DragonFruit.Pages
{
    [IgnoreAntiforgeryToken]
    public class LocationModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private int _PageSize = 25;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion
        public void OnGet()
        {
            CheckAuth();
        }
        public IActionResult OnPostLoadLocationInfo([FromBody] ItemInfo request)
        {
            CheckAuth();
            string zMessageLog = "";

            string zLocationID = request.LocationID;
            List<string[]> zData = Models.AccessData.GetLocationInfo(zLocationID);
            return new JsonResult(zData);
        }
        public IActionResult OnPostLoadData([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            DataTable zListNotice = new DataTable();
            ItemView zItem = new ItemView();
            int zPageNumber = 1;
            int zTotal_Record = 0;
            int zTotal_PageNumber = 0;
            string[] zDateConvert;
            if (request.PageSelected.Trim().Length > 0)
            {
                zPageNumber = int.Parse(request.PageSelected.Substring(4, request.PageSelected.Length - 4));
            }
            //string zListField = " fileid,term,device,trandate, issfiid,mcc";
            string zListField = "ID,Name,ValuePath,ParentID,LocationType,ROI ";
            string zWhere = "";
            if (request.SearchContent.Trim().Length > 0)
                zWhere += " WHERE  Name='" + request.SearchContent + "'";
            string zSQL = "SELECT ID,Name,ValuePath,ParentID,LocationType,ROI " +
                "FROM [dbo].[SYS_Location] " + zWhere + " ORDER BY ID";


            string zSQL_Count = "SELECT Count(*) FROM [dbo].[SYS_Location] " + zWhere;


            zItem.FieldsName = new List<string>();
            zItem.FieldsNote = new List<string>();
            string[] zTempName = zListField.Split(',');
            for (int i = 0; i < zTempName.Length; i++)
            {
                zItem.FieldsName.Add(zTempName[i]);
            }

            zTotal_Record = Models.Categories_AccessData.DataCountLocation(zSQL_Count);
            zTotal_PageNumber = zTotal_Record / _PageSize;
            if (zTotal_Record % _PageSize != 0)
                zTotal_PageNumber++;

            zItem.Info = new ItemInfo();
            zItem.Info.LocationName = "OK";
            zItem.Info.PageSize = _PageSize.ToString();
            zItem.Info.PageNumber = zPageNumber.ToString();
            zItem.Info.PageTotal = zTotal_PageNumber.ToString();

            zItem.DataOfTable = Models.Categories_AccessData.DataOfLocation(zSQL, zPageNumber, _PageSize);

            return new JsonResult(zItem);
        }
        public IActionResult OnPostUpdateROI([FromBody] ItemInfo itemUpdate)
        {
            string zMessageLog = "";
            Models.Location location_Model=new Models.Location();
            location_Model.ID = itemUpdate.LocationID;
            location_Model.ROI = itemUpdate.ROI;
            zMessageLog = location_Model.Save();
            return new JsonResult(itemUpdate);
        }
        public class ItemRequest
        {
         //   public string MonthView { get; set; }
        //    public string YearView { get; set; }
            public string SearchContent { get; set; }
            public string PageSelected { get; set; }
            public string[] checkedValues { get; set; }
            public string Key { get; set; }
        }
        public class ItemInfo
        {
            public string LocationID { get; set; }
            public string LocationName { get; set; }
            public string ParentID { get; set; }
            public string LocationType { get; set; }
            public string ValuePath { get; set; }
            public string ROI { get; set; }
            public string PageSize { get; set; }
            public string PageNumber { get; set; }
            public string PageTotal { get; set; }
            public ItemInfo() { }

        }
        public class ItemView
        {
         
            public ItemInfo Info { get; set; }
            public List<string> FieldsName { get; set; }
            public List<string> FieldsNote { get; set; }
            public List<string[]> DataOfTable { get; set; }
            public string Code { get; set; }
            public ItemView() { }
        }
    }
}
