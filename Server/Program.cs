using Serilog;

new HostBuilder()
    .ConfigureAppConfiguration((ctx, cfg) =>
    {
        var envName = ctx.HostingEnvironment.EnvironmentName;
        cfg.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
        cfg.AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: false);
        cfg.AddUserSecrets(typeof(Program).Assembly);
        cfg.AddEnvironmentVariables();
    })
    .UseSerilog((ctx, logger) =>
    {
        logger.ReadFrom.Configuration(ctx.Configuration);
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Artisan.Server.Startup>();
    })
    .Build()
    .Run();
