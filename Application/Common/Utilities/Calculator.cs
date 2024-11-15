namespace Application.Common.Utilities;

public static class Calculator
{
    public static double CalculateWithPercent(int percent, double price)
    {
        var discount = ((price / 100) * percent);
        return price - discount;
    }

    public static double CalculateAmountWithPercent(int percent, double price)
    {
        return ((price / 100) * percent);
    }

    public static double CalculateWithPercentAndAmount(int percent, double price, double discount)
    {
        return (price - ((price / 100 * percent) - discount));
    }

    public static double CurrencyExchangeToDollar(this double amount, double exchangeRate)
    {
        return (amount / exchangeRate);
    }

    public static double CurrencyExchanger(this double amount, double exchangeRate, double exchangeRateTo)
    {
        return CurrencyExchangeToDollar(amount, exchangeRate) * exchangeRateTo;
    }

    public static double DiscountProduct(this double amount, double discount)
    {
        return amount - discount;
    }

    public static double Pass(this bool state, double amount1, double amount2)

    {
        return state ? amount1 : amount2;
    }

    public static double CalcOffer(this DateTime time, double discount, double offerDiscount, double price,
        int hour,
        int min, bool isOffer)
    {
        var offerTime = time.AddHours(hour).AddMinutes(min);

        return (isOffer, offerTime >= DateTime.Now) switch
        {
            (true, true) => price - (offerDiscount + discount),
            (false, true) => price - discount,
            (true, false) => price - discount,
            (false, false) => price - discount
        };
    }

    public static double CalcOffer(this double price, double discount, double offerDiscount)
    {
        return price - (discount + offerDiscount);
    }

    public static bool CalcOffer(this DateTime time, int hour, int min, bool isOffer)
    {
        var offerTime = time.AddHours(hour).AddMinutes(min);
        if (!isOffer)
        {
            return false;
        }
        else
        {
            if (offerTime >= DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}