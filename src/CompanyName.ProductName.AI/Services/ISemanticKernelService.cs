using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.ProductName.AI.Services
{
    public interface ISemanticKernelService
    {
        Task<string> GenerateTextAsync(string prompt, CancellationToken cancellationToken = default);
        Task<string> SummarizeTextAsync(string text, CancellationToken cancellationToken = default);
        Task<bool> AnalyzeSentimentAsync(string text, CancellationToken cancellationToken = default);
    }
}
