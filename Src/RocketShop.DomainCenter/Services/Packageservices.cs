using LanguageExt;
using RocketShop.DomainCenter.Model;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;

namespace RocketShop.DomainCenter.Services
{
    public class Packageservices : BaseServices<Packageservices>
    {
        List<PackageStore> stores = new List<PackageStore>();
        ILogger<Packageservices> _logger;
        public Packageservices(ILogger<Packageservices> logger) : base("Package Services", logger)
        {
            _logger = logger;
        }
        public void Add(string key, string value) =>
            InvokeVoidService(() =>
            {
                stores.Add(new PackageStore(key, value));
                _logger.LogInformation("New Package Key {key} Stroed", key);
            });


        public Either<Exception, string?> Find(string key) =>
            InvokeService(() =>
            {
                var value = stores.FirstOrDefault(x => x.key == key)?.value;
                _logger.LogInformation("Get Data From Key {key} {state}", key, (value.HasMessage() ? "Success" : "Failed"));
                return value;
            });
    }
}
