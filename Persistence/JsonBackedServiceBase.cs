using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using Newtonsoft.Json.Converters;

namespace BallInChair.Persistence
{
    public abstract class JsonBackedServiceBase<TModel>
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings;
        
        static JsonBackedServiceBase()
        { 
            JsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            JsonSerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            JsonSerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
        }
        
        private readonly string _persistenceFile;

        protected JsonBackedServiceBase(string directory, string filename)
        {
            _persistenceFile = Path.Combine(directory, filename);

            if(!File.Exists(_persistenceFile))
            {
                throw new Exception($"Couldn't find persistence file at {_persistenceFile}");
            }
        }

        private static readonly object _fileLocker = new object(); 

        protected IEnumerable<TModel> GetPersistedData()
        {
            string fileContents;
            lock(_fileLocker)
            {
                fileContents = File.ReadAllText(_persistenceFile);
            }
            return JsonConvert.DeserializeObject<IEnumerable<TModel>>(fileContents, JsonSerializerSettings);
        }

        protected void SaveNewEntity(TModel entity)
        {
            var entities = GetPersistedData().ToList();
            entities.Add(entity);
            Save(entities);
        }

        protected void UpdateEntity(TModel entity, Func<TModel, bool> matchFunc)
        {
            var entities = GetPersistedData().ToList();
            entities.RemoveAll(a => matchFunc(a));
            entities.Add(entity);
            Save(entities);
        }

        protected void RemoveEntity(Func<TModel, bool> matchFunc)
        {
            var entities = GetPersistedData().ToList();
            entities.RemoveAll(a => matchFunc(a));
            Save(entities);
        }

        private void Save(IEnumerable<TModel> entities)
        {
            var fileContents = JsonConvert.SerializeObject(entities, JsonSerializerSettings);

            lock(_fileLocker)
            {
                File.WriteAllText(_persistenceFile, fileContents);
            }
        }
    }
}