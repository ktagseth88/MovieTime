using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTime.ViewModels.Overwatch
{
    public class CreateMatchViewModel
    {
        public IEnumerable<SelectListItem> Map { get; set; }
        public string MapId { get; set; }
        public bool IsVictory { get; set; }
        public IEnumerable<SelectListItem> FirstHealer { get; set; }
        public string FirstHealerId { get; set; }
        public IEnumerable<SelectListItem> SecondHealer { get; set; }
        public string SecondHealerId { get; set; }
        public IEnumerable<SelectListItem> FirstTank { get; set; }
        public string FirstTankId { get; set; }
        public IEnumerable<SelectListItem> SecondTank { get; set; }
        public string SecondTankId { get; set; }
        public IEnumerable<SelectListItem> FirstDps { get; set; }
        public string FirstDpsId { get; set; }
        public IEnumerable<SelectListItem> SecondDps { get; set; }
        public string SecondDpsId { get; set; }
    }
}
