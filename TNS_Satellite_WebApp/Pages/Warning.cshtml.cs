using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TNS_Satellite_WebApp.Pages
{
    public class WarningModel : PageModel
    {
        public string Message { get;set; }
        public void OnGet(int id)
        {
            switch (id)
            {
                case 300:
                    Message = "Thông tin không chính xác, vui lòng kiểm tra lại !";
                    break;
                case 403:
                    Message = "BẠN KHÔNG CÓ QUYỀN XEM THÔNG TIN NÀY !";
                    break;
                case 404:
                    Message = "BẠN KHÔNG CÓ QUYỀN THÊM THÔNG TIN NÀY!";
                    break;
                case 405:
                    Message = "BẠN KHÔNG CÓ QUYỀN THAY ĐỔI THÔNG TIN NÀY!";
                    break;
                case 406:
                    Message = "BẠN KHÔNG CÓ QUYỀN XÓA THÔNG TIN NÀY!";
                    break;
                case 407:
                    Message = "BẠN KHÔNG CÓ QUYỀN DUYỆT THÔNG TIN NÀY!";
                    break;
            }
        }
    }
}
