using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cuillere.Models
{
    public enum Unit
    {
        [Display(Name = "p.")]
        unite,
        g,
        Kg,
        ml,
        Cl,
        L,
        [Display(Name = "Cuillère à café")]
        cac,
        [Display(Name = "Cuillère à soupe")]
        cas,
        [Display(Name = "Pincée")]
        pince
    }
    public class Ingredient
    {
        [Key]
        [ScaffoldColumn(false)]
        public int IngredientId { get; set; }
        [Required]
        [Display(Name = "Ingrédient")]
        [StringLength(32, MinimumLength = 4)]
        public string Name { get; set; }
        [Required]
        public int RayonId { get; set; }
        public virtual Rayon Rayon { get; set; }
    }

    public class Rayon
    {
        [Key]
        [ScaffoldColumn(false)]
        public int RayonId { get; set; }
        [Required]
        [Display(Name = "Rayon")]
        [StringLength(32, MinimumLength = 4)]
        public string Name { get; set; }
        public virtual List<Ingredient> Ingredients { get; set; }
    }

    public class Recette
    {
        [Key]
        [ScaffoldColumn(false)]
        public int RecetteId { get; set; }
        [Required]
        [Display(Name = "Recette")]
        [StringLength(32, MinimumLength = 4)]
        public string Name { get; set; }
        [Display(Name = "Catégorie")]
        public int CategoryId { get; set; }
        [Display(Name = "Saison")]
        public int SaisonId { get; set; }
        [Display(Name = "Ingrédients")]
        public virtual List<RecetteDetail> RecetteDetail { get; set; }
        public virtual Category Category { get; set; }
        public virtual Saison Saison { get; set; }
    }

    //Pour ajouter les ingrédients de la recette
    public class RecetteDetail
    {
        public int RecetteDetailId { get; set; }
        [Display(Name = "Recette")]
        public int RecetteId { get; set; }
        [Display(Name = "Ingrédient")]
        public int IngredientId { get; set; }
        [Display(Name = "Quantité")]
        public int Quantity { get; set; }
        
        public Unit unite { get; set; }
        public virtual Ingredient Ingredient { get; set; }

        public virtual Recette Recette { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        [Display(Name = "Catégorie")]
        public string Name { get; set; }
        public virtual List<Recette> Recettes { get; set; }
    }

    public class Saison
    {
        public int SaisonId { get; set; }
        [Display(Name = "Saison")]
        public string Name { get; set; }
        public virtual List<Recette> Recettes { get; set; }
    }

    //Ma liste de courses
    public class Order
    {
        [Key]
        [ScaffoldColumn(false)]
        public int OrderId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
    }

    //Liste de courses détaillée
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }
        [Display(Name = "Ingrédient")]
        public int IngredientId { get; set; }

        public int Quantity { get; set; }
        public Unit unite { get; set; }

        public virtual Ingredient Ingredient { get; set; }

        public virtual Order Order { get; set; }
    }

    //Elément de ma liste de courses
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public string CartId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
        [Required]
        //public string CartId { get; set; }
        public int IngredientId { get; set; }
        public int Count { get; set; }
        public Unit unite { get; set; }

        public virtual Ingredient Ingredient { get; set; }
        }
    }