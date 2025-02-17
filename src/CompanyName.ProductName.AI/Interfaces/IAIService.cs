using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.ProductName.AI.Interfaces
{
    public   interface IAIService
    {
        Task<string> ExecutePromptAsync(string promptName, Dictionary<string, string> variables);
        Task<bool> ValidateResponseAsync(string response, string schema);
    }
}
