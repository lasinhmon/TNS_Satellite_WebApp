using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data.SqlClient;

namespace TNS.Auth
{
    public class Member_Record
    {
        #region [ Field Name ]
        private string _MemberKey = "";
        private string _MemberID = "";
        private string _LastName = "";
        private string _FirstName = "";
        private string _Message = "";
        #endregion

        #region [ Constructor Get Information ]
        public Member_Record()
        {

        }
        public Member_Record(string MemberKey)
        {
            string zSQL = "SELECT * FROM [dbo].[HRM_Member] WHERE MemberKey = @MemberKey AND RecordStatus != 99 ";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@MemberKey", SqlDbType.UniqueIdentifier).Value = new Guid(MemberKey);
                SqlDataReader zReader = zCommand.ExecuteReader();
                if (zReader.HasRows)
                {
                    zReader.Read();
                    _MemberKey = zReader["MemberKey"].ToString();
                    _MemberID = zReader["MemberID"].ToString();
                    _LastName = zReader["LastName"].ToString();
                    _FirstName = zReader["FirstName"].ToString();
                    _Message = "200 OK";
                }
                else
                {
                    _Message = "404 Not Found";
                }
                zReader.Close();
                zCommand.Dispose();
            }
            catch (Exception Err)
            {
                _Message = "501 " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
        }
      
        #endregion

        #region [ Properties ]
        public string Key
        {
            get { return _MemberKey; }
            set { _MemberKey = value; }
        }
        public string ID
        {
            get { return _MemberID; }
            set { _MemberID = value; }
        }

        public string Name
        {
            get { return _LastName + " " + _FirstName ; }
        }
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
   
       
        public string Code
        {
            get
            {
                if (_Message.Length >= 3)
                    return _Message.Substring(0, 3);
                else return "";
            }
        }
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
        #endregion

        
    }
}
