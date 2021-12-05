using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace OpenSourceCurrencyApi.Models
{
    public class CurrencyPrice : IComparable<CurrencyPrice> 
    {
        public string CurrencyShortName { get; set; }
        public float ExchangePrice { get; set; }

        public int CompareTo([AllowNull] CurrencyPrice other)
        {
            if (other == null)
            {
                return 0;
            }


            if (other.ExchangePrice > this.ExchangePrice)
            {
                return 1;
            }
            else if (other.ExchangePrice == this.ExchangePrice)
            {
                return other.CurrencyShortName.CompareTo(this.CurrencyShortName);
            }
            else
            {
                return -1;
            }
        }
    }


}


