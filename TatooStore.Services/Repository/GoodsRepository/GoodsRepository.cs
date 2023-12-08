using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatooStore.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using TatooStore.Models.ViewModels;
using TatooStore.Models.ViewModels.CustomerPages;
using TatooStore.Models.ViewModels.EmployeePages.OrderList;

namespace TatooStore.Services.Repository.GoodsRepository
{
    public class GoodsRepository : IGoodsRepository
    {
        private readonly AppDbContext _context;

        public GoodsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddEmployeeToPurchaseAsync(int employeeId, int purchaseId)
        {
            Purchase purchase = await _context.Purchases.FindAsync(purchaseId);
            purchase.EmployeeId = employeeId;
            return await _context.SaveChangesAsync();
        }

        public async Task<Good> AddGoodAsync(Good good)
        {
            await _context.Goods.AddAsync(good);
            await _context.SaveChangesAsync();
            return good;
        }

        public async Task<BasketVM> AddGoodToBasketAsync(BasketVM basketVM, int goodId)
        {
            if (basketVM.Goods != null)
            {
                int index = basketVM.Goods.FindIndex(g => g.Id == goodId);
                if (index > -1)
                {
                    basketVM.Goods[index].Count += 1;
                    return basketVM;
                }
            }
            GoodVM good = await _context.Goods.Where(g => g.GoodId == goodId)
                .Include(g => g.PriceChanges)
                .Select(g => new GoodVM
                {
                    Id = g.GoodId,
                    Name = g.GoodName,
                    Count = 1,
                    Price = (new PriceChange { NewPrice = g.PriceChanges.OrderByDescending(p => p.PriceChangeDate).Select(p => p.NewPrice).FirstOrDefault() }).NewPrice,
                })
                .FirstOrDefaultAsync();
            basketVM.Goods.Add(good);
            return basketVM;
        }

        public async Task<int> AddManufactureAsync(Manufacture manufacture)
        {
            await _context.Manufactures.AddAsync(manufacture);
            return await _context.SaveChangesAsync();
        }

        public async Task<PriceChange> AddPriceChangeAsync(PriceChange priceChange)
        {
            await _context.PriceChanges.AddAsync(priceChange);
            await _context.SaveChangesAsync();
            return priceChange;
        }

        public async Task<int> AddPurchaseAsync(BasketVM basket, int customerId)
        {
            Purchase purchase = new Purchase()
            {
                PurchaseDate = DateTime.Now,
                DeliveryAddress = basket.deliveryAddress,
                CustomerId = customerId,
            };
            purchase.PurchaseItems = new List<PurchaseItem>();
            foreach (var item in basket.Goods)
            {
                PurchaseItem purchaseItem = new PurchaseItem();
                purchaseItem.GoodId = item.Id;
                purchaseItem.GoodPrice = item.Price;
                purchaseItem.GoodCount = item.Count;
                purchase.PurchaseItems.Add(purchaseItem);
                Good good = await _context.Goods.FindAsync(item.Id);
                good.GoodCount -= item.Count;
                _context.Goods.Update(good);
            }

            await _context.Purchases.AddAsync(purchase);
            
            return await _context.SaveChangesAsync();
        }

        public async Task<Supply> AddSupplyAsync(Supply supply)
        {
            if (supply.Good != null)
            {
                supply.Good.GoodCount += supply.GoodCount;
            } else
            {
                Good good = await _context.Goods.FindAsync(supply.GoodId);
                good.GoodCount += supply.GoodCount;
                supply.Good = good; 
            }
            await _context.AddAsync(supply);
            await _context.SaveChangesAsync();
            return supply;
        }

        public async Task<int> ChangeGoodAsync(Good good)
        {
            _context.Goods.Update(good);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<ErrorBasketVm>> CheckCountOfGoodsAsync(BasketVM basket)
        {
            List<ErrorBasketVm> errorsBasket = new List<ErrorBasketVm>(); 
            foreach (var item in basket.Goods)
            {
                int count = await _context.Goods.Where(g => g.GoodId == item.Id).Select(g => g.GoodCount).FirstOrDefaultAsync();
                if (count < item.Count)
                {
                    ErrorBasketVm errorBasket = new ErrorBasketVm();
                    errorBasket.Id = item.Id;
                    errorBasket.Name = item.Name;
                    errorBasket.Count = count;
                    errorsBasket.Add(errorBasket);
                }
            }
            return errorsBasket;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }


        public async Task<List<Manufacture>> GetAllManufacturesAsync()
        {
            return await _context.Manufactures.Select(u => new Manufacture { ManufactureId = u.ManufactureId, ManufactureName = u.ManufactureName }).ToListAsync();
        }

        public async Task<int> GetCountGoodsAsync()
        {
            return await _context.Goods.CountAsync();
        }

        public async Task<Good> GetGoodByIdAsync(int id)
        {
            return await _context.Goods.Where(s => s.GoodId == id).Include(s => s.PriceChanges)
                .Select(s => new Good
                {
                    GoodId = s.GoodId,
                    GoodName = s.GoodName,
                    GoodCount = s.GoodCount,
                    GoodDescription = s.GoodDescription,
                    GoodImagePath = s.GoodImagePath,
                    ManufactureId = s.ManufactureId,
                    CategoryId = s.CategoryId,
                    PriceChanges = new List<PriceChange>
                    {
                        new PriceChange {NewPrice = s.PriceChanges.OrderByDescending(p => p.PriceChangeDate).Select(p => p.NewPrice).FirstOrDefault()}
                    },
                }).FirstOrDefaultAsync();
        }

        public async Task<int> GetGoodCountByIdAsync(int id)
        {
            return await _context.Goods.Where(g => g.GoodId == id).Select(g => g.GoodCount).FirstOrDefaultAsync();
        }

        public async Task<List<Good>> GetGoodsPartiallyAsync(int count, int? lastId, string? nameGood)
        {
            if (lastId != null && nameGood != null)
            {
                return await _context.Goods.Where(g => string.Compare(g.GoodName, nameGood) >= 0 && g.GoodId > lastId)
                .Include(g => g.PriceChanges)
                .Include(g => g.Category)
                .Include(g => g.Manufacture)
                .Select(g => new Good
                {
                    GoodId = g.GoodId,
                    GoodName = g.GoodName,
                    GoodCount = g.GoodCount,
                    PriceChanges = new List<PriceChange>
                    {
                        new PriceChange {NewPrice = g.PriceChanges.OrderByDescending(p => p.PriceChangeDate).Select(p => p.NewPrice).FirstOrDefault()}
                    },
                    Category = new Category
                    {
                        CategoryName = g.Category.CategoryName
                    },
                    Manufacture = new Manufacture
                    {
                        ManufactureName = g.Manufacture.ManufactureName
                    },
                }).OrderBy(g => g.GoodName)
                .ToListAsync();
            } else
            {
                return await _context.Goods
                .Include(g => g.PriceChanges)
                .Include(g => g.Category)
                .Include(g => g.Manufacture)
                .Select(g => new Good
                {
                    GoodId = g.GoodId,
                    GoodName = g.GoodName,
                    GoodCount = g.GoodCount,
                    PriceChanges = new List<PriceChange>
                    {
                        new PriceChange {NewPrice = g.PriceChanges.OrderByDescending(p => p.PriceChangeDate).Select(p => p.NewPrice).FirstOrDefault()}
                    },
                    Category = new Category
                    {
                        CategoryName = g.Category.CategoryName
                    },
                    Manufacture = new Manufacture
                    {
                        ManufactureName = g.Manufacture.ManufactureName
                    },
                }).OrderBy(g => g.GoodName)
                .ToListAsync();
            }
            

        }

        public async Task<List<Good>> GetGoodsPartiallyByCategoryAsync(int categoryId)
        {
            return await _context.Goods.Where(g => g.CategoryId == categoryId)
                .Include(g => g.Manufacture).Include(g => g.PriceChanges)
                .Select(g => new Good
                {
                    GoodId = g.GoodId,
                    GoodName = g.GoodName,
                    GoodCount = g.GoodCount,
                    GoodDescription = g.GoodDescription,
                    GoodImagePath = g.GoodImagePath,
                    Manufacture = g.Manufacture,
                    ManufactureId = g.ManufactureId,
                    PriceChanges = new List<PriceChange>
                    {
                        new PriceChange {NewPrice = g.PriceChanges.OrderByDescending(p => p.PriceChangeDate).Select(p => p.NewPrice).FirstOrDefault()}
                    },
                })
                .ToListAsync();
        }

        public async Task<List<OrderListVM>> GetOrderListAsync()
        {
            List<Purchase> purchases = await _context.Purchases.Include(p => p.PurchaseItems).OrderBy(p => p.EmployeeId).ToListAsync();
            List<OrderListVM> orderLists = new List<OrderListVM>();
            foreach (Purchase purchase in purchases)
            {
                orderLists.Add(new OrderListVM
                {
                    PurchaseId = purchase.PurchaseId,
                    PurchaseDate = purchase.PurchaseDate,
                    EmployeeId = purchase.EmployeeId,
                    CustomerId = purchase.CustomerId,
                });
                foreach (var item in purchase.PurchaseItems)
                {
                    orderLists.Last().TotalCost += item.GoodCount * item.GoodPrice;
                }
            }
            return orderLists;
        }

        public async Task<OrderListItemVM> GetOrderListItemByIdAsync(int purchaseId)
        {
            Purchase purchase = await _context.Purchases.Where(p => p.PurchaseId == purchaseId)
                .Include(p => p.PurchaseItems).ThenInclude(i => i.Good)
                .Include(p => p.Customer).ThenInclude(c => c.User).FirstOrDefaultAsync();
            OrderListItemVM orderListItem = new OrderListItemVM();
            orderListItem.CustomerId = purchase.CustomerId;
            orderListItem.CustomerLastName = purchase.Customer.User.LastName;
            orderListItem.CustomerFirstName = purchase.Customer.User.FirstName;
            orderListItem.CustomerMiddleName = purchase.Customer.User.MiddleName;
            orderListItem.PhoneNumber = purchase.Customer.User.PhoneNumber; 
            orderListItem.PurchaseId = purchase.PurchaseId;
            orderListItem.PurchaseDate = purchase.PurchaseDate;
            orderListItem.DeliveryAddress = purchase.DeliveryAddress;
            orderListItem.EmployeeId = purchase.EmployeeId;


            foreach (PurchaseItem item in purchase.PurchaseItems)
            {
                orderListItem.OrderItems.Add(new OrderItemVM()
                {
                    PurchaseItemId = item.PurchaseItemId,
                    GoodCount = item.GoodCount,
                    GoodPrice = item.GoodPrice,
                    GoodId = item.GoodId,
                    GoodName = item.Good.GoodName,
                });
                orderListItem.TotalCost += item.GoodCount * item.GoodPrice;
            }
            return orderListItem;
        }

        public async Task<PurchaseHistoryVM> GetPurchaseHistoryAsync(int customerId)
        {
            List<Purchase> purchases = await _context.Purchases.Where(p => p.CustomerId == customerId)
                .Include(p => p.PurchaseItems).ThenInclude(s => s.Good).OrderByDescending(p => p.PurchaseDate).ToListAsync();
            PurchaseHistoryVM purchaseHistory = new PurchaseHistoryVM();
            foreach (var purchase in purchases)
            { 
                purchaseHistory.Purchases.Add(new PurchaseVM()
                {
                    PurchaseId = purchase.PurchaseId,
                    PurchaseDate = purchase.PurchaseDate,
                    Status = purchase.EmployeeId != null ? true : false,
                    DeliveryAddress = purchase.DeliveryAddress,
                    
                });
                foreach (var item in purchase.PurchaseItems)
                {
                    PurchaseItemVM purchaseItem = new PurchaseItemVM();
                    purchaseItem.GoodId = item.GoodId;
                    purchaseItem.GoodName = item.Good.GoodName;
                    purchaseItem.GoodPrice = item.GoodPrice;
                    purchaseItem.GoodCount = item.GoodCount;
                    purchaseItem.PurchaseItemId = item.PurchaseItemId;
                    purchaseHistory.Purchases.Last().PurchaseItems.Add(purchaseItem);
                    purchaseHistory.Purchases.Last().TotalCost += item.GoodPrice * item.GoodCount;
                }
            }
            return purchaseHistory;
        }

        public async Task<string> UploadImageAsync(IFormFile uploadedImage, string startPath, string pathType)
        {
            
            string path = startPath + "\\Files\\" + pathType + "\\" + Guid.NewGuid().ToString() + "_" + uploadedImage.FileName;
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await uploadedImage.CopyToAsync(fileStream);
            }
            path = path.Split("wwwroot")[1].Replace("\\", "/");
            return path;
        }
    }
}
