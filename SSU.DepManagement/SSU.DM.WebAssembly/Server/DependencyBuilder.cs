using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.Core.Interface;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.DataAccessLayer.DataAccessObjects.Impl;
using Microsoft.EntityFrameworkCore;
using SSU.DM.LogicLayer.Interfaces.Request;
using SSU.DM.LogicLayer.Request;
using SSU.DM.Tools.Interface;
using SSU.DM.ExcelParser;
using SSU.DM.ExcelWriter;
using SSU.DM.LogicLayer.Faculty;
using SSU.DM.LogicLayer.Interfaces.Faculty;
using SSU.DM.LogicLayer.Interfaces.Reports;
using SSU.DM.LogicLayer.Reports;

namespace SSU.DM.WebAssembly.Server;

public static class DependencyBuilder
{
    public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services,
        string connectionString)
        => services
            .AddDbContext<ApplicationContext>(options => options
                .UseLazyLoadingProxies()
                .UseNpgsql(connectionString))
            .RegisterParserDependencies()
            .RegisterWriterDependencies()
            .RegisterLogicLayerDependencies()
            .RegisterToolsDependencies()
            .RegisterDaoDependencies()
            .RegisterParsers()
            .RegisterWriters();

    /// <summary>
    /// Logic layer
    /// </summary>
    private static IServiceCollection RegisterLogicLayerDependencies(this IServiceCollection services)
        => services
            .AddScoped<IRequestAccessor, RequestAccessor>()
            .AddScoped<IRequestEditor, RequestEditor>()
            .AddScoped<IFacultyLogic, FacultyLogic>()
            .AddScoped<ICalculationOfHoursBuilder, CalculationOfHoursBuilder>();
    
    /// <summary>
    /// Parsers
    /// </summary>
    private static IServiceCollection RegisterParserDependencies(this IServiceCollection services)
        => services;
    
    /// <summary>
    /// Writers
    /// </summary>
    private static IServiceCollection RegisterWriterDependencies(this IServiceCollection services)
        => services;

    /// <summary>
    /// Tools
    /// </summary>
    private static IServiceCollection RegisterToolsDependencies(this IServiceCollection services)
        => services
            .AddScoped<IExcelParser, Parser>();
    
    /// <summary>
    /// DAO
    /// </summary>
    private static IServiceCollection RegisterDaoDependencies(this IServiceCollection services)
        => services
            .AddScoped<ITransactionManager, TransactionManager>()
            .AddScoped<IFilesStorageDao, FilesStorageDao>()
            .AddScoped<IApplicationFormDao, ApplicationFormDao>()
            .AddScoped<IRequestDao, RequestDao>()
            .AddScoped<IFacultyDao, FacultyDao>();
}
