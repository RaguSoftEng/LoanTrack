using LoanTrack.Application.Centers.Queries;
using LoanTrack.Application.Common.Data;
using LoanTrack.Application.Customers.Queries;
using LoanTrack.Application.Employees.Queries;
using LoanTrack.Application.Finance;
using LoanTrack.Application.Groups.Queries;
using LoanTrack.Application.ListValues.Queries;
using LoanTrack.Application.Loans;
using LoanTrack.Application.LoanSchemes.Queries;
using LoanTrack.Domain.Centers;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Customers;
using LoanTrack.Domain.Employees;
using LoanTrack.Domain.Finance;
using LoanTrack.Domain.Groups;
using LoanTrack.Domain.ListValues;
using LoanTrack.Domain.Loans;
using LoanTrack.Domain.LoanSchemes;
using LoanTrack.Persistence.Centers;
using LoanTrack.Persistence.Common;
using LoanTrack.Persistence.Common.Database;
using LoanTrack.Persistence.Customers;
using LoanTrack.Persistence.Employees;
using LoanTrack.Persistence.Finance;
using LoanTrack.Persistence.Groups;
using LoanTrack.Persistence.ListValues;
using LoanTrack.Persistence.Loans;
using LoanTrack.Persistence.LoanSchemes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace LoanTrack.Persistence;

public static class PersistenceConfiguration
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        string dbConnection
    )
    {
        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(dbConnection).Build();
        services.TryAddSingleton(npgsqlDataSource);

        services.AddDbContext<ApplicationDbContext>((sp, options) => 
            options.UseNpgsql(
                    dbConnection,
                sqlOptions => sqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Constants.SchemaName)
            )
            .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
            .UseSnakeCaseNamingConvention()
        );
        
      //  services.TryAddSingleton<PublishDomainEventsInterceptor>();
        
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>()
            .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<ICustomerQueryRepository, CustomerQueryRepository>()
            .AddScoped<IListValueRepository, ListValueRepository>()
            .AddScoped<IListValueQueryRepository, ListValueQueryRepository>()
            .AddScoped<ICustomerGroupRepository, CustomerGroupRepository>()
            .AddScoped<IGroupQueryRepository, GroupQueryRepository>()
            .AddScoped<IEmployeeRepository, EmployeeRepository>()
            .AddScoped<IEmployeeQueryRepository, EmployeeQueryRepository>()
            .AddScoped<ILoanSchemeRepository, LoanSchemeRepository>()
            .AddScoped<ILoanSchemeQueryRepository, LoanSchemeQueryRepository>()
            .AddScoped<ICenterRepository, CenterRepository>()
            .AddScoped<ICenterQueryRepository, CenterQueryRepository>()
            .AddScoped<ILoanRepository, LoanRepository>()
            .AddScoped<ILoanQueryRepository, LoanQueryRepository>()
            .AddScoped<ILoanInstallmentRepository, LoanInstallmentRepository>()
            .AddScoped<IInstallmentQueryRepository, InstallmentQueryRepository>()
            .AddScoped<IFinanceJournalRepository, FinanceJournalRepository>()
            .AddScoped<IFinanceJournalQueryRepository, FinanceJournalQueryRepository>();
        
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        
        return services;
    }
}
