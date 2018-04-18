using EF事例.Models;
using System.Data.Entity;
namespace EF事例.DAL
{
    /*
     * 
     * DbContext使用注意事项
     * ·随着越来越多的对象和他们的引用进入内存，DbContext的内存消耗可能会迅速增长，这将会引起性能问题。
     * ·当不再使用context对象的时候，一定要释放它。
     * ·如果一个异常使得context进入了一个不可恢复的状态，整个应用可能会终止。
     * ·长时间使用的context会增加并发冲突的可能。
     * 
     */
    public class EFDBContext : DbContext
    {

        public EFDBContext()
            : base("EFDemo")
        {
            //base("Data Source=.;database=EFDemoDB;uid=sa;pwd=Sql2014;")
            /*
             * 指定数据库名称
             * EFDemo 字符串是指在配置文件 App.config 或者 Web.config中添加数据库连接字符串
             * 
             *<connectionStrings>
             *		<add name="EFDemo" connectionString="Data Source=.;database=EFDemoDB;uid=sa;pwd=Sql2014;" providerName="System.Data.SqlClient" />
             *</connectionStrings>
             * 
             */

            //数据库初始化策略
            //1.CreateDatabaseIfNotExists：这是默认的策略。如果数据库不存在，那么就创建数据库。但是如果数据库存在了，而且实体发生了变化，就会出现异常。
            //Database.SetInitializer<EFDBContext>(new CreateDatabaseIfNotExists<EFDBContext>());

            //2.DropCreateDatabaseIfModelChanges：此策略表明，如果模型变化了，数据库就会被重新创建，原来的数据库被删除掉了。


            //3.DropCreateDatabaseAlways：此策略表示，每次运行程序都会重新创建数据库，这在开发和调试的时候非常有用。
            //Database.SetInitializer<EFDBContext>(new EFDBContextDropCreateInitializer());    

            //4.自定制数据库策略：可以自己实现IDatabaseInitializer来创建自己的策略。或者从已有的实现了IDatabaseInitializer接口的类派生。

            //不使用数据库初始化策略
            //不对数据库的表和程序的实体类进行校验，不会创建数据库和表结构
            // Database.SetInitializer<EFDBContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //移除约定

            //指定单数形式的表名   注：很多时候都需要用到
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();

            //配置主键
            //要显式将某个属性设置为主键，可使用 HasKey 方法。在以下示例中，使用了 HasKey 方法对 Department 类型配置 DepartmentID 主键。
            //modelBuilder.Entity<Department>().HasKey(t => t.DepartmentID);

            //配置组合主键
            //以下示例配置要作为Department 类型的组合主键的DepartmentID 和 Name 属性。
            //modelBuilder.Entity<Department>().HasKey(t => new { t.DepartmentID, t.Name });

            //关闭数值主键的标识
            // 以下示例将DepartmentID 属性设置为System.ComponentModel.DataAnnotations.DatabaseGeneratedOption.None，以指示该值不由数据库生成。
            //标记为自动标识 System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity
            //标记为计算所得值 System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed
            //modelBuilder.Entity<Department>().Property(t => t.DepartmentID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            //指定属性的最大长度
            //在以下示例中，Name属性不应超过 50 个字符。如果其值超过 50 个字符，则出现 DbEntityValidationException 异常。如果 Code First 基于此模型创建数据库，它还会将 Name 列的最大长度设置为50 个字符。
            //modelBuilder.Entity<Department>().Property(t =>t.Name).HasMaxLength(50);

            //将属性配置为必需
            //在下面的示例中，Name属性是必需的。如果不指定 Name，则出现 DbEntityValidationException 异常。如果 Code First 基于此模型创建数据库，则用于存储此属性的列将不可为空。
            //modelBuilder.Entity<Department>().Property(t => t.Name).IsRequired();


            //指定不将CLR 属性映射到数据库中的列
            //以下示例显示如何指定CLR 类型的属性不映射到数据库中的列。
            //modelBuilder.Entity<Department>().Ignore(t => t.TestInt);


            //将CLR 属性映射到数据库中的特定列
            //以下示例将Name CLR 属性映射到DepartmentName 数据库列。
            //modelBuilder.Entity<Department>().Property(t => t.Name).HasColumnName("DepartmentName");

            //重命名模型中未定义的外键
            //如果您选择不对CLR 类型定义外键，但希望指定它在数据库中应使用的名称，请编码如下：
            //modelBuilder.Entity<Course>()
            //        .HasRequired(c => c.Department)
            //        .WithMany(t => t.Courses)
            //        .Map(m => m.MapKey("ChangedDepartmentID"));


            //配置字符串属性是否支持Unicode 内容
            //默认情况下，字符串为Unicode（SQLServer 中的nvarchar）。您可以使用IsUnicode 方法指定字符串应为varchar 类型。
            //modelBuilder.Entity<Department>()
            //        .Property(t => t.Name)
            //        .IsUnicode(false);


            //配置数据库列的数据类型
            //HasColumnType 方法支持映射到相同基本类型的不同表示。使用此方法并不支持在运行时执行任何数据转换。请注意，IsUnicode 是将列设置为 varchar 的首选方法，因为它与数据库无关。
            //modelBuilder.Entity<Department>()
            //        .Property(p => p.Name)
            //        .HasColumnType("varchar");

            //配置复杂类型的属性
            //对复杂类型配置标量属性有两种方法。
            //可以对ComplexTypeConfiguration 调用Property。
            //modelBuilder.ComplexType<Details>()
            //        .Property(t => t.Location)
            //        .HasMaxLength(20);

            //也可以使用点表示法访问复杂类型的属性。
            //modelBuilder.Entity<OnsiteCourse>()
            //        .Property(t => t.Details.Location)
            //        .HasMaxLength(20);

            /*
             * 
             *public partial class OnsiteCourse : Course
             *{
             *  publicOnsiteCourse()
             *  {
             *    Details = newDetails();
             *  }
             * 
             *  public Details Details { get;set; }
             *}
             * 
             *public class Details
             *{
             *  publicSystem.DateTime Time { get; set; }
             *  public string Location { get;set; }
             *  public string Days { get; set; }
             *}
             */


            //将属性配置为用作乐观并发令牌
            //要指定实体中的某个属性表示并发令牌，可使用 ConcurrencyCheck 特性或 IsConcurrencyToken 方法。
            //modelBuilder.Entity<OnlineCourse>()
            //        .Property(t => t.URL)
            //        .IsConcurrencyToken();


            //类型映射
            //将类指定为复杂类型
            //按约定，没有指定主键的类型将被视为复杂类型。在一些情况下，Code First 不会检测复杂类型（例如，如果您有名为“ID”的属性，但不想将它用作主键）。在此类情况下，您将使用 Fluent API 显式指定某类型是复杂类型。
            //modelBuilder.ComplexType<Details>();


            //指定不将CLR 实体类型映射到数据库中的表
            //以下示例显示如何排除一个 CLR 类型，使之不映射到数据库中的表。
            // modelBuilder.Ignore<OnlineCourse>();


            //将CLR 实体类型映射到数据库中的特定表
            //Department 的所有属性都将映射到名为 t_ Department 的表中的列。
            //modelBuilder.Entity<Department>().ToTable("t_Department");
            //您也可以这样指定架构名称：
            //modelBuilder.Entity<Department>().ToTable("t_Department", "dbo");

            //映射“每个层次结构一张表(TPH)”继承
            //在 TPH 映射情形下，继承层次结构中的所有类型都将映射到同一个表。鉴别器列用于标识每行的类型。使用 Code First 创建模型时，TPH 参与继承层次结构的类型所用的默认策略。默认情况下，鉴别器列将添加到名为“Discriminator”的表，且层次结构中每个类型的 CLR 类型名称都将用作鉴别器值。可以使用 Fluent API 修改默认行为。
            //modelBuilder.Entity<Course>()
            //        .Map<Course>(m => m.Requires("Type").HasValue("Course"))
            //        .Map<OnsiteCourse>(m => m.Requires("Type").HasValue("OnsiteCourse"));


            //映射“每个类型一张表(TPT)”继承
            //在 TPT 映射情形下，所有类型分别映射到不同的表。仅属于某个基类型或派生类型的属性存储在映射到该类型的一个表中。映射到派生类型的表还会存储一个将派生表与基表联接的外键。
            //modelBuilder.Entity<Course>().ToTable("Course");
            //modelBuilder.Entity<OnsiteCourse>().ToTable("OnsiteCourse");

            //映射“每个具体类一张表(TPC)”继承
            //在 TPC 映射情形下，层次结构中的所有非抽象类型分别映射到不同的表。映射到派生类的表与映射到数据库中基类的表并无关系。类的所有属性（包括继承属性）都将映射到相应表的列。
            //调用MapInheritedProperties 方法来配置每个派生类型。MapInheritedProperties 将继承自基类的所有属性重新映射到派生类的表中的新列。
            //注意：因为属于TPC 继承层次结构的表并不使用同一个主键，因此，如果您让数据库生成的值具有相同标识种子，则在映射到子类的表中执行插入操作时，会产生重复的实体键。要解决此问题，可以为每个表指定不同的初始种子值，或关闭主键属性的标识。当使用 Code First 时，标识就是整数键属性的默认值。

            //modelBuilder.Entity<Course>()
            //    .Property(c => c.CourseID)
            //    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            //排除类型
            modelBuilder.Ignore<Course>();

            modelBuilder.Entity<OnsiteCourse>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("OnsiteCourse");
            }).HasKey(c => c.CourseID).Property(c => c.CourseID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<OnlineCourse>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("OnlineCourse");
            }).HasKey(c => c.CourseID).Property(c => c.CourseID).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);



            //将实体类型的CLR 属性映射到数据库中的多个表（实体拆分）
            //实体拆分允许一个实体类型的属性分散在多个表中。在以下示例中，Department 实体拆分到两个表中：Department 和DepartmentDetails。实体拆分通过多次调用 Map 方法将一部分属性映射到特定表。
            //modelBuilder.Entity<Department>()
            //.Map(m =>
            //{
            //    m.Properties(t => new { t.DepartmentID, t.Name });
            //    m.ToTable("Department");
            //})
            //.Map(m =>
            //{
            //    m.Properties(t => new { t.DepartmentID, t.Administrator, t.StartDate, t.Budget });
            //    m.ToTable("DepartmentDetails");
            //});


            //将多个实体类型映射到数据库中的一个表（表拆分）
            //以下示例将使用同一个主键的两个实体类型映射到同一个表。
            //modelBuilder.Entity<OfficeAssignment>()
            //  .HasKey(t => t.InstructorID);
            //modelBuilder.Entity<Instructor>()
            //    .HasRequired(t => t.OfficeAssignment)
            //    .WithRequiredPrincipal(t => t.Instructor);
            //modelBuilder.Entity<Instructor>().ToTable("Instructor");
            //modelBuilder.Entity<OfficeAssignment>().ToTable("Instructor");


            //使用FluentAPI配置关系
            //简介
            //使用FluentAPI配置关系的时候，首先要获得一个EntityTypeConfiguration实例，然后使用其上的HasRequired, HasOptional或者 HasMany方法来指定当前实体参与的关系类型。HasRequired 和HasOptional方法需要一个lambda表达式来指定一个导航属性，HasMany方法需要一个lambda表达式指定一个集合导航属性。然后可以使用WithRequired, WithOptional和WithMany方法来指定反向导航属性，这些方法有不带参数的重载用来指定单向导航。
            //之后还可以使用HasForeignKey方法来指定外键属性。
            //配置【必须-可选】关系（1-0..1）
            //OfficeAssignment的键属性不符合命名约定，所以需要我们显式指定。下面的关系表明，OfficeAssignment的Instructor必须存在，但是Instructor的OfficeAssignment不是必须存在的。
            //modelBuilder.Entity<OfficeAssignment>()
            //  .HasKey(t => t.InstructorID);

            // Map one-to-zero or one relationship
            //modelBuilder.Entity<OfficeAssignment>()
            //  .HasRequired(t => t.Instructor)
            //  .WithOptional(t => t.OfficeAssignment);
            //配置两端都是必须的关系（1-1）
            //大多数情况下，EF都能推断哪一个类型是依赖项或者是主体项。然而当关系的两端都是必须的或者都是可选的，那么EF就不能识别依赖项或者是主体项。如果关系两端都是必须的，那么在HasRequired方法后使用WithRequiredPrincipal或者WithRequiredDependent来确定主体。如果关系两端都是可选的，那么在HasRequired方法后使用WithOptionalPrincipal和WithOptionalDependent。

            //modelBuilder.Entity<OfficeAssignment>()
            //  .HasKey(t => t.InstructorID);

            //modelBuilder.Entity<Instructor>()
            //  .HasRequired(t => t.OfficeAssignment)
            //  .WithRequiredPrincipal(t => t.Instructor);

            //配置多对多关系
            //下面的代码配置了一个多对多关系，CodeFirst会使用命名约定来创建连接表，命名约定会使用Course_CourseID 和 Instructor_InstructorID作为连接表的列。

            //modelBuilder.Entity<Course>()
            //        .HasMany(t => t.Instructors)
            //        .WithMany(t => t.Courses);
            //如果想指定连接表的表名和列名，需要使用Map方法，如下：
            //modelBuilder.Entity<Course>()
            //        .HasMany(t => t.Instructors)
            //        .WithMany(t => t.Courses)
            //        .Map(m =>
            //        {
            //            m.ToTable("CourseInstructor");
            //            m.MapLeftKey("CourseID");
            //            m.MapRightKey("InstructorID");
            //        });

            //配置单向导航属
            //所谓单向导航属性指的是只在关系的一端定义了导航属性。按照约定，CodeFirst将单向导航理解为一对多关系，如果需要一对一的单向导航属性，需要使用如下方法：
            //modelBuilder.Entity<OfficeAssignment>()
            //  .HasKey(t => t.InstructorID);

            //modelBuilder.Entity<Instructor>()
            //  .HasRequired(t => t.OfficeAssignment)
            //  .WithRequiredPrincipal();

            //启用级联删除
            //使用WillCascadeOnDelete方法来配置关系是否允许级联删除。如果外键是不可空的，CodeFirst默认会设置级联删除；否则，不会设置级联删除，当主体被删除后，外键将会被置空。
            //可以使用如下代码移除此约定：
            //modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.ManyToManyCascadeDeleteConvention>();
            //下面的代码片段配置为外键不能为空，而且禁用了级联删除。

            //modelBuilder.Entity<Course>()
            //  .HasRequired(t => t.Department)
            //  .WithMany(t => t.Courses)
            //  .HasForeignKey(d => d.DepartmentID)
            //  .WillCascadeOnDelete(false);
            //配置组合外键
            //下面的代码配置了组合外键
            //modelBuilder.Entity<Department>()
            //  .HasKey(d => new { d.DepartmentID, d.Name });

            // Composite foreign key
            //modelBuilder.Entity<Course>()
            //    .HasRequired(c => c.Department)
            //    .WithMany(d => d.Courses)
            //    .HasForeignKey(d => new { d.DepartmentID, d.DepartmentName });

            //配置不符合命名约定的外键属性
            //SomeDepartmentID属性不符合外键命名约定，需要使用如下方法将其设置为外键属性：
            //modelBuilder.Entity<Course>()
            //         .HasRequired(c => c.Department)
            //         .WithMany(d => d.Courses)
            //         .HasForeignKey(c => c.SomeDepartmentID);

        }
        //定义DbSet
        //DbContext 使用DbSet 属性
        //需要和数据库访问的实体定义
        //在 CodeFirst 模式下使用时，这会将 Unicorn、Princess、LadyInWaiting 和Castle 配置为实体类型，也将配置可从这些类型访问的其他类型。此外，DbContext 还将自动对其中每个属性调用 setter 以设置相应 DbSet 的实例
        public DbSet<Department> Departments { get; set; }

        public DbSet<OnlineCourse> OnlineCourses { get; set; }

        public DbSet<OnsiteCourse> OnsiteCourses { get; set; }

        //此上下文的工作方式与对其 set 属性使用DbSet 类的上下文完全相同。
        //DbContext 使用只读set 属性
        //如果不希望为DbSet 或 IDbSet 属性公开公共 setter，可以改为创建只读属性并自建 set 实例。例如：
        public DbSet<Blog> Blogs
        {
            get { return Set<Blog>(); }
        }

        public DbSet<Post> Posts
        {
            get { return Set<Post>(); }
        }
        //请注意，DbContext将缓存从 Set 方法返回的 DbSet 实例，以便每次调用其中每个属性时都返回同一实例。
        //搜索 CodeFirst 实体类型的工作方式与搜索具有公共 getter 和setter 的属性相同。

        public DbSet<Person> Persons { get; set; }


    }


    public class EFDBContextDropCreateInitializer : DropCreateDatabaseAlways<EFDBContext>
    {
        protected override void Seed(EFDBContext context)
        {
            //预制数据或者测试数据
            Department department = new Department();
            department.Name = "主管";
            context.Departments.Add(department);
            base.Seed(context);
        }
    }
}
