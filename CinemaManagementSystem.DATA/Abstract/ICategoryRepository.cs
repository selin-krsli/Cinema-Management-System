using CinemaManagementSystem.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.DATA.Abstract
{
    public interface ICategoryRepository:IRepository<Category>
    {
        Category GetByIdWithMovies(int categoryId);
    }
}
