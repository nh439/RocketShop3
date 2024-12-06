using GraphQL;
using RocketShop.Shared.Model.WHRequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Model
{
    public class WHQueryContainer(string token)
    {
        public readonly string Token = token;
        public List<WarehouseQuery> Queries { get; set; } = new();
        public GraphQLRequest ToGraphQLRequest() =>
            new GraphQLRequest
            {
                Query = $@"""
                query {{
                    {string.Join(Environment.NewLine,Queries.Select(s=>s.ToQueryString()))}
                    }}
                """
            };
    }

}
