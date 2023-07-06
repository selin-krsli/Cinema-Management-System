using CinemaManagementSystem.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.BUSINESS.Abstract
{
    public interface ICategoryService
    {
        Category GetById(int id);
        Category GetByIdWithMovies(int categoryId);
        List<Category> GetAll();
        void Create(Category entity);
        void Update(Category entity);
        void Delete(Category entity);
    }
}
