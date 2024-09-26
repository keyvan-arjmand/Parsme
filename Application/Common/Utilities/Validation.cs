using System.Text.RegularExpressions;

namespace Application.Common.Utilities;

public static class Validation
{
    public static bool ValidationDate(DateTime timeStart, DateTime timeEnd)
    {
        switch (DateTime.Compare(timeStart, timeEnd))
        {
            case < 0:
                return true;
            case 0:
                return false;
            case > 0:
                return false;
        }
    }
   public static bool IsValidIranianPhoneNumber(this string phoneNumber)
    {
        // الگوی منظم برای اعتبارسنجی شماره تلفن‌های ایران
        string pattern = @"^09\d{9}$";
        
        return Regex.IsMatch(phoneNumber, pattern);
    }
   
    public static bool IsValidIranianNationalCode(this string nationalCode)
    {
        // بررسی اینکه کد ملی یک عدد 10 رقمی است
        if (!Regex.IsMatch(nationalCode, @"^\d{10}$"))
            return false;

        // بررسی اینکه همه ارقام کد ملی یکسان نباشند
        if (new string(nationalCode[0], 10) == nationalCode)
            return false;

        // محاسبه و بررسی رقم کنترلی
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += (int)char.GetNumericValue(nationalCode[i]) * (10 - i);
        }

        int remainder = sum % 11;
        int controlDigit = (int)char.GetNumericValue(nationalCode[9]);

        return (remainder < 2 && controlDigit == remainder) || (remainder >= 2 && controlDigit == 11 - remainder);
    }
}