using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Views;

namespace ApplicationCore.ViewServices
{
    public static class ApiKeysViewService
    {
        public static ApiKeyViewModel MapViewModel(this ApiKey apiKey, IMapper mapper)
            => mapper.Map<ApiKeyViewModel>(apiKey);

        public static List<ApiKeyViewModel> MapViewModelList(this IEnumerable<ApiKey> apiKeys, IMapper mapper)
             => apiKeys.Select(item => MapViewModel(item, mapper)).ToList();
        public static IEnumerable<ApiKey> GetOrdered(this IEnumerable<ApiKey> apiKeys) => apiKeys.OrderByDescending(x => x.CreatedAt);
    }
}
