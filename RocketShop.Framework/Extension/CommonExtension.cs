using LanguageExt;
using System.Text.Json;

namespace RocketShop.Framework.Extension
{
    public static class CommonExtension
    {
        public static Option<T> AsOptional<T>(this T item) =>
            Prelude.Optional(item);

        public static async Task<Option<T>> AsOptionalAsync<T>(this Task<T> item) =>
            Prelude.Optional(await item);

        public static T? Extract<T>(this Option<T> option,T? valueIsnull = default) =>
            option.IsNone ? valueIsnull : option.FirstOrDefault();

        public static R? GetRight<L,R>(this Either<L,R> eithers) =>
            eithers.FirstOrDefault()!.Right;

        public static L? GetLeft<L,R>(this Either<L,R> eithers) =>
            eithers.FirstOrDefault()!.Left;

        public static object? GetData<L, R>(this Either<L, R> eithers) =>
            eithers.IsRight ? eithers.FirstOrDefault()!.Right : eithers.FirstOrDefault()!.Left;

        public static bool If<T>(this T? input ,Func<T?,bool> predicate )=>
            predicate(input);

        public static Toutput? If<Tinput, Toutput>(this Tinput? input,
            Func<Tinput?, bool> predicate,
            Toutput? valueIfTrue,
            Toutput? valueIfFalse = default) =>
            predicate(input) ? valueIfTrue : valueIfFalse;

        public static Toutput? If<Tinput, Toutput>(this Tinput? input,
            Func<Tinput?, bool> predicate,
            Func<Tinput?, Toutput> trueOperation,
            Func<Tinput?, Toutput>? falseOperation = null) {
            if(predicate(input))
                return trueOperation(input);
            else if(falseOperation != null)
                return falseOperation(input);
            return default;
        }

        public static Tinput? If<Tinput>(this Tinput? input,
            Func<Tinput?, bool> predicate,
            Action<Tinput?> trueOperation,
            Action<Tinput?>? falseOperation = null) {
            if(predicate(input))
                 trueOperation(input);
            else if(falseOperation != null)
                 falseOperation(input);
            return input;
        }

        public static T? CustomParse<T>(this string jsonStr) =>
          string.IsNullOrEmpty( jsonStr) ? default :  JsonSerializer.Deserialize<T>(jsonStr);

        public static bool IsNotNull(this object? item) =>
            item != null;
    }
}
