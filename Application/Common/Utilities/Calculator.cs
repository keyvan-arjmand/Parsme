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
}