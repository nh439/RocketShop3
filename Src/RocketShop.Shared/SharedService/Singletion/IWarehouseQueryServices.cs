using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using LanguageExt;
using Microsoft.Extensions.Logging;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using RocketShop.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.SharedService.Singletion
{
    public interface IWarehouseQueryServices
    {
        Task<Either<Exception, WHResponseModel>> QueryAsync(WHQueryContainer wHQueryContainer);
    }
    public class WarehouseQueryServices(ILogger<WarehouseQueryServices> logger,
        IUrlIndeiceServices urlIndeiceServices
        ) : BaseServices<WarehouseQueryServices>("Warehouse Query Services", logger),IWarehouseQueryServices
    {
        public async Task<Either<Exception, WHResponseModel>> QueryAsync(WHQueryContainer wHQueryContainer) =>
            await InvokeServiceAsync(async () =>
            {
                var urlResult = await urlIndeiceServices.GetUrls();
                if (urlResult.IsLeft)
                    throw urlResult.GetLeft()!;
                var url = $"{urlResult.GetRight()!.WarehouseAPIUrl}/query";
                var request = wHQueryContainer.ToGraphQLRequest();
                using var graphQLClient = new GraphQLHttpClient(url, new NewtonsoftJsonSerializer());
                graphQLClient.HttpClient.DefaultRequestHeaders.Authorization= new AuthenticationHeaderValue("Bearer", wHQueryContainer.Token);
                var response = await graphQLClient.SendQueryAsync<Dictionary<string,dynamic>>(request);
                var output = new WHResponseModel();
                output.Data = new WHResponseData( response.Data);
                return output;
            });
    }
}
