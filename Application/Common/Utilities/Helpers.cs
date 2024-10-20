﻿using System.Globalization;
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
    public static string CodeGenerator(int id , string month)
    {
        var code = string.Empty;
        for (var i = 0; i < 5; i++)
        {
            if (i == 0)
            {
                var dis = id.ToString() + "UM" + month;
                code +=dis+ new Random().Next(0, 9).ToString();
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
    public static DateTime ToGregorianDate (this DateTimeUtil time)
    {
            PersianCalendar persianCalendar = new PersianCalendar();
            DateTime gregorianDate = persianCalendar.ToDateTime(time.Years, time.Month, time.Days, time.Hours, time.Minute, time.Seconds, 0);
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
                word += i ;

            }

            index++;
        }

        return word;
    }
}