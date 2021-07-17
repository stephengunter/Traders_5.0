using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Views;
using Infrastructure.Views;
using ApplicationCore.Paging;
using ApplicationCore.Helpers;

namespace ApplicationCore.ViewServices
{
    public static class KLinesViewService
    {
        public static KLineViewModel MapViewModel(this KLine kLine, IMapper mapper)
        { 
            var model = mapper.Map<KLineViewModel>(kLine);
            model.Time = kLine.Time.ToTimeString();
            return model;
        }
       

        public static List<KLineViewModel> MapViewModelList(this IEnumerable<KLine> kLines, IMapper mapper)
             => kLines.Select(item => MapViewModel(item, mapper)).ToList();
        public static IEnumerable<KLine> GetOrdered(this IEnumerable<KLine> kLines) => kLines.OrderBy(item => item.Time);

        public static KLine MapEntity(this KLineViewModel model, IMapper mapper) => mapper.Map<KLineViewModel, KLine>(model);
        public static PagedList<KLine, KLineViewModel> GetPagedList(this IEnumerable<KLine> kLines, IMapper mapper, int page = 1, int pageSize = -1)
        {
            var pageList = new PagedList<KLine, KLineViewModel>(kLines, page, pageSize);

            pageList.ViewList = pageList.List.MapViewModelList(mapper);

            pageList.List = null;

            return pageList;
        }
    }
}
