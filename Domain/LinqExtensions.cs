using System;
using System.Globalization;
using System.Xml.Linq;

namespace Paragoniarz.Domain;

public static class LinqExtensions
{
    public static DateTime GetDateTimeAttribute(this XElement element, string name)
    {
        return DateTime.Parse(element!.Attribute(name)!.Value, CultureInfo.InvariantCulture);
    }

    public static int? GetInt(this XElement element, string name)
    {
        string? value = element.GetString(name);
        if (string.IsNullOrEmpty(value))
            return null;

        return int.Parse(value, CultureInfo.InvariantCulture);
    }

    public static decimal? GetDecimal(this XElement element, string name)
    {
        string? value = element.GetString(name);
        if (string.IsNullOrEmpty(value))
            return null;

        return decimal.Parse(value, CultureInfo.InvariantCulture);
    }

    public static DateTime? GetDateTime(this XElement element, string name)
    {
        string? value = element.GetString(name);
        if (string.IsNullOrEmpty(value))
            return null;

        return DateTime.Parse(value, CultureInfo.InvariantCulture);
    }

    public static string? GetString(this XElement element, string name)
    {
        return element.Element(name)?.Value;
    }
}
