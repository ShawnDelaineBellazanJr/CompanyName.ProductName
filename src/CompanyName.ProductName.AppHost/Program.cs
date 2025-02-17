using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("sql-password", "Dev@Password123", secret: true);
var sql1 = builder.AddSqlServer("sql", password)
    .AddDatabase("CatalogDb");


builder.Build().Run();