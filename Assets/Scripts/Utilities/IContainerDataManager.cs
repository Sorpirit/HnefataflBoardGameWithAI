using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utilities
{
    public interface IContainerDataManager<T>
    {
        public Task<bool> SaveDataContainerAsync(string containerName, T data, bool overrideExisting = false);

        public Task<T> LoadDataContainerAsync(string containerName);

        public Task<T[]> LoadAllDataContainersAsync();
        
        public IReadOnlyCollection<string> GetContainerNames();
    }
}