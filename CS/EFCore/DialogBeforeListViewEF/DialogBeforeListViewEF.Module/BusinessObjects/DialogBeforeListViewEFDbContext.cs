using DevExpress.ExpressApp.EFCore.Updating;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp.Design;
using DevExpress.ExpressApp.EFCore.DesignTime;
using E1554.Module;
using DevExpress.Persistent.Base;

namespace DialogBeforeListViewEF.Module.BusinessObjects;

// This code allows our Model Editor to get relevant EF Core metadata at design time.
// For details, please refer to https://supportcenter.devexpress.com/ticket/details/t933891.
public class DialogBeforeListViewEFContextInitializer : DbContextTypesInfoInitializerBase {
	protected override DbContext CreateDbContext() {
		var optionsBuilder = new DbContextOptionsBuilder<DialogBeforeListViewEFEFCoreDbContext>()
            .UseSqlServer(";")
            .UseChangeTrackingProxies()
            .UseObjectSpaceLinkProxies();
        return new DialogBeforeListViewEFEFCoreDbContext(optionsBuilder.Options);
	}
}
//This factory creates DbContext for design-time services. For example, it is required for database migration.
public class DialogBeforeListViewEFDesignTimeDbContextFactory : IDesignTimeDbContextFactory<DialogBeforeListViewEFEFCoreDbContext> {
	public DialogBeforeListViewEFEFCoreDbContext CreateDbContext(string[] args) {
		throw new InvalidOperationException("Make sure that the database connection string and connection provider are correct. After that, uncomment the code below and remove this exception.");
		//var optionsBuilder = new DbContextOptionsBuilder<DialogBeforeListViewEFEFCoreDbContext>();
		//optionsBuilder.UseSqlServer("Integrated Security=SSPI;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=DialogBeforeListViewEF");
        //optionsBuilder.UseChangeTrackingProxies();
        //optionsBuilder.UseObjectSpaceLinkProxies();
		//return new DialogBeforeListViewEFEFCoreDbContext(optionsBuilder.Options);
	}
}
[TypesInfoInitializer(typeof(DialogBeforeListViewEFContextInitializer))]
public class DialogBeforeListViewEFEFCoreDbContext : DbContext {
	public DialogBeforeListViewEFEFCoreDbContext(DbContextOptions<DialogBeforeListViewEFEFCoreDbContext> options) : base(options) {
	}
    public DbSet<Detail> Details { get; set; }
    public DbSet<Master> Masters { get; set; }
    public DbSet<ViewFilterObject> ViewFilterObjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
        modelBuilder.Entity<ViewFilterObject>().Property(e => e.ObjectType).HasConversion(v => v.FullName,v=> ReflectionHelper.FindType((string)v));
    }
}
