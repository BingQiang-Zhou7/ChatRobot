
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class CheckNetworkAccess
    {
        public bool isConnect()
        {
            try
            {
                System.Net.Dns.GetHostEntry("www.baidu.com");
                return true;
            }
            catch (Exception)
            {
                //throw;
                return false;
            }
        }
    }
}
