using System.Globalization;
using System.Text.RegularExpressions;
using Application.Dtos;
using Domain.Enums;

namespace Application.Common.Utilities;

public static partial class Helpers
{
    [GeneratedRegex("^\\d+$")]
    private static partial Regex MyRegex();

    public static bool IsPhone(this string value)
    {
        return MyRegex().IsMatch(value);
    }

    public static string CodeGenerator(int id, string month)
    {
        var code = string.Empty;
        for (var i = 0; i < 5; i++)
        {
            if (i == 0)
            {
                var dis = id.ToString() + "UM" + month;
                code += dis + new Random().Next(0, 9).ToString();
            }
            else
            {
                code += new Random().Next(0, 9).ToString();
            }
        }

        return code;
    }

    public static string GetConfirmCode()
    {
        var code = string.Empty;
        for (var i = 0; i < 5; i++)
        {
            code += new Random().Next(0, 9).ToString();
        }

        return code;
    }

    public static int ToInt(this string? val)
    {
        return string.IsNullOrEmpty(val) ? 0 : Convert.ToInt32(val);
    }

    public static double ToDouble(this string? val)
    {
        return string.IsNullOrEmpty(val) ? 0 : Convert.ToDouble(val);
    }

    public static int GetDataTypeId(this DataType type)
    {
        var id = 0;
        switch (type)
        {
            case DataType.AreaBox:
                id = 2;
                break;
            case DataType.CheckBox:
                id = 3;
                break;
            case DataType.SelectBox:
                id = 1;
                break;
            case DataType.TextBox:
                id = 0;
                break;
        }

        return id;
    }

    public static int GetAccTypeId(this AccountType type)
    {
        var id = 0;
        switch (type)
        {
            case AccountType.Legal:
                id = 1;
                break;
            case AccountType.Genuine:
                id = 0;
                break;
        }

        return id;
    }

    public static string ToPersianTime(this DateTime calendar)
    {
        try
        {
            PersianCalendar pc = new PersianCalendar();
            return string.Format("{0}/{1}/{2}", pc.GetYear(calendar), pc.GetMonth(calendar),
                pc.GetDayOfMonth(calendar));
        }
        catch (Exception e)
        {
            return string.Empty;
        }
    }

    public static DateTime ToGregorianDate(this DateTimeUtil time)
    {
        PersianCalendar persianCalendar = new PersianCalendar();
        DateTime gregorianDate = persianCalendar.ToDateTime(time.Years, time.Month, time.Days, time.Hours, time.Minute,
            time.Seconds, 0);
        return gregorianDate;
    }

    public static string ToStringList(this List<string> o)
    {
        string word = string.Empty;
        int index = 0;
        foreach (var i in o)
        {
            if (index != o.Count)
            {
                word += i + ",";
            }
            else
            {
                word += i;
            }

            index++;
        }

        return word;
    }

    public static string MellatResult(string ID)
    {
        string result = "";
        switch (ID)
        {
            case "-100":
                result = "پرداخت لغو شده";
                break;
            case "0":
                result = "تراكنش با موفقيت انجام شد";
                break;

            case "11":
                result = "شماره كارت نامعتبر است ";
                break;
            case "12":
                result = "موجودي كافي نيست ";
                break;
            case "13":
                result = "رمز نادرست است ";
                break;
            case "14":
                result = "تعداد دفعات وارد كردن رمز بيش از حد مجاز است ";
                break;
            case "15":
                result = "كارت نامعتبر است ";
                break;
            case "16":
                result = "دفعات برداشت وجه بيش از حد مجاز است ";
                break;
            case "17":
                result = "كاربر از انجام تراكنش منصرف شده است ";
                break;
            case "18":
                result = "تاريخ انقضاي كارت گذشته است ";
                break;
            case "19":
                result = "مبلغ برداشت وجه بيش از حد مجاز است ";
                break;
            case "111":
                result = "صادر كننده كارت نامعتبر است ";
                break;
            case "112":
                result = "خطاي سوييچ صادر كننده كارت ";
                break;
            case "113":
                result = "پاسخي از صادر كننده كارت دريافت نشد ";
                break;
            case "114":
                result = "دارنده كارت مجاز به انجام اين تراكنش نيست";
                break;
            case "21":
                result = "پذيرنده نامعتبر است ";
                break;
            case "23":
                result = "خطاي امنيتي رخ داده است ";
                break;
            case "24":
                result = "اطلاعات كاربري پذيرنده نامعتبر است ";
                break;
            case "25":
                result = "مبلغ نامعتبر است ";
                break;
            case "31":
                result = "پاسخ نامعتبر است ";
                break;

            case "32":
                result = "فرمت اطلاعات وارد شده صحيح نمي باشد ";
                break;
            case "33":
                result = "حساب نامعتبر است ";
                break;
            case "34":
                result = "خطاي سيستمي ";
                break;
            case "35":
                result = "تاريخ نامعتبر است ";
                break;
            case "41":
                result = "شماره درخواست تكراري است ، دوباره تلاش کنید";
                break;
            case "42":
                result = "يافت نشد  Sale تراكنش";
                break;
            case "43":
                result = "داده شده است  Verify قبلا درخواست";
                break;
            case "44":
                result = "يافت نشد  Verfiy درخواست";
                break;
            case "45":
                result = "شده است  Settle تراكنش";
                break;
            case "46":
                result = "نشده است  Settle تراكنش";
                break;
            case "47":
                result = "يافت نشد  Settle تراكنش";
                break;
            case "48":
                result = "شده است  Reverse تراكنش";
                break;
            case "49":
                result = "يافت نشد  Refund تراكنش";
                break;
            case "412":
                result = "شناسه قبض نادرست است ";
                break;
            case "413":
                result = "شناسه پرداخت نادرست است ";
                break;
            case "414":
                result = "سازمان صادر كننده قبض نامعتبر است ";
                break;
            case "415":
                result = "زمان جلسه كاري به پايان رسيده است ";
                break;
            case "416":
                result = "خطا در ثبت اطلاعات ";
                break;
            case "417":
                result = "شناسه پرداخت كننده نامعتبر است ";
                break;

            case "418":
                result = "اشكال در تعريف اطلاعات مشتري ";
                break;
            case "419":
                result = "تعداد دفعات ورود اطلاعات از حد مجاز گذشته است ";
                break;
            case "421":
                result = "نامعتبر است  IP";
                break;
            case "51":
                result = "تراكنش تكراري است ";
                break;
            case "54":
                result = "تراكنش مرجع موجود نيست ";
                break;
            case "55":
                result = "تراكنش نامعتبر است ";
                break;
            case "61":
                result = "خطا در واريز ";
                break;

            default:
                result = string.Empty;
                break;
        }

        return result;
    }

    public static string GetPredefinedColor(this int index)
    {
        string[] colors = { "#dc3545", "#28a745", "#ffc107", "#17a2b8", "#007bff", "#6c757d" };
        return colors[index % colors.Length];
    }

    public static string GetPredefinedHighlight(this int index)
    {
        string[] highlights = { "#dc3545", "#28a745", "#ffc107", "#17a2b8", "#007bff", "#6c757d" };
        return highlights[index % highlights.Length];
    }
}