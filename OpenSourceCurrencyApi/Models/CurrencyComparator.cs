using OpenSourceCurrencyApi.Structures;
using System.Collections.Generic;

namespace OpenSourceCurrencyApi.Models
{
    public class CurrencyComparator
    {
        public string Date { get; set; }
        public CircularLinkedList<CurrencyPrice> CurrencyBasePriceList { get; set; }
    }
}
