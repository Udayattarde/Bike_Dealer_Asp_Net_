using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bike_Dealer.Models.ViewModels
{
    public class ViewModel
    {
        public Model Models { get; set; }
        public IEnumerable<Make> Makes { get; set; }

        public IEnumerable<SelectListItem> cselectlistitem(IEnumerable<Make> items)
        {
            List<SelectListItem> makelist = new List<SelectListItem>();
            SelectListItem sli = new SelectListItem();
            sli = new SelectListItem()
            {
                Text = "select a value",
                Value="0"
            };
            makelist.Add(sli);

            foreach (Make make in items)
            {
                sli = new SelectListItem
                {
                    Text = make.GetType().GetProperty("Name").GetValue(make, null).ToString(),
                    Value = make.GetType().GetProperty("id").GetValue(make, null).ToString()
                };
                  makelist.Add(sli);
            }
                 return  makelist;

        }
    }  
}
