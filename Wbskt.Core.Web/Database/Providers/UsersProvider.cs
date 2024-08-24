﻿using System.Data;
using System.Data.SqlClient;

namespace Wbskt.Core.Web.Database.Providers
{
    public class UsersProvider : IUsersProvider
    {
        private readonly ILogger<UsersProvider> logger;
        private readonly string _connectionString;

        public UsersProvider(ILogger<UsersProvider> logger, IConnectionStringProvider connectionStringProvider)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionString = connectionStringProvider?.ConnectionString ?? throw new ArgumentNullException(nameof(connectionStringProvider));
        }

        public int AddUser(UserData user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "dbo.User_Insert";

            command.Parameters.Add(new SqlParameter("@UserName", ProviderExtensions.ReplaceDbNulls(user.UserName)));
            command.Parameters.Add(new SqlParameter("@EmailId", ProviderExtensions.ReplaceDbNulls(user.EmailId)));
            command.Parameters.Add(new SqlParameter("@PasswordHash", ProviderExtensions.ReplaceDbNulls(user.PasswordHash)));
            command.Parameters.Add(new SqlParameter("@PasswordSalt", ProviderExtensions.ReplaceDbNulls(user.PasswordSalt)));

            var id = new SqlParameter("@Id", SqlDbType.Int) { Size = int.MaxValue };
            id.Direction = ParameterDirection.Output;
            command.Parameters.Add(id);

            command.ExecuteNonQuery();

            return user.UserId = (int)(ProviderExtensions.ReplaceDbNulls(id.Value) ?? 0);
        }

        public UserData GetUserDataById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "dbo.User_GetBy_Id";

            command.Parameters.Add(new SqlParameter("@Id", id));

            using var reader = command.ExecuteReader();
            var mapping = GetColumnMapping(reader);
            reader.Read();
            return ParseData(reader, mapping);
        }

        public UserData GetUserDataByEmailId(string emailId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "dbo.User_GetBy_EmailId";

            command.Parameters.Add(new SqlParameter("@EmailId", emailId));

            using var reader = command.ExecuteReader();
            var mapping = GetColumnMapping(reader);
            reader.Read();
            return ParseData(reader, mapping);
        }

        private static UserData ParseData(IDataRecord reader, OrdinalColumnMapping mapping)
        {
            var data = new UserData
            {
                UserId = (int)reader.GetValue(mapping.UserId),
                UserName = (string)reader.GetValue(mapping.EmailId),
                EmailId = (string)reader.GetValue(mapping.EmailId),
                PasswordHash = (string)reader.GetValue(mapping.EmailId),
                PasswordSalt = (string)reader.GetValue(mapping.EmailId)
            };

            return data;
        }

        private static OrdinalColumnMapping GetColumnMapping(IDataRecord reader)
        {
            var mapping = new OrdinalColumnMapping();

            mapping.UserId = reader.GetOrdinal("Id");
            mapping.UserName = reader.GetOrdinal("UserName");
            mapping.EmailId = reader.GetOrdinal("EmailId");
            mapping.PasswordHash = reader.GetOrdinal("PasswordHash");
            mapping.PasswordSalt = reader.GetOrdinal("PasswordSalt");

            return mapping;
        }

        private class OrdinalColumnMapping
        {
            public int UserId;
            public int UserName;
            public int EmailId;
            public int PasswordHash;
            public int PasswordSalt;
        }
    }
}
