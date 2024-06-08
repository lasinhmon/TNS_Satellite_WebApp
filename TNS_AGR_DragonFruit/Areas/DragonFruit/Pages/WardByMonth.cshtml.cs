using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TNS_AGR.Areas.DragonFruit.Pages
{
    [IgnoreAntiforgeryToken]
    public class WardByMonthModel : PageModel
    {
        #region [ Security ]
        public TNS.Auth.UserLogin_Info UserLogin;
        private void CheckAuth()
        {
            UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion
        public List<string[]>? DataOfProvinces = Models.AccessData.GetProvinces();
        public void OnGet()
        {
            CheckAuth();
        }
        public IActionResult OnPostLoadService([FromBody] ItemRequest request)
        {
            CheckAuth();
            int zYearView = int.Parse(request.YearView);
            string zDistrict = request.LocationID;
            List<string[]> zData = Models.AccessData.DataReportByWard(zYearView, zDistrict);
            return new JsonResult(zData);
        }
        public IActionResult OnPostCollectData([FromBody] ItemRequest request)
        {
            CheckAuth();
            string zMessageLog = "";
            string zProvince = request.LocationID;
            List<string[]> zListDistrict = Models.AccessData.GetDistrictByProvince(zProvince);

            return new JsonResult(zListDistrict);
        }
        public class ItemRequest
        {
            public string YearView { get; set; }
            public string LocationID { get; set; }
            public string LocationName { get; set; }
            public string ParentID { get; set; }
            public string LocationType { get; set; }
        }
        public class ItemView
        {
            public List<string[]> DataOfTable { get; set; }

            public string Code { get; set; }
            public ItemView() { }
        }
    }
}
