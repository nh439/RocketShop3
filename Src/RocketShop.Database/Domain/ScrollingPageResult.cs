using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Domain
{
    public sealed record ScrollingPageResult<TData>(
        IEnumerable<TData>? Data,
        int Limit,
        bool HasValue,
        bool HasNextData,
        string? NextToken
        );
    public sealed record NextPageToken(
        int Offset,
        int Limit = 10,
        string? TableName = null,
        IEnumerable<string>? selectedCol = null,
        string? rawCondition = null,
        string? OrderBy = null,
        object? Parameter = null,
        bool asc = true     
        );
}
