using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF事例.Models
{
    //表示该类是复杂类型。
    //把该类作为另一个类的复杂内容的一部分，该类可以没有主键
    //[ComplexType]
    //标记数据库名称 主要参数为数据库对应的表名，可选参数Schema数据库的主域默认dbo
    //[Table("Department", Schema = "dbo")]
    //[Table("Department")]
    public class Department
    {
        // Primary key 主键默认规则 ID 或者 类名+ID
        //手动标签指定
        // [Key]
        //指定在数据库中列名 TypeName=数据库中的类型如：ntext ，text等
        //[Column("ID")]
        //[Column("ID", TypeName = "int")]
        public int DepartmentID { get; set; }

        //必要 数据库对应为 not null
        //[Required]
        //MaxLength 最大长度
        //MinLength 最少长度
        //[MaxLength(10), MinLength(5)]//或者 
        //[MinLength(5)]
        //[MaxLength(10)]
        //
        public string Name { get; set; }

        //不反射该属性
        [NotMapped]
        // DatabaseGenerated
        //一个重要的数据库功能是可以使用计算属性。如果您将 Code First 类映射到包含计算列的表，
        //则您可能不想让实体框架尝试更新这些列。但是在插入或更新数据后，您的确需要 EF 从数据库
        //中返回这些值。您可以使用 DatabaseGenerated 注释与 Computed 枚举一起在您的类中标注这些
        //属性。其他枚举为 None 和Identity。
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //外键
        //[ForeignKey("CourseId")]
        public int TestInt { get; set; }

        // Navigationproperty
        public virtual ICollection<Course> Courses { get; set; }
    }



    // InverseProperty

    //您还需要在这些属性引用的 Person 类中添加内容。Person类具有返回到 Post 的导航属性，一个属性指向该用户撰写的所有文章，一个属性指向该用户更新的所有文章。
    //publicclass Person
    //{
    //  public int Id { get; set; }
    //  public string Name { get; set; }
    //  public List<Post>PostsWritten { get; set;}
    //  public List<Post>PostsUpdated { get; set;}
    //}
    //Code First 不能自行使这两个类中的属性匹配。Posts 的数据库表应该有一个表示 CreatedBy 人员的外键，有一个表示 UpdatedBy 人员的外键，但是 Code First 将创建四个外键属性：Person_Id、Person_Id1、CreatedBy_Id 和UpdatedBy_Id。(针对每个导航属性创建一个外键)
    //要解决这些问题，您可以使用 InverseProperty 注释来指定这些属性的匹配。
    //[InverseProperty("CreatedBy")]
    //publicList<Post>PostsWritten { get; set;}

    //[InverseProperty("UpdatedBy")]
    //publicList<Post>PostsUpdated { get; set;}


}
