using CinemaManagementSystem.BUSINESS.Abstract;
using CinemaManagementSystem.DATA.Concrete.EfCore;
using CinemaManagementSystem.ENTITY;
using CinemaManagementSystem.WEBUI.Extensions;
using CinemaManagementSystem.WEBUI.Identity;
using CinemaManagementSystem.WEBUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CinemaManagementSystem.WEBUI.Controllers
{
    //[Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        private IMovieService _movieService;
        private ICategoryService _categoryService;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        public AdminController(IMovieService movieService, ICategoryService categoryService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _movieService = movieService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }
        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            UserDetailsModel userDetailsModel = new UserDetailsModel();

            if (user != null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(i => i.Name);
                ViewBag.Roles = roles;

                userDetailsModel.UserId = user.Id;
                userDetailsModel.UserName = user.UserName;
                userDetailsModel.FirstName = user.FirstName;
                userDetailsModel.LastName = user.LastName;
                userDetailsModel.Email = user.Email;
                userDetailsModel.EmailConfirmed = user.EmailConfirmed;
                userDetailsModel.SelectedRoles = selectedRoles;


            }
            return View(userDetailsModel);


        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles ?? new string[] { };
                        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user, selectedRoles.Except(selectedRoles).ToArray<string>());
                        return Redirect("/Admin/UserList");
                    }
                }
                return Redirect("/Admin/UserList");
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // User not found
                return Ok("Error");
            }

            var result = await _userManager.DeleteAsync(user);
            
            return Redirect("/Admin/UserList");
        }

        [HttpGet]
        [Route("/Admin/RoleEdit/{id}")]
        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var members = new List<User>();
            var nonmembers = new List<User>();
            foreach (var user in _userManager.Users.ToList())
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonmembers;
                list.Add(user);
            }
            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonmembers
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {

            if (model.IdsToDelete == null)
                ModelState.Remove("IdsToDelete");
            else
                ModelState.Remove("IdsToAdd");


            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
            }
            return Redirect("/Admin/RoleEdit/" + model.RoleId);
        }
        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                if (result.Succeeded)
                {
                    return Redirect("/Admin/RoleList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {

                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RoleDelete(string id)
        {
 

            var role = await _roleManager.FindByNameAsync(id);
            if (role == null)
            {
                // Role not found
                return Ok("Error");
            }

            var result =   _roleManager.DeleteAsync(role);


            return Redirect("/Admin/RoleList");
        }


        public IActionResult MovieList()
        {
            return View(new MovieListViewModel()
            {
                Movies = _movieService.GetAll()
            });
        }
        public IActionResult CategoryList()
        {
            return View(new CategoryListViewModel()
            {
                Categories = _categoryService.GetAll()
            });
        }
        [HttpGet]
        public IActionResult MovieCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult MovieCreate(MovieModel model)
        {

            var entity = new Movie()
            {
                MovieName = model.MovieName,
                Url = model.Url,
                MovieStory = model.MovieStory,
                Genre = model.Genre,
                Director = model.Director,
                ImageUrl = model.ImageUrl,
            };
            _movieService.Create(entity);
            TempData.Put("message", new MessageInfo()
            {
                Title = "Record Added.",
                Message = $"{entity.MovieName} has been added.",
                AlertType = "success"
            });

            return RedirectToAction("MovieList");


        }
        public IActionResult CategoryCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel model)
        {

            var entity = new Category()
            {
                Name = model.Name,
                Url = model.Url,
            };
            _categoryService.Create(entity);
            TempData.Put("message", new MessageInfo()
            {
                Title = "Record Added.",
                Message = $"{entity.Name} has been added.",
                AlertType = "success"
            });

            return RedirectToAction("CategoryList");
        }
        [HttpGet]
        public IActionResult MovieEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _movieService.GetByIdWithCategories((int)id);
            if (entity == null)
            {
                return NotFound();
            }
            var model = new MovieModel()
            {
                MovieId = entity.MovieId,
                MovieName = entity.MovieName,
                Url = entity.Url,
                MovieStory = entity.MovieStory,
                Genre = entity.Genre,
                Director = entity.Director,
                ImageUrl = entity.ImageUrl,
                SelectedCategories = entity.MovieCategories.Select(i => i.Category).ToList()
            };
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }
        [HttpPost]
        public IActionResult MovieEdit(MovieModel model, int[] categoryIds)
        {

            var entity = _movieService.GetById(model.MovieId);
            if (entity == null)
            {
                return NotFound();
            }
            entity.MovieName = model.MovieName;
            entity.Url = model.Url;
            entity.MovieStory = model.MovieStory;
            entity.Genre = model.Genre;
            entity.Director = model.Director;
            entity.ImageUrl = model.ImageUrl;

            _movieService.Update(entity, categoryIds);

            TempData.Put("message", new MessageInfo()
            {
                Title = "Record Updated.",
                Message = $"{entity.MovieName} has been updated.",
                AlertType = "info"
            });

            return RedirectToAction("MovieList");


        }
        [HttpGet]
        public IActionResult CategoryEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _categoryService.GetByIdWithMovies((int)id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new CategoryModel()
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Movies = entity.MovieCategories.Select(m => m.Movie).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _categoryService.GetById(model.CategoryId);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Url = model.Url;

                _categoryService.Update(entity);
                TempData.Put("message", new MessageInfo()
                {
                    Title = "Record Updated.",
                    Message = $"{entity.Name} has been updated.",
                    AlertType = "info"
                });

                return RedirectToAction("CategoryList");
            }
            return View(model);
        }
        public IActionResult DeleteMovie(int movieId)
        {
            var entity = _movieService.GetById(movieId);
            if (entity != null)
            {
                _movieService.Delete(entity);
            }
            TempData.Put("message", new MessageInfo()
            {
                Title = "Record Deleted.",
                Message = $"{entity.MovieName} has been deleted.",
                AlertType = "danger"
            });

            return RedirectToAction("MovieList");
        }
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);
            if (entity != null)
            {
                _categoryService.Delete(entity);
            }
            TempData.Put("message", new MessageInfo()
            {
                Title = "Record Deleted.",
                Message = $"{entity.Name} has been deleted.",
                AlertType = "danger"
            });

            return RedirectToAction("CategoryList");
        }
        private void CreateMessage(string message, string alerttype)
        {
            var msg = new MessageInfo()
            {
                Message = message,
                AlertType = alerttype
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
        }
    }

}
