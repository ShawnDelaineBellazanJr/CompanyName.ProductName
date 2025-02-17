using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// SQL Server configuration
var password = builder.AddParameter("sql-password", secret: true);
var sql = builder.AddSqlServer("sql", password)
    .WithBindMount("C:\\data", "/var/opt/mssql/data")
    .AddDatabase("ApplicationDb");

var grpcService = builder.AddProject<Projects.CompanyName_ProductName_GrpcService>("companyname-productname-grpcservice")
    .WithEnvironment("AzureOpenAI__ApiKey", builder.Configuration["Parameters:ApiKey"])
    .WithEnvironment("AzureOpenAI__Endpoint", builder.Configuration["Parameters:Endpoint"])
    .WithEnvironment("AzureOpenAI__ChatDeploymentName", builder.Configuration["Parameters:DeploymentName"])
    .WithEnvironment("","")
    .WithReference(sql)
    .WaitFor(sql);


builder.Build().Run();