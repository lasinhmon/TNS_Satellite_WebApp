using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TNS_AGR.Areas.DragonFruit.Pages
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
        public List<string[]>? DataOfProvinces = Models.AccessData.GetProvinces();
        public void OnGet()
        {
            CheckAuth();
           // DataOfProvinces = Models.AccessData.GetProvinces();
        }
        public IActionResult OnPostLoadService([FromBody] ItemRequest request)
        {
            CheckAuth();
            ItemView zItem = new ItemView();
            string zMessageLog = "";
            //  int zYear = int.Parse(request.YearView);
         //   zItem.DataOfDistinct = Models.AccessData.GetDistrict();

            return new JsonResult(null);
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
