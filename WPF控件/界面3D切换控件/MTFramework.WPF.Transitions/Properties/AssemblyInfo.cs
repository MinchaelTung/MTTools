using System.Reflection;
using System.Runtime.InteropServices;
using MTFramework.Engines.MapPathfindingEngine;

// 有关程序集的常规信息通过以下
// 特性集控制。更改这些特性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle(ProductsInfo.Name + "[" + ProductsInfo.Configuration + "]")]
[assembly: AssemblyDescription(ProductsInfo.Name + "[" + ProductsInfo.Configuration + "]")]
[assembly: AssemblyConfiguration(ProductsInfo.Configuration)]
[assembly: AssemblyCompany(CompanyInfo.CompanyName)]
[assembly: AssemblyProduct(ProductsInfo.Name)]
[assembly: AssemblyCopyright(CompanyInfo.Copyright)]
[assembly: AssemblyTrademark(CompanyInfo.CompanyName)]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 使此程序集中的类型
// 对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型，
// 则将该类型上的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("b0b48a4e-891f-4823-b32d-4cda32b2a4ca")]

// 程序集的版本信息由下面四个值组成:
//
//      主版本
//      次版本 
//      内部版本号
//      修订号
//
// 可以指定所有这些值，也可以使用“内部版本号”和“修订号”的默认值，
// 方法是按如下所示使用“*”:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(ProductsInfo.Version)]
[assembly: AssemblyFileVersion(SetupInfo.FileVersion)]
