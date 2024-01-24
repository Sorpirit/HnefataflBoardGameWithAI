using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ProtoBuf;

namespace Utilities
{
    public class ProtoContainerDataManager<T> : IContainerDataManager<T>
    {
        
        private const string ContainerExtension = ".con";
        private readonly string _rootPath;

        public string RootPath => _rootPath;

        public ProtoContainerDataManager(string rootPath)
        {
            _rootPath = rootPath;
            
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);
        }

        public async Task<bool> SaveDataContainerAsync(string containerName, T data, bool overrideExisting = false)
        {
            string path = _rootPath + "/" + containerName + ContainerExtension;
            if (File.Exists(path) && !overrideExisting)
                return false;

            await using var file = File.Create(path);
            Serializer.Serialize(file, data);
            return true;
        }

        public async Task<T> LoadDataContainerAsync(string containerName)
        {
            string path = _rootPath + "/" + containerName + ContainerExtension;
            if (!File.Exists(path))
                throw new FileNotFoundException("File not found: " + path);

            await using var file = File.OpenRead(path);
            var result = Serializer.Deserialize<T>(file);
            return result;
        }

        public async Task<T[]> LoadAllDataContainersAsync()
        {
            List<Task<T>> tasks = new List<Task<T>>();
            var containerNames = GetContainerNames();
            foreach (var containerName in containerNames)
            {
                tasks.Add(LoadDataContainerAsync(containerName));
            }

            return await Task.WhenAll(tasks);
        }

        public IReadOnlyCollection<string> GetContainerNames()
        {
            return Directory.EnumerateFiles(_rootPath, "*" + ContainerExtension).ToArray();
        }
    }
}