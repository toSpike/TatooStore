using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.ComponentModel.DataAnnotations;
using TatooStore.Models.DataModels;
using TatooStore.Models.ViewModels;
using TatooStore.Services.Repository;
using TatooStore.Services.Repository.AccountRepository;
using TatooStore.Services.Repository.GoodsRepository;

namespace TatooStoreGeneral.Pages
{
    public class BasketModel : PageModel
    {
        public readonly SignInManager<AppUser> _signInManager;
        private readonly IGoodsRepository _repos;
        private readonly IAccountRepository _account;

        public BasketModel(SignInManager<AppUser> signInManager, IGoodsRepository repos, IAccountRepository account)
        {
            _signInManager = signInManager;
            _repos = repos;
            _account = account;
        }
        public BasketVM Basket { get; set; }

        public void OnGet()
        {
            Basket = HttpContext.Session.Get<BasketVM>("basket");
        }
        public async Task<IActionResult> OnPostDeleteItemAsync(int id)
        {
            Basket = HttpContext.Session.Get<BasketVM>("basket");
            int index = Basket.Goods.FindIndex(g => g.Id == id);
            Basket.Goods.RemoveAt(index);
            HttpContext.Session.Set<BasketVM>("basket", Basket);
            return Page();
        }
        public async Task<IActionResult> OnPostAddCountItemBasketAsync(int id, int value)
        {
            Basket = HttpContext.Session.Get<BasketVM>("basket");
            int index = Basket.Goods.FindIndex(g => g.Id == id);
            Basket.Goods[index].Count = value;
            HttpContext.Session.Set<BasketVM>("basket", Basket);
            return new JsonResult(new { Cost = Basket.Goods[index].Cost, TotalCost = Basket.TotalCost });
        }
        public async Task<IActionResult> OnPostPlaceOrderAsync(string address)
        {
            ErrorCountItems errorsBasket = new ErrorCountItems();
            Basket = HttpContext.Session.Get<BasketVM>("basket");
            if (_signInManager.IsSignedIn(User))
            {
                errorsBasket.Errors = await _repos.CheckCountOfGoodsAsync(Basket);

                if (errorsBasket.Errors.Count == 0)
                {
                    int id = await _account.GetCustomerIdAsync(User.Identity.Name);
                    Basket.deliveryAddress = address;
                    await _repos.AddPurchaseAsync(Basket, id);
                    HttpContext.Session.Clear();
                    return new JsonResult(new
                    {
                        Redirect = "/Result?answer=Заказ успешно оформлен",
                    });
                }
                return PartialView("_ErrorCountItems", errorsBasket); 
            }
            return new JsonResult(new
            {
                Redirect = "/Account/Login?returnUrl=/Basket",
            });

        }
        public IActionResult OnPostChangeErrorsBasket(string[] goods)
        {
            Basket = HttpContext.Session.Get<BasketVM>("basket");
            for (int i = 0; i < goods.Length; i++)
            {
                int id = Convert.ToInt32(goods[i].Split("/")[0]);
                int count = Convert.ToInt32(goods[i].Split("/")[1]);
                int index = Basket.Goods.FindIndex(g => g.Id == id);
                Basket.Goods[index].Count = count;
            }
            HttpContext.Session.Set<BasketVM>("basket", Basket);
            OnGet();
            return Page();
        }

        private IActionResult PartialView(string viewName, ErrorCountItems errorsBasket)
        {
            return new PartialViewResult()
            {
                ViewName = viewName,
                ViewData = new ViewDataDictionary<ErrorCountItems>(ViewData, errorsBasket)
            };
        }
    }
}
