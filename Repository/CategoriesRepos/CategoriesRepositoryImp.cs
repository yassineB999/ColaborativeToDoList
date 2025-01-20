using CollaborativeToDoList.Data;
using CollaborativeToDoList.Models;
using Microsoft.EntityFrameworkCore;

namespace CollaborativeToDoList.Repository.CategoriesRepos
{
    public class CategoriesRepositoryImp : ICategoriesRepository
    {
        private readonly TodoListDbContext _db;

        public CategoriesRepositoryImp(TodoListDbContext db)
        {
            _db = db;   
        }

        public async Task<IEnumerable<Categories>> GetAllCategories()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<Categories> GetCategoryByName(string name)
        {
            var categoriesName = await _db.Categories.FirstOrDefaultAsync(c => c.Name == name);
            if(categoriesName == null)
            {
                throw new Exception("not found");
            }
            return categoriesName;
        }
    }
}
