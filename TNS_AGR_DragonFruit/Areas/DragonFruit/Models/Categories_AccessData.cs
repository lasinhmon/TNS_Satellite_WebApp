using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_AGR.Areas.DragonFruit.Models
{
    public class Categories_AccessData
    {
        public static List<string[]> DataOfLocation(string SQL, int PageNumber, int PageSize)
        {
            DataTable zTable = new DataTable();
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            SQL += " OFFSET @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY OPTION(RECOMPILE)";
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                zCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = PageSize;
                zCommand.Parameters.Add("@PageNumber", SqlDbType.Int).Value = PageNumber;
                SqlDataAdapter zAdapter = new SqlDataAdapter(zCommand);
                zAdapter.Fill(zTable);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            List<string[]> zResult = new List<string[]>();
            string[] zItem;
            int n = zTable.Columns.Count;// cột = 5



            foreach (DataRow zRow in zTable.Rows)
            {

                zItem = new string[n];
                for (int i = 0; i < n; i++)
                {

                    zItem[i] = zRow[i].ToString();


                }

                zResult.Add(zItem);

            }
            return zResult;
        }
        public static int DataCountLocation(string SQL)
        {
            int zQuantity = 0;
            string zConnectionString = TNS.DBConnection.Connecting.SQL_MainDatabase;
            try
            {
                SqlConnection zConnect = new SqlConnection(zConnectionString);
                zConnect.Open();
                SqlCommand zCommand = new SqlCommand(SQL, zConnect);
                string zResult = zCommand.ExecuteScalar().ToString();
                int.TryParse(zResult, out zQuantity);
                zCommand.Dispose();
                zConnect.Close();
            }
            catch (Exception ex)
            {
                string zstrMessage = ex.ToString();
            }
            return zQuantity;
        }
    }
}
