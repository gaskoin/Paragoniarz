using System;
using System.Threading.Tasks;
using Paragoniarz.Domain.Orders;

namespace Paragoniarz.Domain;
public interface IDocumentService
{
    Task<OrderSummary?> GenerateDocuments(OrderSummary summary, IProgress<int> progress);
}
