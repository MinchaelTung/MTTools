// CSharpInvokeCPP.CPPDemo.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"

//创建DLL 32 项目 默认

//关键修饰符 extern "C" 或者 EXTERN_C 原因是 默认定义  #define EXTERN_C    extern "C"
//   extern "C" 包含双重含义，从字面上即可得到：首先，被它修饰的目标是“extern”的；
//   其次，被它修饰的目标是“C”的。而被extern "C"修饰的变量和函数是按照C语言方式编译和连接的。

// __declspec(dllexport)
//__declspec(dllexport)的目的是为了将对应的函数放入到DLL动态库中。
//   extern "C" __declspec(dllexport)加起来的目的是为了使用DllImport调用非托管C++的DLL文件。
//   因为使用DllImport只能调用由C语言函数做成的DLL。
extern "C" __declspec(dllexport) int Add(int x, int y)
{
	return x + y;
}

extern "C" __declspec(dllexport) int Sub(int x, int y)
{
	return x - y;
}

EXTERN_C __declspec(dllexport) int Multiply(int x, int y) {
	return x * y;
}

EXTERN_C __declspec(dllexport) int Divide(int x, int y) {
	return x / y;
}
//编译项目程序，最后在Debug目录生成CSharpInvokeCPP.CPPDemo.dll和CSharpInvokeCPP.CPPDemo.lib
//把这两个文件放到你需要执行的程序下
