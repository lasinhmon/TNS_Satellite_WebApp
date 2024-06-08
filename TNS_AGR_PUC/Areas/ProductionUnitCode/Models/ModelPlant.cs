using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNS_AGR_PUC.Areas.ProductionUnitCode.Models
{
    public class ModelPlant
    {
        #region [ Field Name ]

        private string _Index = "";
        private string _LC = "";
        private string _Geo = null;
        #endregion
        #region [ Properties ]

        public string Index { get => _Index; set => _Index = value; }
        public string LC { get => _LC; set => _LC = value; }
        public string Geo { get => _Geo; set => _Geo = value; }
      

        #endregion
        #region [ Constructor Get Information ]
        public ModelPlant()
        {

        }
        public ModelPlant(string tam)
        {

        }
        #endregion
    }
}
