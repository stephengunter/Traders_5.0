using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Views;

namespace ApplicationCore.ViewServices
{
    public static class TicksViewService
    {
        public static IEnumerable<TickViewModel> GetOrdered(this IEnumerable<TickViewModel> ticks) => ticks.OrderBy(t => t.Order);

    }
}
