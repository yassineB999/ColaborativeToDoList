using CollaborativeToDoList.Models;

namespace CollaborativeToDoList.Repository.CategoriesRepos
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Categories>> GetAllCategories();
        Task<Categories> GetCategoryByName(string name);
    }
}
