using System.Text.Json.Serialization;

namespace shop
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public int BasketId { get; set; }
        [JsonIgnore]
        public Basket? Basket { get; set; }
    }
}
