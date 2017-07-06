using Cuillere.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cuillere.ViewModels
{
    public class RecetteViewModel
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Saison { get; set; }
        public List<RecetteDetail> RecetteDetails { get; set; }
    }
}