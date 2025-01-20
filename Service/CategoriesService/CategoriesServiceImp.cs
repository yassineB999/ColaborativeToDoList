using CollaborativeToDoList.Models;
using CollaborativeToDoList.Repository.CategoriesRepos;

namespace CollaborativeToDoList.Service.CategoriesService
{
    public class CategoriesServiceImp : ICategoriesService
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoriesServiceImp(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<IEnumerable<Categories>> GetAllCategories()
        {
            return await _categoriesRepository.GetAllCategories();
        }

        public async Task<Categories> GetCategoryByName(string name)
        {
            return await _categoriesRepository.GetCategoryByName(name);
        }
    }
}
