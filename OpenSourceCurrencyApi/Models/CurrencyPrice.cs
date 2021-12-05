using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace OpenSourceCurrencyApi.Models
{
    /// <summary>
    /// Helps in storing the comparison result for currency and price key value pair
    /// </summary>
    public class CurrencyPrice : IComparable<CurrencyPrice> 
    {
        public string CurrencyShortName { get; set; }
        public float ExchangePrice { get; set; }

        /// <summary>
        /// Compare to method helps in sorting a collection of this class
        /// if the incoming 'other ' objects exchange price is greater than the currenct value then return 1
        /// if the incoming 'other ' objects exchange price is lesser  than the currenct value then return -1
        /// if the incoming 'other ' objects exchange price is equal  than the currenct value then return based on
        /// value of CurrencyShorName alphabetaically sorting position same principle        
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// 
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
            // if exhange price is same for both we will sort based on the curency name/alphabets sort
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


