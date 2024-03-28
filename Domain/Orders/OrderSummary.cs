using System;
using System.Collections.Generic;

namespace Paragoniarz.Domain.Orders;

public record OrderSummary(DateTime Date, IEnumerable<Order> Orders);
