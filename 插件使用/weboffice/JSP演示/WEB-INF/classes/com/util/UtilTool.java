package util;
import java.io.*;
import java.text.*;
import java.util.*;

public class UtilTool{

	// ��ȡ�ļ�����
	public String getFileName(String fullFileName){
		int i = fullFileName.lastIndexOf("\\");
		return fullFileName.substring(i+1);		// ��ȡ�ļ���

	}

	// ��ȡ�ļ���չ��
	public String getFileExtName(String fullFileName){
		int i = fullFileName.lastIndexOf("\\");		// ���һ��"\"��λ��
		int j = fullFileName.lastIndexOf(".");		// ���һ��"."��λ��
		if(i<j) {	// ������չ��
			return fullFileName.substring(j+1);		// ��ȡ�ļ���
		} else {	// ��������չ��
			return "";
		}
	}

	// �Զ����ʽ���ڡ�ʱ��
	public String getDateTime(String format){
		SimpleDateFormat dateFormat = new SimpleDateFormat(format);
		java.util.Date cur_date = new java.util.Date();
		return dateFormat.format(cur_date);
	}
	// ��ȫ��ʽ�����ڡ�ʱ��
	public String getDateTime(){

		SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		java.util.Date cur_date = new java.util.Date();
		return dateFormat.format(cur_date);
	}
	// ��ȡ����
	public String getDate(){

		SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd");
		java.util.Date cur_date = new java.util.Date();
		return dateFormat.format(cur_date);
	}
	// ��ȡʱ��
	public String getTime(){

		SimpleDateFormat dateFormat = new SimpleDateFormat("HH:mm:ss");
		java.util.Date cur_date = new java.util.Date();
		return dateFormat.format(cur_date);
	}

	public static void main(String[] args){
		System.out.println(getTime());
	}	

	public synchronized String GenerateDocID(){
		Date d = new Date();
        long l =d.getTime();
		return new Long(l).toString();
	}

}