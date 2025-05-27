using System.ComponentModel.DataAnnotations;

namespace PeyShop.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        

        [Required]
        [StringLength(100)]
        [Display(Name = "Название товара")]
        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Количество на складе")]
        public int Quantity { get; set; }

        [StringLength(500)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Url]
        [Display(Name = "Изображение продукта")]
        public string Image { get; set; }


        [Display(Name = "Категория")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }


        [Display(Name = "Тип")]
        public int TypeOfPrTypeId { get; set; }
        public TypeOfPr TypeOfPr { get; set; }


        [Display(Name = "Фирма")]
        public int FirmId { get; set; }
        public Firm Firm { get; set; }

        public ICollection<Basket> Baskets { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

    }
}
