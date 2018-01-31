using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Mvc.ViewModels.AdminPanel
{
    public class PanelViewModel
    {
        public IList<AppUser> UserName { set; get; }
    }
}
