using ItNews.Business.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business.Managers
{
    public interface IManager<T> 
        where T : IEntity
    {

    }
}
