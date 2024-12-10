using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Helper;

public static class HtmlView
{
    public static IHtmlContent ToRaw(this string value)
    {
        return new HtmlString(value ?? string.Empty);
    }
}