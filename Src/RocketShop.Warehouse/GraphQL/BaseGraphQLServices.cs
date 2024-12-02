using RocketShop.Framework.Extension;
using System.Text.Json;

namespace RocketShop.Warehouse.GraphQL
{
    public class BaseGraphQLServices<GraphQLService>(IHttpContextAccessor accessor, ILogger<GraphQLService> logger)
    {
       protected async Task<T?> AuthorizeQueryAsync<T>(string objectName, Func<Task<T>> authorizedOperation)
        {
            var scopes = JsonSerializer.Deserialize<string[]>(accessor.HttpContext!.Session.GetString("read")!);
            var authorized = scopes!.Where(x => x == objectName).HasData();
            if (!authorized)
            {
                logger.LogError("Invalid Scope");
                return default!;
            }
            return await authorizedOperation();
        }
    }
}
