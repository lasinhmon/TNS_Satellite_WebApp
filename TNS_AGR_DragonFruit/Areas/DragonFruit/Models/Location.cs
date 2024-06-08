using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_AGR.Areas.DragonFruit.Models
{
    public class Location
    {
        #region [ Field Name ]
        private string _ID = "";
        private string _Name = "";
        private string _ValuePath = "";
        private string _ParentID = "";
        private string _LocationType = "";
        private string _ROI = "";
        #endregion
        #region [ Properties ]
        public string ID { get => _ID; set => _ID = value; }
        public string Name { get => _Name; set => _Name = value; }
        public string ValuePath { get => _ValuePath; set => _ValuePath = value; }
        public string ParentID { get => _ParentID; set => _ParentID = value; }
        public string LocationType { get => _LocationType; set => _LocationType = value; }
        public string ROI { get => _ROI; set => _ROI = value; }

        #endregion
        #region [ Constructor Get Information ]
        public Location()
        {

        }
        public string Save()
        {
            string zSQL = "UPDATE [dbo].[SYS_Location] set ROI=@ROI where ID=@ID";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@ROI", SqlDbType.NVarChar).Value = ROI;
                zCommand.Parameters.Add("@ID", SqlDbType.NVarChar).Value = ID;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                //  _Message = "201 Created";
            }
            catch (Exception Err)
            {
                // _Message = "501 " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
        #endregion
    }
}
