using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sensato.TestSec.Model;
using Sensato.DataAccess;
using System.Data;
using System.Data.SqlClient;

namespace Sensato.TestSec
{
    public partial class UserContex
    {
        public List<spr_Profiles_Set_0> GetProfiles()
        {
            DataTable dt = DataAccessADO.GetDataTable("spr_Profiles", CommandType.StoredProcedure, null, "");
            List<spr_Profiles_Set_0> resultList = dt.AsEnumerable().Select(x => new spr_Profiles_Set_0() {
                Description= x.Field<String>("Description"),

            }).ToList();
            return resultList;
        }

        public List<spr_Profiles_Set_0> GetProfiles(SqlTransaction sqlTran)
        {
            DataTable dt = DataAccessADO.GetDataTable("spr_Profiles", CommandType.StoredProcedure, null, "");
            List<spr_Profiles_Set_0> resultList = dt.AsEnumerable().Select(x => new spr_Profiles_Set_0()
            {
                Description = x.Field<String>("Description"),

            }).ToList();
            return resultList;
        }



    }
}
