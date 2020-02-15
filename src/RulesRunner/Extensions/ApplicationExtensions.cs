namespace RulesRunner.Extensions {
    // TODO: Get this to work with IHostBuilder for non asp.net apps
    //public static IHostBuilder UseNRules(this IHostBuilder app) {
    //    var repo = app.ApplicationServices.GetService<RuleRepository>();
    //    var factory = app.ApplicationServices.GetService<ISessionFactory>();

    //    if (factory == null || repo == null)
    //        throw new InvalidOperationException("Dependencies not registered. Call AddNRules in Startup.cs");

    //    repo.Activator = new AspNetCoreRuleActivator(app.ApplicationServices);
    //    factory.DependencyResolver = new AspNetCoreDependencyResolver(app.ApplicationServices);

    //    return app;
    //}
}