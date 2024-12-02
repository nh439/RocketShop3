using Dapper;
using Irony.Parsing;
using LanguageExt;
using RocketShop.Database;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using System.Data;
using Token = RocketShop.Database.Model.Warehouse.Authorization.Token;

namespace RocketShop.Warehouse.Repository
{
    public class TokenRepository
    {
        const string tableName = TableConstraint.Token;

        public async Task<Token?> CreateToken(
            Token token,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null) =>
            await warehouseConnection.CreateQueryStore(tableName, true)
                .InsertAndReturnItemAsync<Token>(token, transaction);

        public async Task<Option<Token>> Introspection(string tokenKey,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null) =>
            await warehouseConnection.CreateQueryStore(tableName, true)
            .Where(nameof(Token.TokenKey), tokenKey)
            .FetchOneAsync<Token>(transaction);

        public async Task<bool> UsableCheck(string tokenKey,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null) =>
            await warehouseConnection.QueryFirstOrDefaultAsync<int>(
                $@"select 1  from ""{tableName}"" 
    where ""{nameof(Token.TokenKey)}"" = @token and
    (""{nameof(Token.RemainingAccess)}"" > 0 or ""{nameof(Token.RemainingAccess)}"" is null ) and 
(
    ""{nameof(Token.IssueDate)}""+ (""{nameof(Token.TokenAge)}""::INTERVAL) > CURRENT_TIMESTAMP or 
    ""{nameof(Token.TokenAge)}"" is null
);",
               new
               {
                   token = tokenKey
               }, transaction).EqAsync(1);

        public async Task<bool> UseToken(
            string tokenKey,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null
            ) =>
            await warehouseConnection.ExecuteAsync($@"update ""{tableName}""
set ""{nameof(Token.RemainingAccess)}""=""{nameof(Token.RemainingAccess)}""-1 
where ""{nameof(Token.TokenKey)}"" = @token and ""{nameof(Token.RemainingAccess)}"" is not null",
                 new
                 {
                     token = tokenKey
                 }, transaction)
            .GeAsync(-1);

        public async Task<bool> Revocation(string token,
             IDbConnection warehouseConnection,
            IDbTransaction? transaction = null) =>
            await warehouseConnection.CreateQueryStore(tableName)
            .Where(nameof(Token.TokenKey), token)
            .UpdateAsync(new
            {
                RemainingAccess = 0
            },transaction)
            .GeAsync(0);
    }
}
