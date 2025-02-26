﻿using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.ProductName.Plugins.Core.Abstractions
{
    public interface ISkill
    {
        string Name { get; }
        string Description { get; }
        Task InitializeAsync(Kernel kernel);
    }
}
