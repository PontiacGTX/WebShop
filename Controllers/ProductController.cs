using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Models;
using WebShop.Repository;
using WebShop.ViewModels;

namespace WebShop.Controllers
{
    public class ProductController : Controller
    {
        public ProductRepository _ProductRepository { get; set; }
        public CategoryRepository _CategoryRepository { get; set; }
        public ProductController(ProductRepository productRepository,CategoryRepository categoryRepository)
        {
            _ProductRepository = productRepository;
            _CategoryRepository = categoryRepository;
        }
        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            return View(await _ProductRepository.GetProducts());
        }
        [HttpGet]
        public async Task<IActionResult> ProductDetails([Required]string ProdutId)
        {
            if(ModelState.IsValid)
            { 
                ProductViewModel product = new ProductViewModel();
                try
                {
                    if (Guid.TryParse(ProdutId, out Guid productId))
                    {
                        product.Product = await _ProductRepository.GetProduct(productId);
                        product.Categories = await _ProductRepository.GetProductCategories(productId);
                    }
                    return View(product);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"{ex.Message} {ex.InnerException}";
                    return View("Error500");
                }

            }
            ViewBag.ErrorTitle = $"ProductId is not valid";
            string errors = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            ViewBag.ErrorBody = $"Please,Try again,There were some errors:  {errors}";

            return View("ProductList");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteProduct([FromQuery]string ProductId)
        {
            if(ModelState.IsValid)
            {
                if (Guid.TryParse(ProductId, out Guid productId))
                {
                    if(! await _ProductRepository.Exists(productId))
                    {
                        return View("Error404");
                    }

                    bool deleted = await _ProductRepository.DeleteProduct(productId);

                    return RedirectToAction("ProductList");
                }
                return View("Error");
            }
            ViewBag.ErrorTitle = $"ProductId {ProductId} is not a valid string";
            ViewBag.ErrorBody = "Select a Different Product";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory([FromQuery] string CategoryId)
        {
            if (ModelState.IsValid)
            {
                if (Guid.TryParse(CategoryId, out Guid categoryId))
                {
                    if (!await _ProductRepository.Exists(categoryId))
                    {
                        return View("Error404");
                    }

                    bool? deleted = await _CategoryRepository.DeleteCategory(categoryId);

                    return RedirectToAction("CategorytList");
                }
                return View("Error");
            }
            ViewBag.ErrorTitle = $"CateogoryId '{CategoryId}' is not a valid string";
            ViewBag.ErrorBody = "Select a Different Product";
            return View();
        }

        [HttpGet]
        async Task<IActionResult> EditCategory([FromQuery]string CategoryId)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    if(Guid.TryParse(CategoryId, out Guid categoryId))
                    {
                         Category category=  await _CategoryRepository.GetCategory(categoryId);
                        return View("EditCategory",category);
                    }

                    return View("Error500");
                }
                catch (Exception ex)
                {
                    return View("Error500");
                }
            }
            ViewBag.ErrorTitle = $"CategoryId '{CategoryId}' is not valid";
            return View();
        }

        [HttpPost]
        public  async Task<IActionResult> EditCategory([FromBody]Category category)
        {
           if(ModelState.IsValid)
            {
                try
                {
                    bool exists =await _CategoryRepository.Exist(category.CategoryId);
                    if(!exists)
                    {
                        return View("Error404");
                    }
                    await _CategoryRepository.UpdateCategory(category, category.CategoryId);

                    return RedirectToAction("CategoryList");
                }
                catch (Exception)
                {
                    return View("Error500");
                }
            }

            return View(category);
        }


        [HttpGet]
        public async Task<IActionResult> EditProduct([Required][FromQuery]string ProductId)
        {
            if(ModelState.IsValid)
            {
                if(Guid.TryParse(ProductId, out Guid productId))
                {
                   if (! await _ProductRepository.Exists(productId))
                   {
                        return View("Error404");
                   }

                    Product prod = await _ProductRepository.GetProduct(productId);
                    
                   return View("Edit",new EditProductViewModel { Product = prod, ProductId = prod.ProductId , Images =await _ProductRepository.GetProductImages(productId)});

                }
                return View("Error");
            }
            ViewBag.ErrorTitle = $"ProductId '{ProductId}' is not a valid string";
            ViewBag.ErrorBody = "Select a Different Product";
            return View();
        }

        [HttpGet] 
        public IActionResult AddCategory()
        {
            return View(new AddCategoryViewModel { CategoryId = "00000000-0000-0000-0000-000000000000" });
        }

        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            return View(await  _CategoryRepository.GetCategories());
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromForm] AddCategoryViewModel category)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    Category categoryReturn = await _CategoryRepository.AddCategory(new Category { CategoryName = category.CategoryName });
                    if (categoryReturn is null)
                    {
                        TempData["ErrorMessage"] = $"Couldn't create category {category.CategoryName}";
                        return View("Error500");
                    }

                    return View("CategoryList");
                }
                catch (Exception ex)
                {
                    return View("Error500");
                }
            }
            ViewBag.ErrorTitle = $"Category {category.CategoryName} is not a valid model";
            ViewBag.ErrorBody = "Error while adding a new category";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            var catList = (await _CategoryRepository.GetCategories());
            return View(new AddProductViewModel {ProductId = "00000000-0000-0000-0000-000000000000", Categories = catList.Any()? catList.Select((val,idx) => new CategorySelectModel { CategoryId =val.CategoryId.ToString(), CategoryName = val.CategoryName  }).ToList(): new List<CategorySelectModel>() }  );
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] AddProductViewModel model)
        {

           
            if(ModelState.IsValid)
            {
                try
                {
                     Product p =  await _ProductRepository.AddProduct(
                     new Product
                     {
                         Enabled = model.Enabled, 
                         HasCategory =model.Categories.Any(), 
                         Price = model.Price,
                         ProductName = model.ProductName,
                         Quantity=model.Quantity
                     });
                   
                    if(p is null)
                    {
                        TempData["ErrorMessage"] = "Couldnt store the product you have sent";
                        model.Categories = (await _CategoryRepository.GetCategories()).Select(x => new CategorySelectModel { CategoryId = x.CategoryId.ToString(), CategoryName = x.CategoryName }).ToList();
                        return View(model);
                    }

                    
                    if (model.Categories.Any())
                    {
                        await _CategoryRepository.AddProductCategories(model.Categories, p);
                    }

                    return View("ProductList", await _ProductRepository.GetProducts());
                }
                catch (Exception)
                {
                    return View("Error500");
                }
            }

            ViewBag.ErrorTitle = $"Product is not valid";
            string errors =string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            ViewBag.ErrorBody = $"Please,Try again,There were some errors:  {errors}";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct([FromBody]EditProductViewModel product)
        {
            if(ModelState.IsValid)
            {
                if(! await _ProductRepository.Exists(product.ProductId))
                {
                    return View("Error404");
                }

               await _ProductRepository.UpdateProduct(product.Product, product.ProductId);

               

            }
            ViewBag.ErrorTitle = $"Product is not a valid model";
            ViewBag.ErrorBody = "Select a Different Product";
            return View(product);
        }
    }
}
