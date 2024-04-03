using System;
using System.Collections.Generic;

namespace Paragoniarz.Domain.Orders
{
    public class ValidationException : Exception
    {
        private readonly IEnumerable<string> missingFields;

        public ValidationException(IEnumerable<string> missingFields)
        {
            this.missingFields = missingFields;
        }

        public override string ToString()
        {
            return "Missing: [" + string.Join(", ", missingFields) + "]";
        }
    }
}
