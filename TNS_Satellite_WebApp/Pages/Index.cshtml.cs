using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TNS_Satellite_WebApp.Pages
{
    public class IndexModel : PageModel
    {
        //public Models.Task_Status TaskReport ;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            CheckAuth();
            //TaskReport = new Models.Task_Status(DateTime.Now.Month,DateTime.Now.Year,_UserLogin.EmployeeKey);
        }

        #region [ Security ]
        private TNS.Auth.UserLogin_Info _UserLogin;
        private void CheckAuth()
        {
            _UserLogin = new TNS.Auth.UserLogin_Info(User);
        }
        #endregion

       
    }
}