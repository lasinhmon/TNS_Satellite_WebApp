using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Reflection;

namespace TNS_AGR.Areas.ProductionUnitCode.Pages
{
    [IgnoreAntiforgeryToken]
    public class MapPolygonPUCModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion
        private int _PageSize = 25;
        public List<string[]>? DataOfProvinces = Models.AccessData.GetProvinces();
        public void OnGet(string styleView)
        {
            CheckAuth();

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
          
            string zListField = "a.AutoKey,a.PUC,a.Commodity,a.Polygon,a.LongLat,a.Surface,a.Field ";
            //string zFieldName = "Username,Ngay Nhap Lieu, Status, Remark ";
              string zWhere = "WHERE " + request.LocationID + " and DATALENGTH(a.Polygon) > 0 ";
          //  string zWhere = " WHERE " + request.LocationID + " AND (month(DATEOFAPPROVED) between " + request.MonthFrom + " and " + request.MonthTo + ") AND(year(DATEOFAPPROVED) between " + request.YearFrom + " and " + request.YearTo + ") AND DATALENGTH(LongLat) > 0";
            if (request.Commodity.Trim().Length > 0)
            {
                zWhere += " AND Commodity='" + request.Commodity + "'";
            }
            if (request.SearchContent.Trim().Length > 0)
                zWhere += " AND a.PUC='" + request.SearchContent + "'";
            string zSQL = "SELECT " + zListField + " FROM [dbo].[AGR_Plant_Rice_Area] a INNER JOIN [dbo].[AGR_PUC_Planting] b ON a.PUC=b.PUC " + zWhere + " ";
           
            zItem.FieldsName = new List<string>();
            zItem.FieldsNote = new List<string>();



            string[] zTempName = zListField.Split(',');
            for (int i = 0; i < zTempName.Length; i++)
            {
                zItem.FieldsName.Add(zTempName[i]);
            }

           

            zItem.Info = new ItemInfo();
            zItem.Info.Name = "OK";
        

         //   zItem.DataOfTable = Models.AccessData.DataOfTable(zSQL, zPageNumber, _PageSize);
            zItem.DataOfPUC = Models.AccessData.DataOfPolygon(zSQL);


            return new JsonResult(zItem);
        }
        public IActionResult OnPostCollectData([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            string zProvince = request.LocationID;
            List<string[]> zListDistrict = Models.AccessData.GetDistrictByProvince(zProvince);

            return new JsonResult(zListDistrict);
        }
        public IActionResult OnPostCollectCommodity([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            string zProvince = request.LocationID;
            List<string[]> zList = Models.AccessData.GetCommodityByDistrict(zProvince);

            return new JsonResult(zList);
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
            public List<string[]> DataOfPUC { get; set; }
            // public List<string[]> btnAction { get; set; }
            public string Code { get; set; }
            public ItemView() { }

        }
    }
}
