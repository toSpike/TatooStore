using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TatooStore.Models.DataModels;
using TatooStore.Models.ViewModels;
using TatooStore.Services.Repository.GoodsRepository;
using TatooStore.Services.Repository;

namespace TatooStoreGeneral.Pages
{
    public class CatalogModel : PageModel
    {
        private readonly IGoodsRepository _repos;

        public CatalogModel(IGoodsRepository repos)
        {
            _repos = repos;
        }
        public List<Good> Goods { get; set; }
        public Category Category { get; set; } = new Category();
        public async Task<IActionResult> OnGetAsync(int CategoryId, string CategoryName)
        {
            Category.CategoryId = CategoryId;
            Category.CategoryName = CategoryName;
            Goods = await _repos.GetGoodsPartiallyByCategoryAsync(CategoryId);
            return Page();
        }

        public async Task<IActionResult> OnPostAddGoodToBasketAsync(int id)
        {
            BasketVM basket;
            if (HttpContext.Session.Keys.Contains("basket"))
            {
                basket = HttpContext.Session.Get<BasketVM>("basket");
                basket = await _repos.AddGoodToBasketAsync(basket, id);
                HttpContext.Session.Set<BasketVM>("basket", basket);
            }
            else
            {
                basket = new BasketVM();
                basket = await _repos.AddGoodToBasketAsync(basket, id);
                HttpContext.Session.Set<BasketVM>("basket", basket);
            }
            return new JsonResult(new {TotalCost = basket.TotalCost});   
        }
    }
}
