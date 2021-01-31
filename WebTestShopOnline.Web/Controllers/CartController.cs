using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTestShopOnline.CartToBuy;
using WebTestShopOnline.Data;
using WebTestShopOnline.Entity;
using WebTestShopOnline.Page;
using WebTestShopOnline.Web.ConnectAPI.UserAPICN;

namespace WebTestShopOnline.Web.Controllers
{
    public class CartController : Controller
    {
       
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context)
        {
          
            _context = context;
        }
      
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult BuyFinish()
        {
            
            return View();
        }
        public IActionResult HistoryCartBuy()
        {
            
            return View();
        }
        
        public JsonResult CartBuy(CreatBuyCart request)
        {
            var getall = HttpContext.Session.GetString("CartRequest");
            var listcart = JsonConvert.DeserializeObject<List<CartItem>>(getall);
            var order = new Order();
            order.Name = request.Name;
            order.Phone = request.Phone;
            order.Adress = request.Adress;
            order.Email = request.Email;
            order.Contact = request.Contact;
            
            var listorderDetail = new List<OrderDetail>();
           
            for(int i = 0; i < listcart.Count(); i++)
            {
                listorderDetail.Add(new OrderDetail()
                {
                    ColorProduct = listcart[i].Color,
                    NameProduct = listcart[i].Product.Name,
                    PriceProduct = listcart[i].Product.Price,
                    PathImage = listcart[i].Product.PathImage,
                });
            }
            order.OrderDetails = listorderDetail;
            _context.Orders.Add(order);
            _context.SaveChanges();
            return Json(new
            {
                status = true,

            });
        }
        public JsonResult DeleteAll()
        {
            var getall = HttpContext.Session.GetString("CartRequest");
            var listcart = JsonConvert.DeserializeObject<List<CartItem>>(getall);
            for(int i = 0; i < listcart.Count(); i++)
            {
                listcart.Remove(listcart[i]);
            }
            HttpContext.Session.SetString("CartRequest", JsonConvert.SerializeObject(listcart));
            return Json(new
            {
                status = true
            });
        }
        public JsonResult UpdateCart(string ValueId)
        {
            var getall = HttpContext.Session.GetString("CartRequest");
            var listcart = JsonConvert.DeserializeObject<List<CartItem>>(getall);
            var listcartUpdate = JsonConvert.DeserializeObject<List<CartItemRequest>>(ValueId);
            foreach(var cart in listcartUpdate)
            {
                var cartItem = listcart.SingleOrDefault(x => x.Product.Id == cart.Id);
                if (cartItem != null)
                {
                    cartItem.Quantity = cart.Quantity;
                }
                HttpContext.Session.SetString("CartRequest", JsonConvert.SerializeObject(listcart));
            }
            return Json(new
            {
                status = true,

            });
        }
        public async Task<IActionResult> AddToCart(int quantity, int Id, string Color)
        {
            var product = await _context.Products.FindAsync(Id);
            var getall = HttpContext.Session.GetString("CartRequest");
            if (getall != null)
            {
                var listproduct = JsonConvert.DeserializeObject<List<CartItem>>(getall);
                if (listproduct.Exists(x => x.Product.Id == Id&&x.Color==Color))
                {
                    foreach (var cart in listproduct)
                    {
                        if (cart.Product.Id == Id && cart.Color == Color)
                        {
                            cart.Quantity = cart.Quantity + quantity;

                        }
                    }
                }
                else
                {
                    var cartnew = new CartItem()
                    {
                        Product = product,
                        Quantity = quantity,
                        Color = Color
                    };
                    listproduct.Add(cartnew);

                }
                HttpContext.Session.SetString("CartRequest", JsonConvert.SerializeObject(listproduct));
            }
            else
            {

                var listcartItem = new List<CartItem>();
                var cartItem = new CartItem()
                {
                    Product = product,
                    Quantity = quantity,
                    Color = Color,
                };
                listcartItem.Add(cartItem);
                HttpContext.Session.SetString("CartRequest", JsonConvert.SerializeObject(listcartItem));
            }
            return RedirectToAction("GetAllCart", "Cart");

        }

        public IActionResult GetAllCartUpdate(string keyword, int pageIndex = 1, int pageSize = 5)
        {
           
            var request = new PageRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            };
            var getall = HttpContext.Session.GetString("CartRequest");
            var listcart = JsonConvert.DeserializeObject<List<CartItem>>(getall);

            var product = from p in listcart
                          select new { p };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                product = product.Where(x => x.p.Product.Name.Contains(request.Keyword));
            }
            var total = product.Count();
            var vt = product.Skip((request.PageIndex - 1) * (request.PageSize)).Take(request.PageSize)
                .Select(x => new CartItem()
                {
                    Quantity = x.p.Quantity,
                    Product = x.p.Product,
                    Color = x.p.Color
                }).ToList();
            var page = new PagedResult<CartItem>()
            {
                Items = vt,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = total
            };
            return View(page);

        }
        public IActionResult GetAllCart(string keyword,int pageIndex=1,int pageSize =5)
        {
            var user = User.Identity.Name;
            ViewBag.User = user;
            var request = new PageRequest()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = keyword
            };
            var getall = HttpContext.Session.GetString("CartRequest");
            if (getall == null)
            {
                return RedirectToAction("NotFindCart", "Cart");
            }
            var listcart = JsonConvert.DeserializeObject<List<CartItem>>(getall);


            var product = from p in listcart
                          select new { p };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                product = product.Where(x => x.p.Product.Name.Contains(request.Keyword));
            }
            var total = product.Count();
            var vt = product.Skip((request.PageIndex - 1) * (request.PageSize)).Take(request.PageSize)
                .Select(x => new CartItem()
                {
                    Quantity = x.p.Quantity,
                    Product = x.p.Product,
                    Color=x.p.Color
                }).ToList();
            var page = new PagedResult<CartItem>()
            {
                Items = vt,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = total
            };
            return View(page);

        }

        [Route("Cart/AddCart/{Id}&&{quantity}&&{Color}")]
        public async Task<IActionResult> AddCart(int quantity,int Id,string Color)
        {
            var product = await _context.Products.FindAsync(Id);
            var getall = HttpContext.Session.GetString("CartRequest");
            if (getall != null)
            {
                var listproduct = JsonConvert.DeserializeObject<List<CartItem>>(getall);
                if (listproduct.Exists(x => x.Product.Id == Id))
                {
                    foreach (var cart in listproduct)
                    {
                        if (cart.Product.Id == Id && cart.Color==Color)
                        {
                            cart.Quantity = cart.Quantity + quantity;

                        }
                    }
                }
                else
                {
                    var cartnew = new CartItem()
                    {
                        Product = product,
                        Quantity = quantity,
                        Color = Color
                    };
                    listproduct.Add(cartnew);
                   
                }
                HttpContext.Session.SetString("CartRequest", JsonConvert.SerializeObject(listproduct));
            }
            else
            {
               
                var listcartItem = new List<CartItem>();
                var cartItem = new CartItem()
                {
                    Product = product,
                    Quantity = quantity,
                    Color=Color,
                };
                listcartItem.Add(cartItem);
                HttpContext.Session.SetString("CartRequest", JsonConvert.SerializeObject(listcartItem));
            }
            return RedirectToAction("GetAllCart", "Cart");
          
        }
        public JsonResult DeleteCart(int Id)
        {
            var getall = HttpContext.Session.GetString("CartRequest");
            var listcart = JsonConvert.DeserializeObject<List<CartItem>>(getall);
            if (listcart.Exists(x => x.Product.Id == Id))
            {
                for(int i = 0; i < listcart.Count(); i++)
                {
                    if (listcart[i].Product.Id == Id)
                    {
                        listcart.Remove(listcart[i]);
                    }
                }
            }
            HttpContext.Session.SetString("CartRequest", JsonConvert.SerializeObject(listcart));
            return Json(new
            {
                status = true,
            });
        }
        public JsonResult Remove(int Id)
        {
            var getall = HttpContext.Session.GetString("CartRequest");
            var listcart = JsonConvert.DeserializeObject<List<CartItem>>(getall);
            if (listcart.Exists(x => x.Product.Id == Id))
            {
                foreach (var cart in listcart)
                {
                    if (cart.Product.Id == Id)
                    {
                        cart.Quantity = cart.Quantity - 1;
                    }
                }
            }
            HttpContext.Session.SetString("CartRequest", JsonConvert.SerializeObject(listcart));
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Add(int Id)
        {
            var getall = HttpContext.Session.GetString("CartRequest");
            var listcart = JsonConvert.DeserializeObject<List<CartItem>>(getall);
            if (listcart.Exists(x => x.Product.Id == Id))
            {
                foreach(var cart in listcart)
                {
                    if (cart.Product.Id == Id)
                    {
                        cart.Quantity = cart.Quantity + 1;
                    }
                }
            }
            HttpContext.Session.SetString("CartRequest", JsonConvert.SerializeObject(listcart));
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Danger(int Id)
        {
            var getall = HttpContext.Session.GetString("CartRequest");
            var listcart = JsonConvert.DeserializeObject<List<CartItem>>(getall);
            if (listcart.Exists(x => x.Product.Id == Id))
            {
               for(int i = 0; i < listcart.Count(); i++)
                {
                    listcart.Remove(listcart[i]);
                }
            }
            HttpContext.Session.SetString("CartRequest", JsonConvert.SerializeObject(listcart));
            return Json(new
            {
                status = true
            });
        }
        public IActionResult NotFindCart()
        {
            return View();
        }
       
    }
    
         
}
