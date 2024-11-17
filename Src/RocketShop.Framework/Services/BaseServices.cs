using LanguageExt;
using Microsoft.Extensions.Logging;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Services
{
    public class BaseServices<TService>(string serviceName,
            ILogger<TService> logger,
            IDbConnection? connection = null,
            bool AutoDisposedConnection = true) : IDisposable
    {

        protected Either<Exception, T> InvokeService<T>(Func<T> operation,
            Action<Exception>? catchOperation = null,
            string? errorMessage = null,
            int retries = 0,
            bool isExponential = false,
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
                    logger.LogError(x, "Service : {ServiceName} Error : {Message}", serviceName, errorMessage ?? x.Message);
                    if (catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.                
                    if (retriesCount.GeOrEq(retries))
                        return x;
                    PolicyManager.AppliedDelayPolicy(retriesCount, intervalSecond, isExponential);
                    retriesCount++;
                }
            }
        }
        protected async Task<Either<Exception, T>> InvokeServiceAsync<T>(Func<Task<T>> operation,
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
                    logger.LogError(x, "Service : {ServiceName} Error : {Message}", serviceName, errorMessage ?? x.Message);
                    if (catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    if (retriesCount.GeOrEq(retries))
                        return x;
                    await PolicyManager.AppliedDelayPolicyAsync(retriesCount, intervalSecond, isExponential);
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
                    logger.LogError(x, "Service : {ServiceName} Error : {Message}", serviceName, errorMessage ?? x.Message);
                    if (catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    if (retriesCount.GeOrEq(retries))
                        throw;
                    PolicyManager.AppliedDelayPolicy(retriesCount, intervalSecond, isExponential);
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
                    logger.LogError(x, "Service : {ServiceName} Error : {Message}", serviceName, errorMessage ?? x.Message);
                    if (catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    if (retriesCount.GeOrEq(retries))
                        throw;
                    await PolicyManager.AppliedDelayPolicyAsync(retriesCount, intervalSecond, isExponential);
                    retriesCount++;
                }
            }
        }
        protected Either<Exception, T> InvokeDapperService<T>(Func<IDbConnection, T> operation,
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
                    logger.LogError(x, "Service : {ServiceName} Error : {Message}", serviceName, errorMessage ?? x.Message);
                    if (catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    if (retriesCount.Le(retries))
                    {
                        PolicyManager.AppliedDelayPolicy(retriesCount, intervalSecond, isExponential);
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
        protected async Task<Either<Exception, T>> InvokeDapperServiceAsync<T>(Func<IDbConnection, Task<T>> operation,
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
                    logger.LogError(x, errorMessage ?? x.Message);
                    if (catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    if (retriesCount.Le(retries))
                    {
                        PolicyManager.AppliedDelayPolicy(retriesCount, intervalSecond, isExponential);
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


        void IDisposable.Dispose()
        {
            if (AutoDisposedConnection)
            {
                if (connection != null && connection?.State != ConnectionState.Closed)
                    connection?.Close();
                connection?.Dispose();
            }
        }

        public void ClearConnection()
        {
            if (connection != null && connection?.State != ConnectionState.Closed)
                connection?.Close();
            connection?.Dispose();
        }

        public void OverrideConnection(IDbConnection newConnection) =>
            connection = newConnection;

        public void OpenConnection()
        {
            if(connection?.State != ConnectionState.Open)
                connection?.Open();
        }

        public void CloseConnection()
        {
            if(connection?.State != ConnectionState.Closed)
                connection?.Close();
        }

        public IDbConnection? GetConnection() => connection;


    }
}
