using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.DataAccess;
using ApplicationCore.Models;
using ApplicationCore.Specifications;
using ApplicationCore.Helpers;

namespace ApplicationCore.Services
{
    public interface IApiKeysService
    {
        Task<IEnumerable<ApiKey>> FetchAsync(User user);
        Task<ApiKey> CreateAsync(ApiKey apiKey);
        Task UpdateAsync(ApiKey apiKey);
        Task DeleteAsync(ApiKey apiKey);
        Task SyncRolesAsync(User user, IList<string> roles);
        ApiKey Find(string key);
        Task<ApiKey> GetByIdAsync(int id);
    }

    public class ApiKeysService : IApiKeysService
    {
        private readonly IDefaultRepository<ApiKey> _apiKeyRepository;
        public ApiKeysService(IDefaultRepository<ApiKey> apiKeyRepository)
        {
            _apiKeyRepository = apiKeyRepository;
        }

        
        public async Task<IEnumerable<ApiKey>> FetchAsync(User user)
        {
            var spec = new ApiKeyFilterSpecification(user);
            return await _apiKeyRepository.ListAsync(spec);
        }
        public async Task<ApiKey> CreateAsync(ApiKey apiKey) => await _apiKeyRepository.AddAsync(apiKey);
        public async Task UpdateAsync(ApiKey apiKey) => await _apiKeyRepository.UpdateAsync(apiKey);
        public async Task DeleteAsync(ApiKey apiKey) => await _apiKeyRepository.DeleteAsync(apiKey);
        public ApiKey Find(string key)
        {
            var spec = new ApiKeyFilterSpecification(key);
            return _apiKeyRepository.GetSingleBySpec(spec);
        }
        public async Task<ApiKey> GetByIdAsync(int id) => await _apiKeyRepository.GetByIdAsync(id);

        public async Task SyncRolesAsync(User user, IList<string> roles)
        {
            var apiKeys = await FetchAsync(user);
            if (apiKeys.IsNullOrEmpty()) return;

            foreach (var apiKey in apiKeys)
            {
                apiKey.Role = roles.JoinToString();
            }

            _apiKeyRepository.UpdateRange(apiKeys);
        }
    }
}
