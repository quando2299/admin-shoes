using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AdminShoesStore.Data;

namespace AdminShoesStore.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ProductsController : Controller
    {
        private ShoesStoreContext _context;

        public ProductsController(ShoesStoreContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var products = _context.Products.Select(i => new {
                i.Id,
                i.Name,
                i.Price,
                i.Descriptions,
                i.BranchId,
                i.CategoryId
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(products, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Product();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Products.Add(model);
            await _context.SaveChangesAsync();

            return Json(result.Entity.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Products.FirstOrDefaultAsync(item => item.Id == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key) {
            var model = await _context.Products.FirstOrDefaultAsync(item => item.Id == key);

            _context.Products.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> BranchesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Branches
                         orderby i.Name
                         select new {
                             Value = i.Id,
                             Text = i.Name
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> CategoriesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Categories
                         orderby i.Name
                         select new {
                             Value = i.Id,
                             Text = i.Name
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Product model, IDictionary values) {
            string ID = nameof(Product.Id);
            string NAME = nameof(Product.Name);
            string PRICE = nameof(Product.Price);
            string DESCRIPTIONS = nameof(Product.Descriptions);
            string BRANCH_ID = nameof(Product.BranchId);
            string CATEGORY_ID = nameof(Product.CategoryId);

            if(values.Contains(ID)) {
                model.Id = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(NAME)) {
                model.Name = Convert.ToString(values[NAME]);
            }

            if(values.Contains(PRICE)) {
                model.Price = Convert.ToInt32(values[PRICE]);
            }

            if(values.Contains(DESCRIPTIONS)) {
                model.Descriptions = Convert.ToString(values[DESCRIPTIONS]);
            }

            if(values.Contains(BRANCH_ID)) {
                model.BranchId = values[BRANCH_ID] != null ? Convert.ToInt32(values[BRANCH_ID]) : (int?)null;
            }

            if(values.Contains(CATEGORY_ID)) {
                model.CategoryId = values[CATEGORY_ID] != null ? Convert.ToInt32(values[CATEGORY_ID]) : (int?)null;
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}