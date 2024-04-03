using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace Paragoniarz.Domain.Orders;

[XmlRoot("orders")]
public class Orders
{
    [XmlAttribute("date")] public string? RawDate { get; set; }

    [XmlElement("item")] public List<Item> Items { get; set; }

    [XmlIgnore]
    public DateTime? Date
    {
        get
        {
            if (string.IsNullOrEmpty(RawDate))
                return null;

            return DateTime.Parse(RawDate, CultureInfo.InvariantCulture);
        }
    }
}
