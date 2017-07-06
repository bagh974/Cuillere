﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cuillere.Models
{
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
        public List<Ingredient> Ingredients { get; set; }
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

    public enum Unit
    {
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
        public List<Recette> Recettes { get; set; }
    }

    public class Saison
    {
        public int SaisonId { get; set; }
        [Display(Name = "Saison")]
        public string Name { get; set; }
        public List<Recette> Recettes { get; set; }
    }

    public class Order
    {
        [Key]
        [ScaffoldColumn(false)]
        public int OrderId { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
    }

    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }
        [Display(Name = "Ingrédient")]
        public int IngredientId { get; set; }

        public int Quantity { get; set; }

        public virtual Ingredient Ingredient { get; set; }

        public virtual Order Order { get; set; }
    }

    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        public string CartId { get; set; }
        public int IngredientId { get; set; }
        public int Count { get; set; }

        public virtual Ingredient Ingredient { get; set; }
    }
}