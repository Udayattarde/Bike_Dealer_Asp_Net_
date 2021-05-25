using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bike_Dealer.Models.ViewModels
{
    public class BikeViewModel
    {
        public Bike Bike { get; set; }
        public IEnumerable<Make> Makes { get; set; }
        public IEnumerable<Model> Models { get; set; }
        public IEnumerable<Currency> Currencies { get; set; }
        private List<Currency> list = new List<Currency>();

        private List<Currency> createCurrency()
        {
            list.Add(new Currency("India", "Ruppee"));
            list.Add(new Currency("USA", "USD"));
            list.Add(new Currency("Jpan", "Yen"));
            return list;

        }

        public BikeViewModel()
        {
            Currencies = createCurrency();
        }

    }

    public class Currency
    {
      public string id { get; set; }
        public string Name { get; set; }
        public Currency(string id,string Name)
        {
            this.id=id;
           this.Name = Name;
        }
    }
}
