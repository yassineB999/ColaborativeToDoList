using CollaborativeToDoList.Models;

namespace CollaborativeToDoList.Service.CategoriesService
{
    public interface ICategoriesService
    {
        Task<IEnumerable<Categories>> GetAllCategories();
        Task<Categories> GetCategoryByName(string name);
    }
}
