using System.Collections.Generic;

namespace OpenSourceCurrencyApi.Models
{
    public class CurrencyComparator
    {
        public string Date { get; set; }
        public List<CurrencyPrice> CurrencyBasePriceList { get; set; }
    }
}
