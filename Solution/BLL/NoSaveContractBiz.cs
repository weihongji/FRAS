using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DAL;

namespace BLL
{
   public  class NoSaveContractBiz
    {
       public static DataTable GetListNoSaveContract(int  org) {
           return NoSaveContractDao.GetListNoSaveContract(org);
       }
    }
}
