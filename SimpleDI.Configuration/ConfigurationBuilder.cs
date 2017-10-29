using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleDI.Configuration
{
    public class ConfigurationBuilder : IConfigurationBuilder
    {
        private readonly IInternalConfigurationRoot _configurationRoot;

        private string _basePath = string.Empty;
        private IDictionary<string, bool> _configFiles = new Dictionary<string, bool>();

        public ConfigurationBuilder()
        {
            _configurationRoot = new ConfigurationRoot();
        }

        public IConfigurationBuilder SetBasePath(string path)
        {
            _basePath = path;
            return this;
        }

        public IConfigurationBuilder AddJsonFile(string filename)
        {
            return AddJsonFile(filename, false);
        }

        public IConfigurationBuilder AddJsonFile(string filename, bool optional)
        {
            AddFileToList(_configFiles, filename, optional);
            return this;
        }

        public IConfigurationRoot Build()
        {
            foreach (var file in CleanFileList())
            {
                ReadFile(file.Key, file.Value);
            }
            return (IConfigurationRoot)_configurationRoot;
        }

        private void AddFileToList(IDictionary<string, bool> fileList, string filename, bool optional)
        {
            if (!fileList.Keys.Contains(filename))
            {
                fileList.Add(filename, optional);
            }
            else if (optional == false && fileList[filename] == true)
            {
                fileList[filename] = false;
            }
        }

        private IDictionary<string, bool> CleanFileList()
        {
            var configFiles = new Dictionary<string, bool>();

            foreach (var file in _configFiles)
            {
                try
                {
                    var fullPath = Path.GetFullPath(Path.Combine(_basePath, file.Key));
                    AddFileToList(configFiles, fullPath, file.Value);
                }
                catch(Exception e)
                {
                    throw new ArgumentException($"Specified path to config file not supported: {file.Key}.", e);
                }
            }

            return configFiles;
        }

        protected virtual void ReadFile(string filePath, bool optional)
        {
            if (!File.Exists(filePath) && !optional)
            {
                throw new FileNotFoundException($"Required configuration file not found: {filePath}");
            }
            if (File.Exists(filePath))
            {
                using (MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(filePath)))
                {
                    AddFileContents(memoryStream);
                }
            }
        }

        protected void AddFileContents(MemoryStream memoryStream)
        {
            _configurationRoot.AddJsonFromStream(memoryStream);
        }
    }
}
