package util;
import java.io.*;
import java.text.*;
import java.util.*;

public class UtilTool{

	// 获取文件名称
	public String getFileName(String fullFileName){
		int i = fullFileName.lastIndexOf("\\");
		return fullFileName.substring(i+1);		// 获取文件名

	}

	// 获取文件扩展名
	public String getFileExtName(String fullFileName){
		int i = fullFileName.lastIndexOf("\\");		// 最后一个"\"的位置
		int j = fullFileName.lastIndexOf(".");		// 最后一个"."的位置
		if(i<j) {	// 存在扩展名
			return fullFileName.substring(j+1);		// 获取文件名
		} else {	// 不存在扩展名
			return "";
		}
	}

	// 自定义格式日期、时间
	public String getDateTime(String format){
		SimpleDateFormat dateFormat = new SimpleDateFormat(format);
		java.util.Date cur_date = new java.util.Date();
		return dateFormat.format(cur_date);
	}
	// 完全格式的日期、时间
	public String getDateTime(){

		SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		java.util.Date cur_date = new java.util.Date();
		return dateFormat.format(cur_date);
	}
	// 获取日期
	public String getDate(){

		SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd");
		java.util.Date cur_date = new java.util.Date();
		return dateFormat.format(cur_date);
	}
	// 获取时间
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