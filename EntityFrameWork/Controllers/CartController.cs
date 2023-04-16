using EntityFrameWork.Data;
using EntityFrameWork.Models;
using EntityFrameWork.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EntityFrameWork.Controllers
{
    public class CartController : Controller
    {


        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> Index()
        {

            List<BasketVM> basketProducts= GetBasketDatas();
          
            List<BasketDetailVM> basketDetails = new();

            foreach (var item in basketProducts)
            {
                Product? dbProduct = await _context.Products.Include(m => m.Images).FirstOrDefaultAsync(m => m.Id == item.Id);

                basketDetails.Add(new BasketDetailVM
                {
                    Id = dbProduct.Id,
                    Name=dbProduct.Name,
                    Description=dbProduct.Description,
                    Price=dbProduct.Price,
                    Count=item.Count,
                    Image=dbProduct.Images.Where(m=>m.IsMain).FirstOrDefault()?.Image,
                    Total=item.Count*dbProduct.Price

                });
            }
             
            return View(basketDetails);
        }


        public List<BasketVM> GetBasketDatas()
        {
            List<BasketVM> basket;

            if (Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketVM>();
            }


            return basket;
        }

        [ActionName("Delete")]
        public IActionResult DeleteProductFromBasket(int? id)
        {

            if (id is null) return BadRequest();

            List<BasketVM>? basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);

            BasketVM? deletedProduct = basketProducts.FirstOrDefault(m => m.Id == id);

            basketProducts.Remove(deletedProduct);

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketProducts));

            return Ok(basketProducts);
        }



        public IActionResult MinusProductFromBasket(int? id)
        {
            if (id is null) return BadRequest();
            List<BasketVM>? basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            var basketCount = basketProducts.FirstOrDefault(m => m.Id == id).Count--;
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketProducts));
            return Ok(basketCount);


        }

        public IActionResult PlusProductFromBasket(int? id)
        {
            if (id is null) return BadRequest();
            List<BasketVM>? basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            var basketCount = basketProducts.FirstOrDefault(m => m.Id == id).Count++;
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketProducts));
            return Ok(basketCount);


        }

    }

}
