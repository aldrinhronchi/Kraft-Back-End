using KaibaSystem_Back_End.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KaibaSystem_Back_End.Connections.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> option) : base(option)
        {
        }

        public DatabaseContext() : base()
        {
        }

        #region Dbset

        public DbSet<Erro> ErrosLog { get; set; }
        public DbSet<Ocorrencia> OcorrenciaLog { get; set; }

        #endregion Dbset

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
                modelBuilder.ApplyConfiguration(new Configuration());

                modelBuilder.ApplyGlobalConfigurations();

             */

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                                                                         .SetBasePath(Environment.CurrentDirectory)
                                                                         .AddJsonFile("appsettings.json")
                                                                         .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("SotequiDB"));
        }
    }
}