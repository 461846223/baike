using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baike.Pagebuild
{
    using Baike.Pagebuild.Models;

    public class PagebuildConsole
    {
       public void Main()
       {
           var listController = new ListController(1);
           var request = new ListRequest();
           request.Pageindex = 0;
           request.Pagesize = 50;
           listController.Index(request);

       }
    }
}
