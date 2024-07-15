using LanguageExt;
using RocketShop.DomainCenter.Model;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;

namespace RocketShop.DomainCenter.Services
{
    public class Packageservices : BaseServices
    {
        List<PackageStore> stores = new List<PackageStore>();
        Serilog.ILogger _logger;
        public Packageservices(Serilog.ILogger logger) : base("Package Services", logger)
        {
            _logger = logger;
        }
        public void Add(string key, string value) =>
            InvokeVoidService(() =>
            {
                stores.Add(new PackageStore(key, value));
                _logger.Information("New Package Key {key} Stroed", key);
            });


        public Either<Exception, string?> Find(string key) =>
            InvokeService(() =>
            {
                var value = stores.FirstOrDefault(x => x.key == key)?.value;
                _logger.Information("Get Data From Key {key} {state}", key, (value.HasMessage() ? "Success" : "Failed"));
                return value;
            });
           

    }
}
