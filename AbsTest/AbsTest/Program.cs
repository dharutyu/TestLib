using AbsDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsTest
{

    


    class Program
    {
        static void Main(string[] args)
        {
            UnitOfWork uow = new UnitOfWork();
            TestService svc = new TestService(uow);
            
        }
    }
}
