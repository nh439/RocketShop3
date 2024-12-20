using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Model
{
    public sealed class WHResponseModel
    {
        public WHResponseData Data { get; set; }

        public Option<T> GetItem<T>(string Key) => Data.Items[Key];

        public List<string> GetItemKeys() => new List<string>(Data.Items.Keys);

        public bool ExistData(string Key) => Data.Items.ContainsKey(Key) && Data.Items[Key] is not null;
    }
    public sealed record WHResponseData(Dictionary<string,dynamic> Items);
}
