using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Configurations;

namespace Persistence.Context;

//IMPORTANT : Domain vs Entity Class
//We can turn domain classes into entities by specifying them as DbSet<TEntity> type properties. This will allow Entity Framework to track their changes and perform CRUD operations accordingly.
//The DbSet<TEntity> type allows EF Core to query and save instances of the specified entity to the database. LINQ queries against a DbSet<TEntity> will be translated into queries against the database
//Reference link: https://www.entityframeworktutorial.net/efcore/entity-framework-core-dbcontext.aspx
//We don't use this app db context that way that's why we don't need to set the entities like in the code below
//public DbSet<CustomerModel> Customers { get; set; }
//Please see how we make transaction to the database in the repository classes

//Our class now inherits from the IdentityDbContext class and not DbContext because we want to integrate our context with Identity
public class AppDBContext : IdentityDbContext<UserModel>
{
    private IConfiguration _configuration;

    public AppDBContext(DbContextOptions<AppDBContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    //We can set the database connection here or in the service registering by utilizing lambda expression
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("DBConnection");
        optionsBuilder.UseSqlServer(connectionString, option =>
            {
                //option.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                option.CommandTimeout(60);
            }

            );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //We call the OnModelCreating method from the base class because we use Asp.NET Core Identity and it required to let Microsoft Identity do its stuff.
        //Now, we have to move on to the configuration part. Please see the configuration class for more details in Api > Service Installers > IdentityInstaller.cs.
        base.OnModelCreating(modelBuilder);

        //IMPORTANT: Database Configuration
        //The set of rules for the table/entity can be defined in either the domain class or the configuration using Fluent API, or a combination of both can be utilized.
        //We can utilize both for a better result.

        //It will take all configurations from IEntityTypeConfiguration interface and run it.
        //Please refer to this link : https://learn.microsoft.com/en-us/ef/core/modeling/
        //For more details of EF Core code-first data model, please refer to this link : https://www.entityframeworktutorial.net/code-first
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerConfiguration).Assembly);

        //modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    }
}