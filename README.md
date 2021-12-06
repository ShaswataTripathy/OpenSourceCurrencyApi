# OpenSourceCurrencyApi
This APi uses https://github.com/fawazahmed0/currency-api   to get the data 

**Get All Currencies**
https://open-source-currency-api.herokuapp.com/currency/currencies
this end point will giev all the currenices available wih thier short and long form

**Get Currency Comparison result with Date**
https://open-source-currency-api.herokuapp.com/currency/comparison/{currencyshortform}
here you can give the currency to fetch the comparison with rest of currenices in terms of exhcnage price
and the date on which it was compared
example 
https://open-source-currency-api.herokuapp.com/currency/comparison/aud


**Get currency Comparison result as CSV file**
https://open-source-currency-api.herokuapp.com/currency/comparison/download/{currencyShortForm}
this endpoint will give the currency comaprison in csv file .
the name of the will contain the date , on which the prices were compared 
