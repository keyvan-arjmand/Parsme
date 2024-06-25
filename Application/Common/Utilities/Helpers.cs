using System.Text.RegularExpressions;

namespace Application.Common.Utilities;

public static partial class Helpers
{
    [GeneratedRegex("^\\d+$")]
    private static partial Regex MyRegex();

    public static bool IsPhone(this string value)
    {
        return MyRegex().IsMatch(value);
    }

    public static string GetConfirmCode()
    {
        var code = string.Empty;
        for (var i = 0; i < 6; i++)
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
}