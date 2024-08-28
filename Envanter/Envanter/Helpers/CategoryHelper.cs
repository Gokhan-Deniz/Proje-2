using Envanter.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Envanter.Helpers
{
    public class CategoryHelper
    {
        private readonly SQLiteAsyncConnection _database;

        public CategoryHelper(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Category>().Wait();
        }

        public Task<List<Category>> GetCategoriesAsync()
        {
            return _database.Table<Category>().ToListAsync();
        }

        public Task<int> SaveCategoryAsync(Category category)
        {
            if (category.Id != 0)
            {
                return _database.UpdateAsync(category);
            }
            else
            {
                return _database.InsertAsync(category);
            }
        }

        public Task<int> DeleteCategoryAsync(Category category)
        {
            return _database.DeleteAsync(category);
        }
    }
}
