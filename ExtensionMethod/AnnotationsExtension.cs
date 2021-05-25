using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bike_Dealer.ExtensionMethod
{
    public class AnnotationsExtension : RangeAttribute
    {
        public AnnotationsExtension(int year)
            :base(year,DateTime.Today.Year)
        {

        }
    }
}
