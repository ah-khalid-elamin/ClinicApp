using Microsoft.Azure.Cosmos.Table;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Helpers.Conversations {
    public class BaseTablesHelper<TEntity>
           where TEntity : TableEntity, new()
    {
        internal CloudTable _table;
        internal string _partitionKey;
        public BaseTablesHelper(
           IConfiguration configuration,
           string tableName,
           string partitionKey)
        {
            var storageAccountConnectionString = configuration["AzureTableAccountConnectionString"];
            var tableClient = CloudStorageAccount.Parse(storageAccountConnectionString).CreateCloudTableClient();
            _table = tableClient.GetTableReference(tableName);
            _table.CreateIfNotExistsAsync();
            _partitionKey = partitionKey;
        }

        public async Task Create(TEntity entity)
        {
            var operation = TableOperation.Insert(entity);
            await _table.ExecuteAsync(operation);
        }
        public async Task CreateOrUpdateAsync(TEntity entity)
        {
            var operation = TableOperation.InsertOrReplace(entity);
            await _table.ExecuteAsync(operation);
        }
        public Task DeleteAsync(TEntity entity)
        {
            var operation = TableOperation.Delete(entity);
            return _table.ExecuteAsync(operation);
        }

        public async Task<TEntity> GetAsync(string rowKey)
        {
            var operation = TableOperation.Retrieve<TEntity>(_partitionKey, rowKey);
            var result = await _table.ExecuteAsync(operation);
            return result.Result as TEntity;
        }


    }
}
