namespace RocketShop.Framework.Extension
{
    public static class EnumerableExtension
    {
        public static bool HasData<T>(this IEnumerable<T>? Titem)
        {
            try
            {
                return Titem != null && Titem.FirstOrDefault() != null && Titem.Count() > 0;
            }
            catch { return false; }
        }
        public static bool NoData<T>(this IEnumerable<T>? Titem) =>
           !Titem.HasData();

        public static IEnumerable<T>? HasData<T>(this IEnumerable<T>? Titem,
            Func<IEnumerable<T>, IEnumerable<T>> hasDataOperation,
            Func<IEnumerable<T>?, IEnumerable<T>>? noDataOperation = null
            )
        {
            if (Titem.HasData())
                return hasDataOperation(Titem!);
            if (noDataOperation != null)
                return noDataOperation(Titem);
            return Titem;
        }

        public static IEnumerable<T>? HasDataAndForEach<T>(this IEnumerable<T>? Titem, Action<T> innerForEachOperation)
        {
            if (Titem.HasData())
                foreach (var item in Titem!)
                    innerForEachOperation(item);
            return Titem;
        }

        public static async Task<IEnumerable<T>?> HasDataAndForEachAsync<T>(this IEnumerable<T>? Titem, Action<T> innerForEachOperation) =>
          await Task.Run(() => Titem?.HasDataAndForEach(innerForEachOperation));

        public static async Task<IEnumerable<T>?> HasDataAndParallelForEachAsync<T>(this IEnumerable<T>? Titem, Action<T> innerForEachOperation)
        {
            if (Titem.HasData())
            {
                object lockObj = new object();
                int w = 0;
                Parallel.ForEach(Titem!, i =>
                {
                    innerForEachOperation(i);
                    lock (lockObj)
                        w++;
                });
                while (w < Titem!.Count())
                    await Task.Delay(1);
            }
            return Titem;
        }

        public static IEnumerable<TResult>? HasDataAndMap<TSource,TResult>(this IEnumerable<TSource>? items,
            Func<TSource,TResult> mapFunction)
        {
            if (items.HasData())
                return items!.Select(s=> mapFunction(s));
            return null;
        }

        public static async Task< IEnumerable<TResult>?> HasDataAndMapAsync<TSource,TResult>(this IEnumerable<TSource>? items,
            Func<TSource,TResult> mapFunction) =>
            await Task.Run(() => items.HasDataAndMap(mapFunction));
        
        public static TResult? HasDataAndTranformData<TSource,TResult>(this IEnumerable<TSource>? items,
            Func<IEnumerable<TSource>, TResult> hasDataOperation,
            Func<TResult>? isNullOperation = null)
        {
            TResult? result = default;
            if (items.HasData())
                result = hasDataOperation(items!);
            else if (isNullOperation != null)
                result = isNullOperation();
            return result;
        }

public static async Task< TResult?> HasDataAndTranformDataAsync<TSource,TResult>(this IEnumerable<TSource>? items,
            Func<IEnumerable<TSource>, TResult> hasDataOperation,
            Func<TResult>? isNullOperation = null)
        => await Task.Run(() => items.HasDataAndTranformData(hasDataOperation, isNullOperation));


        public static bool IsNull<T>(this IEnumerable<T> Titem) => Titem == null;

        public static bool IsNotNull<T>(this IEnumerable<T> Titem) => Titem != null;
    }
}
