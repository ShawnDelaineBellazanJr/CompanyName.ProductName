using CompanyName.ProductName.AI.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Text.Json;

namespace CompanyName.ProductName.AI.Services
{
    public class SemanticKernelService : ISemanticKernelService
    {


        private readonly Kernel _kernel;
        private readonly AzureAIFoundryOptions _azureAIFoundry;
        private readonly ILogger<SemanticKernelService> _logger;
        private readonly string _promptsBasePath;
        public SemanticKernelService(
            IOptions<AzureAIFoundryOptions> azureAIFoundryOptions,
            IOptions<PromptTemplateOptions> promptTemplateOptions,
            ILogger<SemanticKernelService> logger
            )
        {
            _azureAIFoundry = azureAIFoundryOptions.Value;
            _azureAIFoundry.Validate();

            _logger = logger;
            _promptsBasePath = promptTemplateOptions.Value.BasePath;
            _kernel = CreateKernel();


        }

        public async Task<string> GenerateTextAsync(string prompt, CancellationToken cancellationToken = default)
        {
            try
            {
                // Use the new execution settings pattern
                var result = await _kernel.InvokePromptAsync(
                    promptTemplate: prompt,
                    arguments: new(new OpenAIPromptExecutionSettings()
                    {
                        MaxTokens = 500,
                        Temperature = 0.5,
                        TopP = 0.0,
                        PresencePenalty = 0.0,
                        FrequencyPenalty = 0.0
                    }),
                    cancellationToken: cancellationToken);

                return result.GetValue<string>() ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating text with prompt: {Prompt}", prompt);
                throw;
            }
        }

        public async Task<string> SummarizeTextAsync(string text, CancellationToken cancellationToken = default)
        {
            try
            {
                // Load configuration from JSON
                string configPayload = """
                {
                    "schema": 1,
                    "name": "Summarize",
                    "description": "Summarize the provided text",
                    "type": "completion",
                    "completion": {
                        "max_tokens": 500,
                        "temperature": 0.5,
                        "top_p": 0.0,
                        "presence_penalty": 0.0,
                        "frequency_penalty": 0.0
                    }
                }
                """;

                var promptConfig = JsonSerializer.Deserialize<PromptTemplateConfig>(configPayload)!;
                promptConfig.Template = @"
                    Summarize the following text in a concise way while retaining the key points:
                    {{$input}}
                    
                    Summary:";

                var function = _kernel.CreateFunctionFromPrompt(promptConfig);
                var result = await _kernel.InvokeAsync(
                    function,
                    new KernelArguments { ["input"] = text },
                    cancellationToken);

                return result.GetValue<string>() ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error summarizing text");
                throw;
            }
        }

        public async Task<bool> AnalyzeSentimentAsync(string text, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _kernel.InvokePromptAsync(
                    promptTemplate: @"Analyze the sentiment of the following text and respond with 'true' for positive sentiment or 'false' for negative sentiment:
                                    {{$input}}

                                    Respond with only true or false.",
                                    new(new OpenAIPromptExecutionSettings()
                                    {
                                        MaxTokens = 1,
                                        Temperature = 0.0,
                                        TopP = 0.0,
                                        FrequencyPenalty = 0.0
                                    }),
                                     cancellationToken: cancellationToken);

                return bool.TryParse(result.GetValue<string>(), out bool sentiment) && sentiment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing sentiment for text: {Text}", text);
                throw;
            }
        }

        private Kernel CreateKernel()
        {
            try
            {
                return Kernel.CreateBuilder()
                    .AddAzureOpenAIChatCompletion(
                        deploymentName: _azureAIFoundry.ChatDeploymentName, 
                        endpoint: _azureAIFoundry.Endpoint,
                        apiKey: _azureAIFoundry.ApiKey)
                    .Build();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create Semantic Kernel instance");
                throw;
            }
        }
     

    }

}