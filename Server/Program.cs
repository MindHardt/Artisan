using Serilog;

Host.CreateDefaultBuilder(args)
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
