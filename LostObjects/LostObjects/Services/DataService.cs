namespace LostObjects.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Models;
    using Interfaces;
    using SQLite;
    using Xamarin.Forms;

    public class DataService
    {
        private SQLiteAsyncConnection connection;

        public DataService()
        {
            this.OpenOrCreateDB();
        }

        private async Task OpenOrCreateDB()
        {
            var databasePath = DependencyService.Get<IPathService>().GetDatabasePath();
            this.connection = new SQLiteAsyncConnection(databasePath);
            await connection.CreateTableAsync<Objectt>().ConfigureAwait(false);
        }

        public async Task Insert<T>(T model)
        {
            await this.connection.InsertAsync(model);
        }

        public async Task Insert<T>(List<T> models)
        {
            await this.connection.InsertAllAsync(models);
        }

        public async Task Update<T>(T model)
        {
            await this.connection.UpdateAsync(model);
        }

        public async Task Update<T>(List<T> models)
        {
            await this.connection.UpdateAllAsync(models);
        }

        public async Task Delete<T>(T model)
        {
            await this.connection.DeleteAsync(model);
        }

        public async Task<List<Objectt>> GetAllObjects()
        {
            var query = await this.connection.QueryAsync<Objectt>("select * from [Objectt]");
            var array = query.ToArray();
            var list = array.Select(o => new Objectt
            {
                Name = o.Name,
                Type = o.Type,
                PhoneContact = o.PhoneContact,
                PublishOn = o.PublishOn,
                Location = o.Location,
                Description = o.Description,
                IsDelivered = o.IsDelivered,
                ImagePath = o.ImagePath,
                ObjectId = o.ObjectId,
            }).ToList();
            return list;
        }

        public async Task DeleteAllObjects()
        {
            var query = await this.connection.QueryAsync<Objectt>("delete from [Objectt]");
        }
    }
}
