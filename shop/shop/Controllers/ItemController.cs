using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace shop
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        ApplicationContext db;

        public ItemController(ApplicationContext context)
        {
            db = context;
        }

        [HttpGet("getitemsshop")]
        public async Task<ActionResult<ItemShop[]>> GetItemsShop()
        {
            return Ok(db.ItemsShop.ToList());
        }

        [HttpPost("additem")]
        public async Task<ActionResult<Item>> AddItem(Item itemData)
        {
            Basket? basket = GetBasket();

            if (basket == null) return NotFound("Basket not found");

            ItemShop? shop = db.ItemsShop.FirstOrDefault(x => x.Name == itemData.Name);

            if (shop == null) return BadRequest();

            Item? item = new Item();

            item.Name = shop.Name;
            item.Price = shop.Price;
            item.Basket = basket;

            db.Items.Add(item);
            await db.SaveChangesAsync();
            return Ok(item);
        }


        [HttpDelete("deleteitem/{id:int}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            Basket? basket = GetBasket();

            Item? item = db.Items.FirstOrDefault(x => x.Id == id && basket.Id == x.BasketId);

            if (item == null) return BadRequest("item not found");

            db.Items.Remove(item);
            await db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpGet("getitem/{id:int}")]
        public ActionResult GetItem(int id)
        {
            Basket? basket = GetBasket();

            if (basket == null) return NotFound("Basket not found");

            Item? item = db.Items.FirstOrDefault(x => x.Id == id && basket.Id == x.BasketId);
            return Ok(item);
        }

        [HttpGet("getitems")]
        public async Task<ActionResult<Item[]>> GetItems()
        {
            Basket? basket = GetBasket();

            if (basket == null) return NotFound("Basket not found");

            var items = db.Items.Where(x => x.BasketId == basket.Id).ToList();

            return Ok(items);
        }

        [HttpPut("updateitem")]
        public async Task<ActionResult> UpdateItem(Item itemData)
        {
            Basket? basket = GetBasket();

            if (basket == null) return NotFound("Basket not found");

            Item? item = db.Items.FirstOrDefault(x => x.Id == itemData.Id && basket.Id == x.BasketId);

            if (item == null) return BadRequest("item not found");

            item.Name = itemData.Name;
            item.Price = itemData.Price;
            db.Items.Update(item);
            await db.SaveChangesAsync();
            return Ok(item);
        }
        [HttpGet("getshop")]
        public async Task<ActionResult<ItemShop[]>> GetShop([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = db.ItemsShop
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();
            var totalRecords = db.ItemsShop.Count();
            return Ok(new PagedResponse<List<ItemShop>>(pagedData, validFilter.PageNumber, validFilter.PageSize));
        }

        [HttpGet("getshop/{id:int}")]
        public async Task<ActionResult> GetOneShop(int id)
        {
            ItemShop? item = db.ItemsShop.FirstOrDefault(x => x.Id == id);

            if (item == null) return BadRequest("item not found");

            return Ok(item);
        }
        [HttpGet("sortbyprice")]
        public async Task<ActionResult<ItemShop[]>> SortShop()
        {
            var shop = db.ItemsShop.OrderByDescending(x => x.Price).ToList();
            return Ok(shop);
        }
        [HttpGet("sortshopbyalphabet")]
        public async Task<ActionResult<ItemShop[]>> SortShopByAplphabet()
        {
            var shop = db.ItemsShop.OrderBy(x => x.Name).ToList();
            return Ok(shop);
        }

        public Basket GetBasket()
        {
            // if (User.Identity == null) return NotFound("jwt");

            User? user = db.Users.FirstOrDefault(x => x.Email == User.Identity.Name);

            // if (user == null) return NotFound("User not found");

            Basket? basket = db.Baskets.FirstOrDefault(x => x.Id == user.Id);

            // if (basket == null) return NotFound("Basket not found");
            return basket;
        }
    }
}