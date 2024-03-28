using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Paragoniarz.Domain.Orders;
public interface IOrderSummaryService
{
    Task<OrderSummary> ReadFile(Uri path);
}