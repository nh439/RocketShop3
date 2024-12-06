using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Model
{
    public sealed record WHClientConfiguration
    (
        string ClientId,
        string Application,
        string? ClientSecret = null
        );
    
}
