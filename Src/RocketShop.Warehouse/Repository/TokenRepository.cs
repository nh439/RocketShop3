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

        public async Task<Option<Token>> GetInformation(string tokenKey,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null) =>
            await warehouseConnection.CreateQueryStore(tableName, true)
            .Where(nameof(Token.TokenKey), tokenKey)
            .FetchOneAsync<Token>(transaction);

        public async Task<bool> Introspection(string tokenKey,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null) =>
            await warehouseConnection.QueryFirstOrDefaultAsync<int>(
                $@"select 1  from ""{tableName}"" 
    where ""{nameof(Token.TokenKey)}"" = @token and
    ""{nameof(Token.RemainingAccess)}"" > 0 and 
    ""{nameof(Token.IssueDate)}""+ (""{nameof(Token.TokenAge)}""*interval '1 SECOND') > CURRENT_TIMESTAMP;",
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
where ""{nameof(Token.TokenKey)}"" = @token",
                 new
                 {
                     token = tokenKey
                 }, transaction)
            .GeAsync(0);
    }
}
