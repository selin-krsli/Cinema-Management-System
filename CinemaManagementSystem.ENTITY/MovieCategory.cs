using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.ENTITY
{
    public class MovieCategory
    {
        [Key]
        public int CategoryId { get; set; }
        public Category? Category{ get; set; }
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}
