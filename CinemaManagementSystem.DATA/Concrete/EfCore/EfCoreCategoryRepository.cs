using CinemaManagementSystem.DATA.Abstract;
using CinemaManagementSystem.ENTITY;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.DATA.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category, CMSYSTEMContext>, ICategoryRepository
    {
        public Category GetByIdWithMovies(int categoryId)
        {
            using(var context = new CMSYSTEMContext())
            {
                return context.Categories
                        .Where(i => i.CategoryId == categoryId)
                        .Include(i => i.MovieCategories)
                        .ThenInclude(i => i.Movie)
                        .FirstOrDefault();
            }
        }

    }
}
