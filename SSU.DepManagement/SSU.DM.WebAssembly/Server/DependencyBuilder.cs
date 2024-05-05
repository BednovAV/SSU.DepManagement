using SSU.DM.DataAccessLayer.Core;
using SSU.DM.DataAccessLayer.Core.Interface;
using SSU.DM.DataAccessLayer.DataAccessObjects;
using SSU.DM.DataAccessLayer.DataAccessObjects.Impl;
using Microsoft.EntityFrameworkCore;
using SSU.DM.Authorization;
using SSU.DM.LogicLayer.Interfaces.Request;
using SSU.DM.Tools.Interface;
using SSU.DM.ExcelParser;
using SSU.DM.ExcelWriter;
using SSU.DM.LogicLayer.Competencies;
using SSU.DM.LogicLayer.Discipline;
using SSU.DM.LogicLayer.Faculties;
using SSU.DM.LogicLayer.Interfaces.Competencies;
using SSU.DM.LogicLayer.Interfaces.Discipline;
using SSU.DM.LogicLayer.Interfaces.Faculty;
using SSU.DM.LogicLayer.Interfaces.JobTitle;
using SSU.DM.LogicLayer.Interfaces.Reports;
using SSU.DM.LogicLayer.Interfaces.Semesters;
using SSU.DM.LogicLayer.Interfaces.Teachers;
using SSU.DM.LogicLayer.JobTitle;
using SSU.DM.LogicLayer.Reports;
using SSU.DM.LogicLayer.Requests;
using SSU.DM.LogicLayer.Semesters;
using SSU.DM.LogicLayer.Teachers;

namespace SSU.DM.WebAssembly.Server;

public static class DependencyBuilder
{
    public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services,
        string connectionString)
        => services
            .AddDbContext<ApplicationContext>(options => options
                .UseLazyLoadingProxies()
                .UseNpgsql(connectionString))
            .AddAuthorizationServices(connectionString)
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
            .AddScoped<ITeacherLogic, TeacherLogic>()
            .AddScoped<ISemesterLogic, SemesterLogic>()
            .AddScoped<IRequestAccessor, RequestAccessor>()
            .AddScoped<IParsedRequestProcessor, ParsedRequestProcessor>()
            .AddScoped<IRequestEditor, RequestEditor>()
            .AddScoped<IFacultyLogic, FacultyLogic>()
            .AddScoped<ICompetenceLogic, CompetenceLogic>()
            .AddScoped<IDisciplineLogic, DisciplineLogic>()
            .AddScoped<IJobTitleLogic, JobTitleLogic>()
            .AddScoped<ICalculationOfHoursBuilder, CalculationOfHoursBuilder>()
            .AddScoped<IStudyAssignmentReportBuilder, StudyAssignmentReportBuilder>()
            .AddScoped<IDistributionReportBuilder, DistributionReportBuilder>();

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
            .AddScoped<ITeachersDao, TeacherDao>()
            .AddScoped<IFacultyDao, FacultyDao>()
            .AddScoped<IDisciplineDao, DisciplineDao>()
            .AddScoped<ICompetenceDao, CompetenceDao>()
            .AddScoped<IJobTitleDao, JobTitleDao>()
            .AddScoped<ISemesterDao, SemesterDao>();
}