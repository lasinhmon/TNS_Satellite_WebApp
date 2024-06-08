using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TNS.Auth
{
    public class UserLogin_Info
    {
        public string UserKey { get; private set; }
        public string UserName { get; private set; }
        public Member_Record Member { get; private set; }
        public Role_Info Role { get; private set; }
        public UserLogin_Info(ClaimsPrincipal UserCookie)
        {

            UserKey = UserCookie.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            UserName = UserCookie.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value;
            string zMemberKey = UserCookie.Claims.FirstOrDefault(c => c.Type == "MemberKey")?.Value;
            Member = new Member_Record(zMemberKey);
        }
        public void GetRole(string RoleID)
        {
            Role = new TNS.Auth.Role_Info(UserKey, RoleID);
        }
    }
}
