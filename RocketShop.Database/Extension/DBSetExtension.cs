using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Extension
{
    public static class DBSetExtension
    {
        public static IQueryable<T> UsePaging<T>(this IQueryable<T> query, int page, int per = 10) =>
            query.Skip((page - 1) * per)
            .Take(per);

    }
}
