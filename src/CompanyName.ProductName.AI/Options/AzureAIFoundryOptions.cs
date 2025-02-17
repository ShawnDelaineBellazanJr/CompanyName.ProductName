using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.ProductName.AI.Options
{
    public class AzureAIFoundryOptions
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ChatDeploymentName { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;


        public void Validate()
        {
            if (string.IsNullOrEmpty(ApiKey))
                throw new InvalidOperationException("AzureOpenAI:ApiKey is not configured");
            if (string.IsNullOrEmpty(ChatDeploymentName))
                throw new InvalidOperationException("AzureOpenAI:ChatDeploymentName is not configured");
            if (string.IsNullOrEmpty(Endpoint))
                throw new InvalidOperationException("AzureOpenAI:Endpoint is not configured");
        }

    }
}
