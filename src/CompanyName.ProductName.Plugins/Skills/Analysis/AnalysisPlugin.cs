using CompanyName.ProductName.Plugins.Core.Abstractions;
using CompanyName.ProductName.Plugins.Core.Models;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.ProductName.Plugins.Skills.Analysis
{
    // Skills/Analysis/AnalysisPlugin.cs
    public class AnalysisPlugin : ISkill
    {
        public string Name => "Analysis";
        public string Description => "Skills for text and data analysis";

        [KernelFunction, Description("Analyze sentiment of text")]

        public Task InitializeAsync(Kernel kernel)
        {
            throw new NotImplementedException();
        }
    }
}
