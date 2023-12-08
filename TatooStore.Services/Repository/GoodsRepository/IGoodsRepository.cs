using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatooStore.Models.DataModels;
using TatooStore.Models.ViewModels;
using TatooStore.Models.ViewModels.CustomerPages;
using TatooStore.Models.ViewModels.EmployeePages.OrderList;

namespace TatooStore.Services.Repository.GoodsRepository
{
    public interface IGoodsRepository
    {
        Task<string> UploadImageAsync(IFormFile uploadedImage, string startPath, string pathType);
        Task<int> AddCategoryAsync(Category category);
        Task<int> AddManufactureAsync(Manufacture manufacture);
        Task<List<Manufacture>> GetAllManufacturesAsync();
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Good> AddGoodAsync(Good good);
        Task<int> GetCountGoodsAsync();
        Task<List<Good>> GetGoodsPartiallyAsync(int count, int? lastId, string? nameGood); 
        Task<Good> GetGoodByIdAsync(int id);
        Task<int> ChangeGoodAsync(Good good);
        Task<PriceChange> AddPriceChangeAsync(PriceChange priceChange);
        Task<Supply> AddSupplyAsync(Supply supply);
        Task<int> GetGoodCountByIdAsync(int id);
        Task<List<Good>> GetGoodsPartiallyByCategoryAsync(int categoryId);
        Task<BasketVM> AddGoodToBasketAsync(BasketVM basketVM, int goodId);
        Task<List<ErrorBasketVm>> CheckCountOfGoodsAsync(BasketVM basket);
        Task<int> AddPurchaseAsync(BasketVM basket, int customerId);
        Task<PurchaseHistoryVM> GetPurchaseHistoryAsync(int customerId);
        Task<List<OrderListVM>> GetOrderListAsync();
        Task<OrderListItemVM> GetOrderListItemByIdAsync(int purchaseId);
        Task<int> AddEmployeeToPurchaseAsync(int employeeId, int purchaseId);
    }
}
