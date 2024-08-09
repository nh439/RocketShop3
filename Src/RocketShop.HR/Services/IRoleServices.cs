﻿using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Framework.Services;
using RocketShop.HR.Repository;

namespace RocketShop.HR.Services
{
    public interface IRoleServices
    {
        Task<Either<Exception, bool>> Create(Role role);
        Task<Either<Exception, bool>> Update(Role role);
        Task<Either<Exception, bool>> Delete(int roleId);
        Task<Either<Exception, int>> SetUserByRole(int roleId, string[] userIds);
        Task<Either<Exception, int>> SetRoleByUser(string userId, int[] roleIds);
        Task<Either<Exception, List<Role>>> ListRoles(string? searchName = null, int? page = null, int per = 20);
        Task<Either<Exception, List<Role>>> ListRoleByUsers(string userId, int? page = null, int per = 20);
        Task<Either<Exception, Option<Role>>> GetRole(int roleId);
    }
    public class RoleServices(Serilog.ILogger logger,
        IConfiguration configuration,
        RoleRepository roleRepository,
        UserRoleRepository userRoleRepository) :
        BaseServices("Role Service", logger, new NpgsqlConnection(configuration.GetIdentityConnectionString())), IRoleServices
    {
        public async Task<Either<Exception, bool>> Create(Role role) =>
            await InvokeServiceAsync(async () => await roleRepository.Create(role));

        public async Task<Either<Exception, bool>> Update(Role role) =>
            await InvokeDapperServiceAsync(async connection => await roleRepository.Update(role, connection));

        public async Task<Either<Exception, bool>> Delete(int roleId) =>
            await InvokeDapperServiceAsync(async connection =>
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                var relatedUsers = await userRoleRepository.GetUserIdByRole(roleId, connection, transaction);
                await userRoleRepository.RemoveUsersByRole(roleId, relatedUsers, connection, transaction);
                var result = await roleRepository.Delete(roleId, connection, transaction);
                transaction.Commit();
                return result;
            });

        public async Task<Either<Exception, int>> SetUserByRole(int roleId, string[] userIds) =>
            await InvokeDapperServiceAsync(async con =>
            {
                con.Open();
                using var transaction = con.BeginTransaction();
                await userRoleRepository.RemoveUsersByRole(roleId,userIds, con, transaction);
                var result = await userRoleRepository.AddUserByRole(roleId, userIds, con,transaction);
                transaction.Commit();
                con.Close();
                return result;
            });

        public async Task<Either<Exception, int>> SetRoleByUser(string userId, int[] roleIds) =>
            await InvokeDapperServiceAsync(async con => { 
                con.Open();
                using var transaction = con.BeginTransaction();
                var deletedResult = await userRoleRepository.RemoveRoleByUser(userId, roleIds, con, transaction);
                Console.WriteLine(deletedResult);
                var result =  await userRoleRepository.AddRoleByUser(userId, roleIds, con,transaction);
                transaction.Commit();
                con.Close();
                return result;
            });

        public async Task<Either<Exception, List<Role>>> ListRoles(string? searchName = null, int? page = null, int per = 20) =>
            await InvokeServiceAsync(async () => await roleRepository.GetRoles(searchName, page, per));

        public async Task<Either<Exception, List<Role>>> ListRoleByUsers(string userId, int? page = null, int per = 20) =>
            await InvokeDapperServiceAsync(async con =>
            {
                var roleIds = await userRoleRepository.GetRoleIdByUsers(userId, con);
                return await roleRepository.GetIdIn(roleIds, page, per);
            });

        public async Task<Either<Exception,Option<Role>>> GetRole(int roleId)=>
            await InvokeServiceAsync(async () => await roleRepository.GetById(roleId));


    }
}
