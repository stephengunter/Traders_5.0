using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Managers
{
    public class TWFuturesTimeManager
    {
        const int BEGIN = 84500;
        const int END = 134500;

        public static bool InTime(int time) => time >= BEGIN && time <= END;
    }

    public class TWStocksTimeManager
    {
        const int BEGIN = 90000;
        const int END = 133000;

        public static bool InTime(int time) => time >= BEGIN && time <= END;
    }


}
