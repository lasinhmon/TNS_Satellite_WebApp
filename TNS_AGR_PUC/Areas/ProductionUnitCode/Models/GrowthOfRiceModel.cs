using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_AGR.Areas.ProductionUnitCode.Models
{
    public class GrowthOfRiceModel
    {
        #region [ Field Name ]
        private string _AutoKey = "";
        private string _PlantAreaID = "";
        private string _Commodity = "";
        private string _DATESOFGROWTH = null;
        private string _Surface = "";
        private string _Field = "";
        private string _Status = "";
        private string _Message = "";
        #endregion
        #region [ Properties ]
        public string AutoKey { get => _AutoKey; set => _AutoKey = value; }
        public string PlantAreaID { get => _PlantAreaID; set => _PlantAreaID = value; }
        public string Commodity { get => _Commodity; set => _Commodity = value; }
        public string DATESOFGROWTH { get => _DATESOFGROWTH; set => _DATESOFGROWTH = value; }
        public string Surface { get => _Surface; set => _Surface = value; }
        public string Field { get => _Field; set => _Field = value; }
        public string Status { get => _Status; set => _Status = value; }

        #endregion
        #region [ Constructor Get Information ]
        public GrowthOfRiceModel()
        {

        }
        public GrowthOfRiceModel(string tam)
        {

        }
        #endregion
        public string Save()
        {
            string zSQL = "INSERT into [dbo].[AGR_Growth_Of_Rice] Values (newid(),@PlantAreaID, @Commodity,@DATESOFGROWTH,@Surface,@Field,@Status)";
            string zResult = "";
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SqlConnection zConnect = new SqlConnection(zConnectionString);
            zConnect.Open();
            try
            {
                SqlCommand zCommand = new SqlCommand(zSQL, zConnect);
                zCommand.CommandType = CommandType.Text;
                zCommand.Parameters.Add("@PlantAreaID", SqlDbType.NVarChar).Value = PlantAreaID;
                zCommand.Parameters.Add("@Commodity", SqlDbType.NVarChar).Value = Commodity;
              
                zCommand.Parameters.Add("@DATESOFGROWTH", SqlDbType.Date).Value = DateTime.ParseExact(DATESOFGROWTH, "yyyy-MM-dd", null);
                zCommand.Parameters.Add("@Surface", SqlDbType.NVarChar).Value = Surface;
                zCommand.Parameters.Add("@Field", SqlDbType.NVarChar).Value = Field;
                zCommand.Parameters.Add("@Status", SqlDbType.NVarChar).Value = Status;
                zResult = zCommand.ExecuteNonQuery().ToString();
                zCommand.Dispose();
                  _Message = "201 Created";
            }
            catch (Exception Err)
            {
                 _Message = "501 " + Err.ToString();
            }
            finally
            {
                zConnect.Close();
            }
            return zResult;
        }
    }
}
