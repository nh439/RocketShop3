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
    public class BaseServices
    {
        readonly ILogger logger;
        readonly string serviceName;
        readonly IDbConnection? connection;
        public BaseServices(string serviceName,
            ILogger logger,
            IDbConnection? connection = null)
        {
            this.serviceName = serviceName;
            this.logger = logger;
            this.connection = connection;
        }

        protected Either<Exception,T> InvokeService<T>(Func<T> operation,
            Action<Exception>? catchOperation = null,
            string? errorMessage = null)
        {
            try
            {
                return operation();
            }
            catch(Exception x)
            {
                logger.Error(x,errorMessage ?? x.Message);
                if(catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                return x;
            }
        }
        protected async Task< Either<Exception,T>> InvokeServiceAsync<T>(Func<Task<T>> operation,
            Action<Exception>? catchOperation = null,
            string? errorMessage = null)
        {
            try
            {
                return await operation();
            }
            catch(Exception x)
            {
                logger.Error(x,errorMessage ?? x.Message);
                if(catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                return x;
            }
        }
        protected void InvokeVoidService(Action operation,
            Action<Exception>? catchOperation = null,
            string? errorMessage = null)
        {
            try
            {
                 operation();
            }
            catch(Exception x)
            {
                logger.Error(x,errorMessage ?? x.Message);
                if(catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                throw;
            }
        }
        protected async Task InvokeVoidServiceAsync(Func<Task> operation,
            Action<Exception>? catchOperation = null,
            string? errorMessage = null)
        {
            try
            {
                 await operation();
            }
            catch(Exception x)
            {
                logger.Error(x,errorMessage ?? x.Message);
                if(catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                throw;
            }
        }
        protected Either<Exception, T> InvokeDapperService<T>(Func<IDbConnection,T> operation,
            Action<Exception>? catchOperation = null,
            string? errorMessage = null)
        {
            try
            {
                if(connection.IsNull())
                    throw new NullReferenceException("Connection Is NOT Null When Use 'Invoke Dapper Service'");
                return operation(connection!);
            }
            catch (Exception x)
            {
                logger.Error(x, errorMessage ?? x.Message);
                if (catchOperation.IsNotNull())
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    catchOperation(x);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                return x;
            }
        }
        protected async Task<Either<Exception, T>> InvokeDapperServiceAsync<T>(Func<IDbConnection,Task<T>> operation,
            Action<Exception>? catchOperation = null,
            string? errorMessage = null)
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
                return x;
            }
        }



    }
}
