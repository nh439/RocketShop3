using LanguageExt;
using Microsoft.Extensions.Logging;
using RocketShop.Framework.Extension;
using RocketShop.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RocketShop.Shared.SharedService.Singletion
{
    /// <summary>
    /// Provides an interface for in-memory storage services, supporting
    /// add, remove, retrieve, and existence check operations with expiration.
    /// </summary>
    public interface IMemoryStorageServices
    {
        /// <summary>
        /// Adds data to the memory storage with an expiration time.
        /// </summary>
        /// <param name="key">The unique key for the data entry.</param>
        /// <param name="value">The value to be stored.</param>
        /// <param name="expiredIn">The time span after which the data expires.</param>
        void AddData(string key, object value, TimeSpan expiredIn);
        /// <summary>
        /// Removes data from the memory storage by key.
        /// </summary>
        /// <param name="key">The unique key for the data entry to remove.</param>
        void RemoveData(string key);
        /// <summary>
        /// Retrieves data from the memory storage by key.
        /// </summary>
        /// <param name="key">The unique key for the data entry to retrieve.</param>
        /// <returns>An Option containing the data if found; otherwise, an empty Option.</returns>
        Option<object> GetData(string key);
        /// <summary>
        /// Retrieves data of a specified type from the memory storage by key.
        /// </summary>
        /// <typeparam name="T">The expected type of the stored data.</typeparam>
        /// <param name="key">The unique key for the data entry to retrieve.</param>
        /// <returns>An Option containing the data of type <typeparamref name="T"/> if found; otherwise, an empty Option.</returns>
        Option<T> GetData<T>(string key) where T : class;
        /// <summary>
        /// Checks if a data entry exists in the memory storage.
        /// </summary>
        /// <param name="key">The unique key for the data entry to check.</param>
        /// <returns>True if the entry exists; otherwise, false.</returns>
        bool Exists(string key);
    }
    public class MemoryStorageServices : IMemoryStorageServices,IDisposable
    {
        readonly System.Timers.Timer timer;
        readonly ILogger<MemoryStorageServices> logger;
        List<MemoryStorage> storages = new List<MemoryStorage>();
        public MemoryStorageServices(ILogger<MemoryStorageServices> logger)
        {
            this.logger = logger;
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += Fetching;
            timer.Enabled = true;
        }



        public void AddData(string key, object value, TimeSpan expiredIn)
        {
            storages.Add(new MemoryStorage
            {
                CreateDate = DateTime.Now,
                Key = key,
                Value = value,
                ExpiredIn = expiredIn,
            });
            logger.LogInformation("Storage Added");
        }

        public void RemoveData(string key) { 
            storages = storages.Where(x=>x.Key != key).ToList(); 
            if(storages.IsNull())storages = new List<MemoryStorage>();
            logger.LogInformation("Storage Removed");
        }

        public Option<object> GetData(string key)
        {
                var output = storages.FirstOrDefault(x => x.Key == key)?.Value;
                logger.LogInformation("Storage Retrived");
                return output;
        }

        public Option<T> GetData<T>(string key) where T : class 
        {
            try
            {
                var output = storages.FirstOrDefault(x => x.Key == key)?.Value;
                logger.LogInformation("Storage Retrived");
                return output as T;
            }
            catch (Exception ex) {
                logger.LogError(ex, ex.Message);
                return Option<T>.None;
            }
        }

        public bool Exists(string key) => storages.Where(x => x.Key == key).HasData();

        void Fetching(object? src,ElapsedEventArgs e)
        {
            int expiredCnt = storages.Count(x=>x.IsExpired);
            storages =storages.Where(x=>!x.IsExpired).ToList();
            if (storages.IsNull()) storages = new List<MemoryStorage>();
            if (expiredCnt.Ge(0))
                logger.LogInformation("Clear {cnt} Expired Data", expiredCnt);
        }

        void IDisposable.Dispose() { 
        timer?.Dispose();
        }
    }
}
