// CSharpInvokeCPP.CPPDemo.cpp : ���� DLL Ӧ�ó���ĵ���������
//

#include "stdafx.h"

//����DLL 32 ��Ŀ Ĭ��

//�ؼ����η� extern "C" ���� EXTERN_C ԭ���� Ĭ�϶���  #define EXTERN_C    extern "C"
//   extern "C" ����˫�غ��壬�������ϼ��ɵõ������ȣ��������ε�Ŀ���ǡ�extern���ģ�
//   ��Σ��������ε�Ŀ���ǡ�C���ġ�����extern "C"���εı����ͺ����ǰ���C���Է�ʽ��������ӵġ�

// __declspec(dllexport)
//__declspec(dllexport)��Ŀ����Ϊ�˽���Ӧ�ĺ������뵽DLL��̬���С�
//   extern "C" __declspec(dllexport)��������Ŀ����Ϊ��ʹ��DllImport���÷��й�C++��DLL�ļ���
//   ��Ϊʹ��DllImportֻ�ܵ�����C���Ժ������ɵ�DLL��
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
//������Ŀ���������DebugĿ¼����CSharpInvokeCPP.CPPDemo.dll��CSharpInvokeCPP.CPPDemo.lib
//���������ļ��ŵ�����Ҫִ�еĳ�����
