using EF事例.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EF事例.Models;
using System.Configuration;
using System.Data.SqlClient;


namespace EF事例
{
    class Program
    {
        //创建访问类
        private static EFDBContext db = new EFDBContext();

        static void Main(string[] args)
        {

            /*
             * 
             * EF简单介绍：EF框架可以在不同应用平台上使用
             * EF分别有三种模式 Code First ，Database First ，Model First
             * 当前只介绍 Code First 模式
             * 
             */

            /*
             * 
             *1.首先安装EF框架，在VS界面中选择工具->NuGet 程序包管理器->管理解决方案的 NuGet 程序包中安装 EntityFramework
             *2.引入 System.ComponentModel.DataAnnotations 类库 
             *  该类库主要使用 System.ComponentModel.DataAnnotations.Schema 空间中标签，来标记属性在数据库中数据列名，长度，
             *  规则。
             *  引入EF后程序会在 App.config 或者 Web.config中自动添加
             *  
             * 第一个节点插入
             *   <configSections>
             *       <!-- For more information on Entity Framework configuration, visit http://go.microsoft.com/fwlink/?LinkID=237468 -->
             *           <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
             * </configSections>
             * 
             * 
             * 最后的节点中插入
             * <entityFramework>
             *     <defaultConnectionFactory type="System.Data.Entity.Infrastructure.LocalDbConnectionFactory, EntityFramework">
             *           <parameters>
             *                   <parameter value="mssqllocaldb" />
             *           </parameters>
             *     </defaultConnectionFactory>
             *     <providers>
             *           <provider invariantName="System.Data.SqlClient" type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
             *     </providers>
             * </entityFramework>
             * 
             */


            /*
             *3.首先创建Model(在Models文件夹中)
             *4.在创建一个继承DbContext的操作类 DBContext 当前存放在 DAL 文件中
             * 
             */



            /*
             * 不丢失数据进行数据库结构升级 简称 数据迁移
             * 更新数据库表结构 在VS界面中选择工具->NuGet 程序包管理器->程序包管理器控制台
             * 输入 enable-migrations 
             * 再输入add-migration InitialCreate (注：InitialCreate 是自己定义的后续名称，
             *     输入后会自动创建命名为 时间间隔_InitialCreate 的类，可以在该类中的 UP Down 方法中修改对应数据库的表中属性)
             * 该命令输入后将会 创建 Migrations 文件夹 存放这些记录变迁的类文件其中还有一个Configuration.cs 在该类中主要是
             * Seed方法用来做一些插入预制的数据或者测试数据来使用。
             * 最后输入 update-database VS马上更新数据库
             */


            Console.WriteLine("开始使用 DbContext的DbSet来进行数据操作");

            //DbSet使用注意事项
            //DbSet总是针对数据库执行查询，即使要查询的数据已经在上下文中，下面几种情况下会执行数据库查询。
            //·执行foreach
            //·调用ToArray, ToDictionary, ToList.
            //·在最外层查询调用LINQ操作符First，Any等等。
            //·DbSet的扩展方法Load，DbEntityEntry.Reload，Database.ExecuteSqlCommand.
            //当数据库返回查询结果的时候，如果结果集中的对象在context中不存在，那么就会将对象attach到上下文中。如果对象已经存在（根据id来判断），那么就会返回在上下文中已经存在的对象，数据库的值不会覆盖当前对象database values。在这种情况下，如果我们长时间持有DbContext，那么我们在每次查询的时候得到就很有可能不是最新版本的对象。
            //在执行一个查询的时候，上下文中新添加但是还没有保存的对象不会作为查询结果返回，如果想访问这些对象，需要访问Local属性。下面是关于local属性的备注
            //1.Local属性不只是包含新添加的对象，它包含所有已经加载到context中的对象。
            //2.Local属性不包含那些已经被Remove的对象（上下文中remove了，但是还在数据库中）
            //3.查询结果永远反应数据库的真实数据，在上下文中被Remove了但是还没有在数据库删除的对象，仍然可以查询到。DbContext.ChangeTracker属性提供了DbChangeTracker的实例，该实例的Entries属性返回一个DbEntityEntry集合，可以找到所有当前上下文中跟踪的实体及其状态信息。
            //有时候在查询大量实体并只进行只读操作的时候，实体跟踪是没有任何意义的，禁用实体跟踪会提高查询性能，可以AsNoTracking方法来禁用实体跟踪，例如：
            //using (var context = new EFDBContext())
            //{
            //    // Query for allblogs without tracking them
            //    var blogs1 = context.Blogs.AsNoTracking();

            //    // Query for someblogs without tracking them
            //    var blogs2 = context.Blogs
            //                        .Where(b => b.Title.Contains(".NET"))
            //                        .AsNoTracking()
            //                        .ToList();
            //    blogs2.ForEach(s => Console.WriteLine(s.Title));
            //}

            //    string connectionString =
            //ConfigurationManager.ConnectionStrings["EFDemo"].ConnectionString.ToString();  

            Console.WriteLine("结束");
            Console.ReadLine();
        }


        //根据主键查找实体

        //        DbSet.Find方法会根据主键来查找被上下文跟踪的实体。如果上下文中不存在此对象，那么将会对数据库进行查询来查找实体，如果没有找到实体，则返回null。Find方法可以查询到刚刚添加到上下文但是还没有被保存到数据库的实体，这与LINQ查询不同。
        //使用 Find 方法时必须考虑：
        //1.        如果对象没有在缓存中，则 Find 没有优势，但语法仍比按键进行查询简单。
        //2.        如果启用自动检测更改，则根据模型的复杂性以及对象缓存中的实体数量，Find 方法的成本可能会增加一个数量级，甚至更多。
        //此外，请注意Find 仅返回要查找的实体，它不会自动加载未在对象缓存中的关联实体。如果需要检索关联实体，可通过预先加载使用按键查询。

        //创建和修改关系
        //对于拥有外键属性的关系，修改关系是非常简单的，如下：
        //course.DepartmentID =newCourse.DepartmentID;
        //下面的代码通过将外键设置为 null 删除了关系。请注意，外键属性必须可以为 Null。
        //course.DepartmentID = null;
        //注意：如果引用处于已添加状态（在本例中为 course 对象），在调用 SaveChanges 之前，引用导航属性将不与新对象的键值同步。由于对象上下文在键值保存前不包含已添加对象的永久键，因此不发生同步。
        //通过将一个新对象分配给导航属性。下面的代码在 course 和department 之间创建关系。如果对象附加到上下文，course 也会添加到 department.Courses 集合中，course 对象的相应的外键属性设置为 department 的键属性值。
        //course.Department =department;
        //要删除该关系，请将导航属性设置为 null。如果使用的是基于 .NET 4.0 的实体框架，则需要先加载相关端，然后再将其设置为 Null。例如：
        //context.Entry(course).Reference(c=> c.Department).Load();
        //course.Department = null;
        //从实体框架5.0（它基于 .NET 4.5）开始，不必加载相关端就可以将关系设置为 Null。也可以使用以下方法将当前值设置为 Null。
        //context.Entry(course).Reference(c=> c.Department).CurrentValue = null;
        //通过在实体集合中删除或添加对象。例如，可以将 Course 类型的对象添加到 department.Courses 集合中。此操作将在特定 course 和特定 department 之间创建关系。如果对象附加到上下文，course 对象的 department 引用和外键属性将设置为相应的 department。
        //department.Courses.Add(newCourse);
        //此处，如果course的departmentid不能为空，则可能会出现错误，对department.Courses集合不能有删除course的操作，否则会出现错误。因为如果从集合中移除了course，在SaveChanges过程中把该过程识别为更新关系，而那些被删除的course的departmentid又不能为空，所以save不会成功。
        //通过使用 ChangeRelationshipState方法更改两个实体对象间指定关系的状态。此方法是处理 N 层应用程序和独立关联 时最常用的方法（不能用于外键关联）。此外，要使用此方法，必须下拉到 ObjectContext，如下例所示。
        //在下面的示例中，Instructor和 Course 之间存在多对多关系。调用 ChangeRelationshipState 方法并传递 EntityState.Added 参数，使 SchoolContext 知道在这两个对象间添加了关系。
        //       ((IObjectContextAdapter)context).ObjectContext.ObjectStateManager.
        //                 ChangeRelationshipState(course, instructor, c => c.Instructor,EntityState.Added);  
        //请注意，如果是更新（而不仅是添加）关系，添加新关系后必须删除旧关系：
        //      ((IObjectContextAdapter)context).ObjectContext. ObjectStateManager.
        //                 ChangeRelationshipState(course, oldInstructor, c => c.Instructor,EntityState.Deleted);   
        static void FKDemo()
        {
            //同步FK 和导航属性之间的更改
            //使用上述方法中的一种更改附加到上下文的对象的关系时，实体框架需要保持外键、引用和集合同步。实体框架使用代理自动管理 POCO 实体的这种同步（也称为关系修复）。
            //如果不通过代理使用POCO 实体，则必须确保调用 DetectChanges 方法同步上下文中的相关对象。请注意，下面的 API 会自动触发 DetectChanges 调用。
            //·         DbSet.Add
            //·         DbSet.Find
            //·         DbSet.Remove
            //·         DbSet.Local
            //·         DbContext.SaveChanges
            //·         DbSet.Attach
            //·         DbContext.GetValidationErrors
            //·         DbContext.Entry
            //·         DbChangeTracker.Entries
            //·         对 DbSet 执行 LINQ 查询
            //如果context中有很多实体，而且你正在多次调用上述方法，那么就会造成很大的性能影响。可以使用下面的代码来的代码禁用自动检测：

            //using (var context = new EFDBContext())
            //{
            //    try
            //    {
            //        context.Configuration.AutoDetectChangesEnabled = false;

            //        // Make manycalls in a loop
            //        foreach (var blog in aLotOfBlogs)
            //        {
            //            context.Blogs.Add(blog);
            //        }
            //    }
            //    finally
            //    {
            //        context.Configuration.AutoDetectChangesEnabled = true;
            //    }
            //}
            //除了以上方法，还可以调用context.ChangeTracker.DetectChanges方法来显式检测变化。需要小心使用这些高级方法，否则很容易在你的程序里引入微妙的bug。



        }
        //加载相关对象
        //
        static void EagerlyLoading()
        {
            //预加载（EagerlyLoading）
            //预加载表示在查询某类实体时一起加载相关实体，这是使用Include方法完成的，如下：

            using (var context = new EFDBContext())
            {
                // Load all blogsand related posts
                var blogs1 = context.Blogs
              .Include(b => b.Posts)
              .ToList();

                // Load one blogsand its related posts
                var blog1 = context.Blogs
              .Where(b => b.Title == "ADO.NET Blog")
              .Include(b => b.Posts)
              .FirstOrDefault();

                // Load all blogsand related posts
                // using a stringto specify the relationship
                var blogs2 = context.Blogs
                                      .Include("Posts")
                                      .ToList();

                // Load one blogand its related posts
                // using a stringto specify the relationship
                var blog2 = context.Blogs
                                    .Where(b => b.Title == "ADO.NET Blog")
                                    .Include("Posts")
                                    .FirstOrDefault();
            }
            //注意：Include方法是一个扩展方法，在System.Data.Entity命名空间下，确保引用了此命名空间。

        }

        //多级预加载
        static void EagerlyLoading2()
        {
            //下面的代码显示了如何加载多级实体。
            //using (var context = new EFDBContext())
            //{
            //    // Load all blogs,all related posts, and all related comments
            //    var blogs1 = context.Blogs
            //                       .Include(b => b.Posts.Select(p => p.Comments))
            //                       .ToList();

            //    // Load all userstheir related profiles, and related avatar
            //    var users1 = context.Users
            //                        .Include(u => u.Profile.Avatar)
            //                        .ToList();

            //    // Load all blogs,all related posts, and all related comments
            //    // using a stringto specify the relationships
            //    var blogs2 = context.Blogs
            //                       .Include("Posts.Comments")
            //                       .ToList();

            //    // Load all userstheir related profiles, and related avatar
            //    // using a stringto specify the relationships
            //    var users2 = context.Users
            //                        .Include("Profile.Avatar")
            //                        .ToList();
            //}
            //当前不支持在关联实体上进行查询，Include方法总是加载所有关联实体。
        }

        //惰性加载
        static void demo3()
        {
            //惰性加载指的是当第一访问导航属性的时候自动从数据库加载相关实体。这种特性是由代理类实现的，代理类派生自实体类，并重写了导航属性。所以我们的实体类的导航属性就必须标记为virtual，如下：
            //public class Blog
            //{
            //  public int ID { get; set; }
            //  public string Title { get; set; }
            //  public virtual ICollection<Post>Posts { get; set;}
            //}
            //可以对指定实体关闭惰性加载，如下：
            //publicclass Blog
            //{
            //  public int ID { get; set; }
            //  public string Title { get; set; }
            //  public ICollection<Post>Posts { get; set;}
            //}
            //也可以对所有实体关闭惰性加载，如下：
            //public class EFDBContext: DbContext
            //{
            //  public EFDBContext()
            //  {
            //    this.Configuration.LazyLoadingEnabled= false;
            //  }
            //}
        }

        //显式加载
        static void demo4()
        {
            //即使关闭了惰性加载，我们仍然可以通过显式调用来延迟加载相关实体，这是通过调用DbEntityEntry上的相关方法做到的，如下：
            using (var context = new EFDBContext())
            {
                var post = context.Posts.Find(2);

                // Load the blogrelated to a given post
                context.Entry(post).Reference(p => p.Blog).Load();

                // Load the blogrelated to a given post using a string
                context.Entry(post).Reference("Blog").Load();

                var blog = context.Blogs.Find(1);

                // Load the postsrelated to a given blog
                context.Entry(blog).Collection(p => p.Posts).Load();

                // Load the postsrelated to a given blog
                // using a stringto specify the relationship
                context.Entry(blog).Collection("Posts").Load();
            }

            //注意：在外键关联中，加载依赖对象的相关端时，将根据内存中当前的相关外键值加载相关对象：
            // Get thecourse where currently DepartmentID = 1.
            //Course course2 =context.Courses.First(c=>c.DepartmentID == 2);
            // UseDepartmentID foreign key property
            // to change theassociation.
            //course2.DepartmentID = 3;
            // Load therelated Department where DepartmentID = 3
            //context.Entry(course).Reference(c=> c.Department).Load();
            //在独立关联中，基于当前数据库中的外键值查询依赖对象的相关端。不过，如果修改了关系，并且依赖对象的引用属性指向对象上下文中加载的不同主对象，实体框架将尝试创建关系，就像它在客户端定义的那样。
            //Query方法提供了在加载相关实体的时候应用过滤条件的功能，引用导航属和集合导航属性都支持Query方法，但是大部分情况下都会针对集合导航属性使用Query方法，达到只加载部分相关实体的功能，如下：
            using (var context = new EFDBContext())
            {
                var blog = context.Blogs.Find(1);

                // Load the postswith the 'entity-framework' tag related to a given blog
                context.Entry(blog)
                    .Collection(b => b.Posts)
                    .Query()
                    .Where(p => p.Title.Contains("entity-framework"))
                    .Load();

                // Load the postswith the 'entity-framework' tag related to a given blog
                // using a stringto specify the relationship
                context.Entry(blog)
                    .Collection("Posts")
                    .Query()
                    .Cast<Post>()
                    .Where(p => p.Title.Contains("entity-framework"))
                    .Load();
            }
            //注意，使用显式加载的时候，最好关闭惰性加载，避免引起混乱。Load方法是一个扩展方法，记得引用命名空间System.Data.Entity

            //使用Query查询相关实体个数，而不用加载相关实体，如下：
            using (var context = new EFDBContext())
            {
                var blog = context.Blogs.Find(1);

                // Count how manyposts the blog has
                var postCount = context.Entry(blog)
                                      .Collection(b => b.Posts)
                                      .Query()
                                      .Count();
            }
        }


        //使用代理
        //为 POCO 实体类型创建实例时，实体框架常常为充当实体代理的动态生成的派生类型创建实例。此代理重写实体的某些虚拟属性，这样可在访问属性时插入挂钩，从而自动执行操作。例如，此机制用于支持关系的延迟加载。
        //禁止创建代理
        //有时需要禁止实体框架创建代理实例。例如，人们通常认为序列化非代理实例要比序列化代理实例容易得多。可通过清除 ProxyCreationEnabled 标记来关闭代理创建功能。上下文的构造函数便是可执行此操作的一个位置。例如：
        //public class BloggingContext: DbContext
        //{
        //  public BloggingContext()
        //  {
        //    this.Configuration.ProxyCreationEnabled= false;
        //  }

        //  public DbSet<Blog>Blogs { get; set;}
        //  public DbSet<Post>Posts { get; set;}
        //}
        //请注意，在无需代理执行任何操作的情况下，EF 不会为类型创建代理。这意味着，也可以通过使用封装和/或没有虚拟属性的类型，避免生成代理。

        //添加新实体
        static void demo5()
        {
            //使用DbSet.Add方法添加实体
            using (var context = new EFDBContext())
            {
                var blog = new Blog { Title = "ADO.NET Blog" };
                context.Blogs.Add(blog);
                context.SaveChanges();
            }

            //修改Entry的State来添加实体
            using (var context = new EFDBContext())
            {
                var blog = new Blog { Title = "ADO.NET Blog" };
                context.Entry(blog).State = EntityState.Added;
                context.SaveChanges();
            }
            //设置导航属性来添加实体
            using (var context = new EFDBContext())
            {
                // Add a new Userby setting a reference from a tracked Blog
                //var blog = context.Blogs.Find(1);
                //blog.Owner = new User { UserName = "johndoe1987" };

                // Add a new Postby adding to the collection of a tracked Blog
                var blog = context.Blogs.Find(2);
                blog.Posts.Add(new Post { Title = "Howto Add Entities" });

                context.SaveChanges();
            }
            //所有被添加到上下文中的实体的引用实体，如果没有被跟踪，就会被当作新实体添加到上下文中，并在调用SaveChanges方法后被保存到数据库。

        }

        //附加实体到上下文
        static void demo6()
        {
            //如果实体在数据库中存在，但是没有被上下文跟踪，可是使用DbSet.Attach方法将其附加到上下文，附加之后，实体处于Unchanged状态。处于Unchanged状态的实体不会参与SaveChanges的逻辑。

            var existingBlog = new Blog { PrimaryTrackingKey = 1, Title = "ADO.NET Blog" };

            using (var context = new EFDBContext())
            {
                context.Blogs.Attach(existingBlog);

                // Do some morework...

                context.SaveChanges();
            }
            //设置DbEntityEntry对象的State属性，也可以附加对象到上下文中，如下：

            using (var context = new EFDBContext())
            {
                context.Entry(existingBlog).State = EntityState.Unchanged;

                // Do some morework...

                context.SaveChanges();
            }
            //使用上述两种方法附加到上下文的实体如果还引用其他实体，那么这些实体也会被附加到上下文中，状态为Unchanged
            //使用如下方法附加一个存在于数据库，但是还没有附加到上下文的已修改实体：


            using (var context = new EFDBContext())
            {
                context.Entry(existingBlog).State = EntityState.Modified;

                // Do some morework...

                context.SaveChanges();
            }
            //如果把一个实体的状态置为Modified，那么该实体的所有属性都将被标记为已更改状态，当SaveChanges被调用时，所有的属性值都将被保存到数据库。如果不想保存所有值，可以单独为每个想要修改的属性设置IsModified属性，如下：
            using (var context = new EFDBContext())
            {
                var blog = context.Blogs.Find(1);

                context.Entry(blog).Property(u => u.Title).IsModified = true;

                // Use a stringfor the property name
                context.Entry(blog).Property("Name").IsModified = true;
            }

            //如果该实体还引用其他未被跟踪实体，那么这些实体将会作为Unchanged状态的实体附加到上下文。如果想修改这些实体，只能单独把每个引用实体设置为修改状态。

        }


        //最后
        //乐观并发模式
        static void optimistic()
        {
            //乐观并发模式
            //在尝试保存使用外键关联的实体期间，如果检测到乐观并发异常，SaveChanges 将引发 DbUpdateConcurrencyException。DbUpdateConcurrencyException的 Entries 方法为无法更新的实体返回 DbEntityEntry 实例。
            //使用DbContext执行原始SQL
            //使用SqlQuery方法执行SQL查询
            using (var context = new EFDBContext())
            {

                var blogs = context.Departments.SqlQuery("SELECT * FROM dbo.Departments").ToList();
            }
            //执行存储过程查询
            using (var context = new EFDBContext())
            {
                var blogs = context.Departments.SqlQuery("dbo.Departments").ToList();
            }


            //为存储过程传递参数
            using (var context = new EFDBContext())
            {
                var blogId = 1;

                var blogs = context.Departments.SqlQuery("dbo.GetBlogById@p0", blogId).Single();

                //带输入输出参数
                // 存储过程为
                //create procedure proc_Info_OutCount
                //@in_Birthday datetime,
                //@Count int output
                //as
                //begin
                //    .........
                //    select @Count=COUNT(*) from Info where Birthday=@in_Birthday;
                //end;
                //使用方法
                ////定义输出参数
                //SqlParameter paramout = new SqlParameter("Count", -1);
                //paramout.Direction = System.Data.ParameterDirection.Output;
                ////定义输入参数
                //SqlParameter paramin=new SqlParameter("in_Birthday",Convert.ToDateTime("1985-09-01"));
                //var queryList = context.Database.SqlQuery<Person>("exec proc_Info_OutCount_Str @in_Biirthday,@Count output", new Object[] { paramout, paramin }).ToList();
                //string outCount = paramout.Value.ToString();
            }
            //查询非实体类型
            using (var context = new EFDBContext())
            {
                var blogNames = context.Database.SqlQuery<string>(
                                   "SELECTName FROM dbo.Departments").ToList();
            }
            //返回是的对象将不会被跟踪，即使返回类型是实体类型。
            //执行SQL命令
            using (var context = new EFDBContext())
            {
                context.Database.ExecuteSqlCommand(
                    "UPDATE dbo.Departments SET Name = 'Another Name' WHERE BlogId = 1");
            }
        }
    }
}
