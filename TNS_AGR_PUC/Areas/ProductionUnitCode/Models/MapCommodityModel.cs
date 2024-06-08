using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_AGR.Areas.ProductionUnitCode.Models
{
    public class MapCommodityModel
    {
        #region [ Field Name ]
        private string _AutoKey = "";
        private string _LocationId = "";
        private string _MonthFrom = "";
        private string _MonthTo = "";
        private string _YearFrom = "";
        private string _YearTo = "";
        #endregion
        #region [ Properties ]
        public string AutoKey { get => _AutoKey; set => _AutoKey = value; }
        public string LocationId { get => _LocationId; set => _LocationId = value; }
        public string MonthFrom { get => _MonthFrom; set => _MonthFrom = value; }
        public string MonthTo { get => _MonthTo; set => _MonthTo = value; }
        public string YearFrom { get => _YearFrom; set => _YearFrom = value; }
        public string YearTo { get => _YearTo; set => _YearTo = value; }

        #endregion
        #region [ Constructor Get Information ]
        public MapCommodityModel()
        {

        }
        public MapCommodityModel(string tam)
        {

        }
        #endregion
       
    }
}
