using OpenSourceCurrencyApi.Structures;
using System.Collections.Generic;

namespace OpenSourceCurrencyApi.Models
{
    /// <summary>
    /// encapsulates the whole result of comparison endpoint with date and list of
    /// currency and exhchange price
    /// </summary>
    public class CurrencyComparator
    {
        public string Date { get; set; }
        public CircularLinkedList<CurrencyPrice> CurrencyBasePriceList { get; set; }
    }
}
