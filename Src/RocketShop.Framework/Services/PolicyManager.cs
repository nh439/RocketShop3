using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Services
{
    public static class PolicyManager
    {
        #region Retry Policy
        /// <summary>
        /// Executes a specified operation with a retry policy. If the operation fails, 
        /// it retries the operation a specified number of times, with an optional delay between attempts. 
        /// The delay can be either fixed or exponentially increasing.
        /// </summary>
        /// <param name="tryOperation">The operation to attempt, represented as an <see cref="Action"/>.</param>
        /// <param name="catchOperation">An optional action to perform if an exception occurs, represented as an <see cref="Action{Exception}"/>.</param>
        /// <param name="retries">The number of retry attempts. Default is 5.</param>
        /// <param name="intervalSecond">The initial delay between retries, in seconds. Default is 1 second.(Not Effect Then isExponential is True)</param>
        /// <param name="isExponential">Indicates whether the retry delay should increase exponentially. Default is false.</param>
        public static void UseRetriesPolicy(Action tryOperation,
            Action<Exception>? catchOperation,
            int retries = 5,
            int intervalSecond = 1,
            bool isExponential = false)
        {
            int retriesCount = 0;
            while (true)
            {
                try
                {
                    tryOperation();
                    break;
                }
                catch (Exception x)
                {

                    if (catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.                
                    if (retriesCount.GeOrEq(retries))
                        break;
                    AppliedDelayPolicy(retriesCount, intervalSecond, isExponential);
                    retriesCount++;
                }
            }
        }
        /// <summary>
        /// Executes a specified asynchronous operation with a retry policy. If the operation fails, 
        /// it retries the operation a specified number of times, with an optional delay between attempts. 
        /// The delay can be either fixed or exponentially increasing.
        /// </summary>
        /// <param name="tryOperation">The asynchronous operation to attempt, represented as a <see cref="Func{Task}"/>.</param>
        /// <param name="catchOperation">An optional action to perform if an exception occurs, represented as an <see cref="Action{Exception}"/>.</param>
        /// <param name="retries">The number of retry attempts. Default is 5.</param>
        /// <param name="intervalSecond">The initial delay between retries, in seconds. Default is 1 second.</param>
        /// <param name="isExponential">Indicates whether the retry delay should increase exponentially. Default is false.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async static Task UseRetriesPolicyAsync(
            Func<Task> tryOperation,
            Action<Exception>? catchOperation,
            int retries = 5,
            int intervalSecond = 1,
            bool isExponential = false
            ) =>
            await Task.Factory.StartNew(() => UseRetriesPolicyAsync(tryOperation, catchOperation, retries, intervalSecond, isExponential));

        #endregion
        #region Safe While
        /// <summary>
        /// Repeatedly executes a specified action while a given expression remains true, with a limit on the number of iterations.
        /// </summary>
        /// <param name="expression">The condition that determines whether the loop should continue.</param>
        /// <param name="action">The action to execute on each iteration, represented as an <see cref="Action{ulong}"/>.</param>
        /// <param name="maximumLoop">The maximum number of iterations to perform. Default is <see cref="int.MaxValue"/>.</param>
        public static void UseWhile(bool expression, Action<ulong> action, ulong maximumLoop = int.MaxValue)
        {
            ulong currentloop = 0;
            while (true)
            {
                action(currentloop);
                if (currentloop >= maximumLoop)
                    break;
                currentloop++;
            }
        }
        /// <summary>
        /// Asynchronously executes a specified action repeatedly while a given expression remains true, 
        /// with a limit on the number of iterations.
        /// </summary>
        /// <param name="expression">The condition that determines whether the loop should continue.</param>
        /// <param name="action">The action to execute on each iteration, represented as an <see cref="Action{ulong}"/>.</param>
        /// <param name="maximumLoop">The maximum number of iterations to perform. Default is <see cref="int.MaxValue"/>.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task UseWhileAsync(bool expression, Action<ulong> action, ulong maximumLoop = int.MaxValue) =>
            await Task.Factory.StartNew(() => UseWhile(expression, action, maximumLoop));
        #endregion
        #region Parallel
        /// <summary>
        /// Executes a specified action in parallel for each item in a collection and Wait to Read Success.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="items">The collection of items to process.</param>
        /// <param name="action">The action to execute on each item, represented as an <see cref="Action{T}"/>.</param>
        public static void UseParallelReadItem<T>(IEnumerable<T> items, Action<T> action)
        {
            object lockObj = new object();
            long i = 0;
            Parallel.ForEach(items, (item) =>
            {
                action(item);
                lock (lockObj)
                {
                    i++;
                }
            });
            while (i.Le(items.Count()))
                Thread.Sleep(1);
        }
        /// <summary>
        /// Executes a specified action in parallel for each item in a collection  and Wait to Read Success., 
        /// allowing the loop's state to be controlled during execution.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="items">The collection of items to process.</param>
        /// <param name="action">
        /// The action to execute on each item, represented as an <see cref="Action{T, ParallelLoopState}"/>.
        /// The <see cref="ParallelLoopState"/> parameter can be used to control the loop's execution.
        /// </param>
        public static void UseParallelReadItem<T>(IEnumerable<T> items, Action<T, ParallelLoopState> action)
        {
            object lockObj = new object();
            long i = 0;
            Parallel.ForEach(items, (item, state) =>
            {
                action(item, state);
                lock (lockObj)
                {
                    i++;
                }
            });
            while (i.Le(items.Count()))
                Thread.Sleep(1);
        }
        /// <summary>
        /// Executes a specified sequence of actions in parallel for each item in a collection,
        /// with an option to perform operations before, during, and after locking on a shared object.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="items">The collection of items to process.</param>
        /// <param name="lockObject">The object to lock on for thread safety during the execution of the inner action.</param>
        /// <param name="beforeLockObject">The action to execute on each item before acquiring the lock.</param>
        /// <param name="innerLockObject">An optional action to execute on each item while holding the lock.</param>
        /// <param name="afterLockObject">An optional action to execute on each item after releasing the lock.</param>
        public static void UseParallelReadItem<T>(
            IEnumerable<T> items,
            object lockObject,
            Action<T> beforeLockObject,
            Action<T>? innerLockObject = null,
            Action<T>? afterLockObject = null
            )
        {
            long i = 0;
            Parallel.ForEach(items, (item) =>
            {
                beforeLockObject(item);
                lock (lockObject)
                {
                    if (innerLockObject.IsNotNull())
                        innerLockObject!(item);
                    i++;
                }
                if(afterLockObject.IsNotNull())
                    afterLockObject!(item);
            });
            while (i.Le(items.Count()))
                Thread.Sleep(1);
        }
        /// <summary>
        /// Executes a specified sequence of actions in parallel for each item in a collection,
        /// with an option to perform operations before, during, and after locking on a shared object,
        /// and with the ability to control the loop's state.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="items">The collection of items to process.</param>
        /// <param name="lockObject">The object to lock on for thread safety during the execution of the inner action.</param>
        /// <param name="beforeLockObject">
        /// The action to execute on each item before acquiring the lock, 
        /// represented as an <see cref="Action{T, ParallelLoopState}"/>.
        /// The <see cref="ParallelLoopState"/> allows control over the loop's execution.
        /// </param>
        /// <param name="innerLockObject">An optional action to execute on each item while holding the lock.</param>
        /// <param name="afterLockObject">An optional action to execute on each item after releasing the lock.</param>
        public static void UseParallelReadItem<T>(
            IEnumerable<T> items,
            object lockObject,
            Action<T, ParallelLoopState> beforeLockObject,
            Action<T>? innerLockObject = null,
            Action<T>? afterLockObject = null)
        {
            object lockObj = new object();
            long i = 0;
            Parallel.ForEach(items, (item, state) =>
            {
                beforeLockObject(item, state);
                lock (lockObject)
                {
                    if (innerLockObject.IsNotNull())
                        innerLockObject!(item);
                    i++;
                }
                if (afterLockObject.IsNotNull())
                    afterLockObject!(item);
            });
            while (i.Le(items.Count()))
                Thread.Sleep(1);
        }

        #endregion

        #region ParallelAsync
        /// <summary>
        /// Asynchronously executes a specified action in parallel for each item in a collection and Wait to Read Success.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="items">The collection of items to process.</param>
        /// <param name="action">The action to execute on each item, represented as an <see cref="Action{T}"/>.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task UseParallelReadItemAsync<T>(IEnumerable<T> items, Action<T> action) =>
            await Task.Factory.StartNew(()=> UseParallelReadItem(items, action));

        /// <summary>
        /// Asynchronously executes a specified action in parallel for each item in a collection and Wait to Read Success, 
        /// allowing the loop's state to be controlled during execution.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="items">The collection of items to process.</param>
        /// <param name="action">
        /// The action to execute on each item, represented as an <see cref="Action{T, ParallelLoopState}"/>.
        /// The <see cref="ParallelLoopState"/> parameter can be used to control the loop's execution.
        /// </param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task UseParallelReadItemAsync<T>(IEnumerable<T> items, Action<T, ParallelLoopState> action) =>
           await Task.Factory.StartNew(() => UseParallelReadItem(items, action));
        /// <summary>
        /// Asynchronously executes a sequence of actions in parallel for each item in a collection,
        /// with the ability to perform operations before, during, and after locking on a shared object.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="items">The collection of items to process.</param>
        /// <param name="lockObject">The object to lock on for thread safety during the execution of the inner action.</param>
        /// <param name="beforeLockObject">The action to execute on each item before acquiring the lock, represented as an <see cref="Action{T}"/>.</param>
        /// <param name="innerLockObject">An optional action to execute on each item while holding the lock.</param>
        /// <param name="afterLockObject">An optional action to execute on each item after releasing the lock.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task UseParallelReadItemAsync<T>(
           IEnumerable<T> items,
           object lockObject,
           Action<T> beforeLockObject,
           Action<T>? innerLockObject = null,
           Action<T>? afterLockObject = null
           ) =>
            await Task.Factory.StartNew(()=> UseParallelReadItem(items, lockObject, beforeLockObject, innerLockObject, afterLockObject));

        /// <summary>
        /// Asynchronously executes a sequence of actions in parallel for each item in a collection,
        /// with the ability to perform operations before, during, and after locking on a shared object,
        /// and with control over the loop's state.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="items">The collection of items to process.</param>
        /// <param name="lockObject">The object to lock on for thread safety during the execution of the inner action.</param>
        /// <param name="beforeLockObject">
        /// The action to execute on each item before acquiring the lock, 
        /// represented as an <see cref="Action{T, ParallelLoopState}"/>. 
        /// The <see cref="ParallelLoopState"/> allows control over the loop's execution.
        /// </param>
        /// <param name="innerLockObject">An optional action to execute on each item while holding the lock.</param>
        /// <param name="afterLockObject">An optional action to execute on each item after releasing the lock.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task UseParallelReadItemAsync<T>(
            IEnumerable<T> items,
            object lockObject,
            Action<T, ParallelLoopState> beforeLockObject,
            Action<T>? innerLockObject = null,
            Action<T>? afterLockObject = null) =>
            await Task.Factory.StartNew(() => UseParallelReadItem(items, lockObject, beforeLockObject, innerLockObject, afterLockObject));

        #endregion
        internal static void AppliedDelayPolicy(int retries, int interval, bool isExp)
        {
            if (isExp)
                DelayExponential(retries);
            else
                DelaySecond(interval);
        }
        internal static async Task AppliedDelayPolicyAsync(int retries, int interval, bool isExp)
        {
            if (isExp)
                await DelayExponentialAsync(retries);
            else
                await DelaySecondAsync(interval);
        }
        internal static void DelaySecond(int interval) =>
        Thread.Sleep(interval * 1000);

        internal static async Task DelaySecondAsync(int interval) =>
             await Task.Delay(interval * 1000);

        internal static void DelayExponential(int retries) =>
             Thread.Sleep((int)Math.Pow(2, retries) * 1000);

        internal static async Task DelayExponentialAsync(int retries) =>
                     await Task.Delay((int)Math.Pow(2, retries) * 1000);
    }
}
