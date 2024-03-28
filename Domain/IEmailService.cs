using System;
using System.Threading.Tasks;
using Paragoniarz.Domain.Orders;

namespace Paragoniarz.Domain;
public interface IEmailService
{
    Task SendEmails(OrderSummary summary, IProgress<int> progress);
}
