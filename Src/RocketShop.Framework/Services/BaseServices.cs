using LanguageExt;
using RocketShop.Framework.Extension;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Services
{
    public class BaseServices(string serviceName,
            ILogger logger,
            IDbConnection? connection = null)
    {

        protected Either<Exception,T> InvokeService<T>(Func<T> operation,
            Action<Exception>? catchOperation = null,
            string? errorMessage = null,
            int retries = 0,
            bool isExponential= false,
            int intervalSecond = 1
            )
        {
            int retriesCount = 0;
            while (true)
            {
                try
                {
                    return operation();
                }
                catch (Exception x)
                {
                    logger.Error(x, "Service : {ServiceName} Error : {Message}", serviceName, errorMessage ?? x.Message);
                    if (catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.                
                    if (retriesCount.GeOrEq(retries))
                        return x;
                    AppliedDelayPolicy(retriesCount,intervalSecond, isExponential);
                    retriesCount++;
                }
            }
        }
        protected async Task< Either<Exception,T>> InvokeServiceAsync<T>(Func<Task<T>> operation,
            Action<Exception>? catchOperation = null,
            string? errorMessage = null,
            int retries = 0,
            bool isExponential = false,
            int intervalSecond = 1)
        {
            int retriesCount = 0;
            while (true)
            {
                try
                {
                    return await operation();
                }
                catch (Exception x)
                {
                    logger.Error(x, "Service : {ServiceName} Error : {Message}", serviceName, errorMessage ?? x.Message);
                    if (catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    if (retriesCount.GeOrEq(retries))
                        return x;
                    await AppliedDelayPolicyAsync(retriesCount, intervalSecond, isExponential);
                    retriesCount++;
                }
            }
        }
        protected void InvokeVoidService(Action operation,
            Action<Exception>? catchOperation = null,
            string? errorMessage = null,
            int retries = 0,
            bool isExponential = false,
            int intervalSecond = 1)
        {
            int retriesCount = 0;
            while (true)
            {
                try
                {
                    operation();
                }
                catch (Exception x)
                {
                    logger.Error(x, "Service : {ServiceName} Error : {Message}", serviceName, errorMessage ?? x.Message);
                    if (catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    if (retriesCount.GeOrEq(retries))
                        throw;
                    AppliedDelayPolicy(retriesCount, intervalSecond, isExponential);
                    retriesCount++;
                }
            }
        }
        protected async Task InvokeVoidServiceAsync(Func<Task> operation,
            Action<Exception>? catchOperation = null,
            string? errorMessage = null,
            int retries = 0,
            bool isExponential = false,
            int intervalSecond = 1)
        {
            int retriesCount = 0;
            while (true)
            {
                try
                {
                    await operation();
                }
                catch (Exception x)
                {
                    logger.Error(x, "Service : {ServiceName} Error : {Message}", serviceName, errorMessage ?? x.Message);
                    if (catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    if (retriesCount.GeOrEq(retries))
                        throw;
                    await AppliedDelayPolicyAsync(retriesCount, intervalSecond, isExponential);
                    retriesCount++;
                }
            }
        }
        protected Either<Exception, T> InvokeDapperService<T>(Func<IDbConnection,T> operation,
            Action<Exception>? catchOperation = null,
            string? errorMessage = null,
            int retries = 0,
            bool isExponential = false,
            int intervalSecond = 1)
        {
            int retriesCount = 0;
            Exception ex;
            while (true)
            {
                try
                {
                    if (connection.IsNull())
                        throw new NullReferenceException("Connection Is NOT Null When Use 'Invoke Dapper Service'");
                    return operation(connection!);
                }
                catch (Exception x)
                {
                    logger.Error(x, "Service : {ServiceName} Error : {Message}", serviceName, errorMessage ?? x.Message);
                    if (catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    if (retriesCount.Le(retries))
                    {
                        AppliedDelayPolicy(retriesCount, intervalSecond, isExponential);
                        retriesCount++;
                        continue;
                    }             
                    ex = x;
                }
                finally
                {
                    if (connection!.State != ConnectionState.Closed)
                        connection.Close();
                }
                return ex;
            }
        }
        protected async Task<Either<Exception, T>> InvokeDapperServiceAsync<T>(Func<IDbConnection,Task<T>> operation,
            Action<Exception>? catchOperation = null,
            string? errorMessage = null,
            int retries = 0,
            bool isExponential = false,
            int intervalSecond = 1)
        {
            int retriesCount = 0;
            Exception ex;
            while (true)
            {
                try
                {
                    if (connection.IsNull())
                        throw new NullReferenceException("Connection Is NOT Null When Use 'Invoke Dapper Service'");
                    return await operation(connection!);
                }
                catch (Exception x)
                {
                    logger.Error(x, errorMessage ?? x.Message);
                    if (catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    if (retriesCount.Le(retries))
                    {
                        AppliedDelayPolicy(retriesCount, intervalSecond, isExponential);
                        retriesCount++;
                        continue;
                    }
                    ex = x;
                }
                finally
                {
                    if (connection!.State != ConnectionState.Closed)
                        connection.Close();
                }
                return ex;
            }
        }

        void AppliedDelayPolicy(int retries, int interval,bool isExp)
        {
            if (isExp)
                DelayExponential(retries);
            else
                DelaySecond(interval);
        }
        async Task AppliedDelayPolicyAsync(int retries, int interval,bool isExp)
        {
            if (isExp)
               await DelayExponentialAsync(retries);
            else
               await DelaySecondAsync(interval);
        }


        void DelaySecond(int interval)=>
            Thread.Sleep(interval * 1000);

        async Task DelaySecondAsync(int interval) =>
            await Task.Delay(interval * 1000);

        void DelayExponential(int retries) =>
            Thread.Sleep((int)Math.Pow(2,retries)*1000);

        async Task DelayExponentialAsync(int retries) =>
            await Task.Delay((int)Math.Pow(2, retries)*1000);

    }
}
