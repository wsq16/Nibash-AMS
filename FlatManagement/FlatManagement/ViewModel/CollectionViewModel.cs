using FlatManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.ViewModel
{
    public class CollectionViewModel
    {
        public IEnumerable<CommonFundVM> CommonFundVObj { get; set; }
        public IEnumerable<BillVM> BillObj { get; set; }
    }
}
