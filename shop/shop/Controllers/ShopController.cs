using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace shop
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class ShopController : ControllerBase
    {
        ApplicationContext db;

        public ShopController(ApplicationContext context)
        {
            db = context;
        }

        [HttpPost("addshop")]
        public async Task<ActionResult<ItemShop>> AddShop(ItemShop model)
        {
            ItemShop? item = new ItemShop();

            item.Name = model.Name;
            item.Price = model.Price;

            db.ItemsShop.Add(item);
            await db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("deleteshop/{id:int}")]
        public async Task<ActionResult<ItemShop>> DeleteShop(int id)
        {
            ItemShop? item = db.ItemsShop.FirstOrDefault(x => x.Id == id);

            if (item == null) return BadRequest("item not found");

            db.ItemsShop.Remove(item);
            await db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpPut("updateshop")]
        public async Task<ActionResult> UpdateShop(ItemShop itemData)
        {
            ItemShop? item = db.ItemsShop.FirstOrDefault(x => x.Id == itemData.Id);

            if (item == null) return BadRequest("item not found");

            item.Name = itemData.Name;
            db.ItemsShop.Update(item);
            await db.SaveChangesAsync();
            return Ok(item);
        }
    }
}