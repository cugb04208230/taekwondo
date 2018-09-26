using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Common;
using Common.Data;
using Common.Extensions;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using NLog;
using Taekwondo.Data.Entities;

namespace Taekwondo.WebApi.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class BaseController:Controller
    {
	    /// <summary>
	    /// 用户账号Id
	    /// </summary>
	    protected long? UserAccountId => UserAccount?.Id;

		/// <summary>
		/// 用户账号信息
		/// </summary>
		protected UserAccount UserAccount=> GetUserAccount();

	    private UserAccount GetUserAccount()
	    {
		    var user = ViewBag.User as UserAccount;
		    return user;

	    }

	    /// <summary>
		/// 请求IP
		/// </summary>
	    public string Ip => GetIp();

	    private string GetIp()
		{
			var xfor = Request.Headers["X-Forwarded-For"].FirstOrDefault(); ;
			if (!string.IsNullOrEmpty(xfor))
			{
				return xfor;
			}
			var realRemoteIp = Request.Headers["REMOTE_ADDR"].FirstOrDefault();
			if (!string.IsNullOrEmpty(realRemoteIp))
			{
				return realRemoteIp;
			}
			return Request.Host.Host;
		}

		/// <summary>
		/// 数据格式验证
		/// </summary>
		/// <param name="dto"></param>
		protected void Validate(object dto)
		{
			var type = dto.GetType();
			var propertyInfos = type.GetProperties();
			foreach (var propertyInfo in propertyInfos)
			{
				var attr = propertyInfo.GetCustomAttribute<DtoValidateAttribute>();
				if (attr == null)
				{
					continue;
				}
				var desc = propertyInfo.GetCustomAttribute<DescriptionAttribute>()?.Description ?? propertyInfo.Name;
				var value = propertyInfo.GetValue(dto, null)?.ToString();
				if (value.IsNullOrEmpty())
				{
					throw new PlatformException($"{desc}不能为空", (int)ErrorCode.ErrorParameterFormat);
				}
				if (!string.IsNullOrEmpty(attr.Regex))
				{
					Regex reg = new Regex(attr.Regex);
					if (!reg.IsMatch(value))
					{
						throw new PlatformException(attr.RegexNotice.IsNullOrEmpty() ? $"请输入正确的{desc}" : attr.RegexNotice, (int)ErrorCode.ErrorParameterFormat);
					}
				}
				if (value != null && attr.MinLength > 0 && value.Length < attr.MinLength)
				{
					throw new PlatformException($"{desc}不能少于{attr.MinLength}位", (int)ErrorCode.ErrorParameterFormat);
				}
				if (value != null && attr.MaxLength > 0 && value.Length > attr.MaxLength)
				{
					throw new PlatformException($"{desc}不能超过{attr.MaxLength}位", (int)ErrorCode.ErrorParameterFormat);
				}
				if (attr.EnumType != null && attr.EnumType.IsEnum)
				{
					if (value == null || attr.EnumType.ToValueList().All(e => e != int.Parse(value.ToString())))
					{
						throw new PlatformException(attr.RegexNotice.IsNullOrEmpty() ? $"请输入正确的{desc}" : attr.RegexNotice, (int)ErrorCode.ErrorParameter);
					}
				}
			}
		}

		/// <summary>
		/// 保存文件列表
		/// </summary>
	    protected string SaveFiles()
		{
			var files = Request.Form.Files;
			if (files == null || files.Count <= 0)
			{
				return string.Empty;
			}
			List<string> fileNames = new List<string>();
			var len = files.Count();
			LogManager.GetLogger("FilesCount").Info(len);
			for (int i = 0; i < len; i++)
			{
				var file = files.Skip(i).Take(1).First();
				string fileName = file.FileName;
				var name = Guid.NewGuid().ToString("N") + "."+ fileName.Split('.').Last();
				var totalSize = file.Length; var readSize = 1024L;
				var bt = new byte[totalSize > readSize ? readSize : totalSize];
				var currentSize = 0L;
				var path = GetFilePackagePath(name);
				using (var stream = System.IO.File.Create(path))
				{
					//进度条处理流程
					using (var inputStream = file.OpenReadStream())
					{
						//读取上传文件流
						while (inputStream.Read(bt, 0, bt.Length) > 0)
						{
							//当前读取的长度
							currentSize += bt.Length;
							//写入上传流到服务器文件中
							stream.Write(bt, 0, bt.Length);
							//获取每次读取的大小
							readSize = currentSize + readSize <= totalSize ?readSize :totalSize - currentSize;
							//重新设置
							bt = new byte[readSize];
						}
					}
				}
				LogManager.GetLogger("FilesName").Info(name);
				fileNames.Add(name);
			}
			LogManager.GetLogger("FilesName").Info(fileNames.Join(","));
			return fileNames.Join(",");
		}

	    /// <summary>
	    /// 保存文件列表
	    /// </summary>
	    protected string SaveFiles(string path)
	    {
		    var files = Request.Form.Files;
		    if (files == null || files.Count <= 0)
		    {
			    return string.Empty;
		    }
		    List<string> fileNames = new List<string>();
		    var len = files.Count();
		    for (int i = 0; i < len; i++)
		    {
			    var file = files.Skip(i).Take(1).First();
			    string fileName = file.FileName;
			    var name = Guid.NewGuid().ToString("N") + "." + fileName.Split('.').Last();
			    var totalSize = file.Length; var readSize = 1024L;
			    var bt = new byte[totalSize > readSize ? readSize : totalSize];
			    var currentSize = 0L;
			    path = path??GetFilePackagePath(name);
			    using (var stream = System.IO.File.Create(path))
			    {
				    //进度条处理流程
				    using (var inputStream = file.OpenReadStream())
				    {
					    //读取上传文件流
					    while (inputStream.Read(bt, 0, bt.Length) > 0)
					    {
						    //当前读取的长度
						    currentSize += bt.Length;
						    //写入上传流到服务器文件中
						    stream.Write(bt, 0, bt.Length);
						    //获取每次读取的大小
						    readSize = currentSize + readSize <= totalSize ? readSize : totalSize - currentSize;
						    //重新设置
						    bt = new byte[readSize];
					    }
				    }
			    }
			    fileNames.Add(name);
		    }
		    return fileNames.Join(",");
	    }

		/// <summary>
		/// 获取绝对路径
		/// </summary>
		/// <returns></returns>
		protected string GetFilePackagePath(string name)
	    {
		    var relativePath = @"..\files";
		    var fullPath = Path.GetFullPath(relativePath);
			if (!Directory.Exists(fullPath) )//如果不存在就创建file文件夹
		    {
			    Directory.CreateDirectory(fullPath);
		    }
			return fullPath+"\\"+name;
	    }

		/// <summary>
		/// 获取头像
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		protected string GetHeadPic(long id)
	    {
		    var fullPath = Path.GetFullPath($@"..\files\headpic\{id}.jpg");
		    if (System.IO.File.Exists(fullPath))
		    {
			    FileInfo fi = new FileInfo(fullPath);
				return GetImageNetPath($"headpic/{id}.jpg?t={fi.LastWriteTime:yyyyMMddHHmmss}");
			}
		    return GetImageNetPath("headpic/0.jpg");
	    }

		/// <summary>
		/// 保存头像信息
		/// </summary>
		/// <param name="id"></param>
	    protected void SaveHeadPic(long id)
		{
			var relativePath = @"..\files\headpic";
			var fullPath = Path.GetFullPath(relativePath);
			if (!Directory.Exists(fullPath))//如果不存在就创建file文件夹
			{
				Directory.CreateDirectory(fullPath);
			}
			SaveFiles($"{fullPath}\\{id}.jpg");
		}


	    /// <summary>
		/// 获取网络路径
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
	    protected string GetFileNetPath(string name)
	    {
		    return $"{Settings.ApiHost}{name}";
	    }
	    /// <summary>
	    /// 获取网络路径
	    /// </summary>
	    /// <param name="name"></param>
	    /// <returns></returns>
	    protected string GetImageNetPath(string name)
	    {
		    return $"{Settings.ApiHost}{name}";
	    }

		/// <summary>
		/// 分页
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="result"></param>
	    public void PageRender<T>(QueryResult<T> result)
	    {
		    var total = result?.Total ?? 0;
		    var index = result?.Query?.Index ?? 1;
		    var size = result?.Query?.PageSize ?? 10;
		    var totalPage = total / size + (total % size == 0 ? 0 : 1);
		    var queryString = Request.QueryString.Value;
		    if (queryString.IndexOf("?", StringComparison.Ordinal)==0)
		    {
			    queryString = queryString.Substring(1, queryString.Length - 1);
		    }
			var queryStringDic = new Dictionary<string,object>();
			queryString.Split("&").ToList().ForEach(e =>
			{
				if (e.IsNotNullOrEmpty())
				{
					var items = e.Split("=");
					var key = items[0];
					var value = items[1];
					queryStringDic.Add(key, value);
				}
			});
		    if (queryStringDic.ContainsKey("PageIndex"))
		    {
			    queryStringDic.Remove("PageIndex");
			}
		    if (queryStringDic.ContainsKey("HasValue"))
			{
				queryStringDic.Remove("Value");
			}
		    if (queryStringDic.ContainsKey("HasValue"))
			{
				queryStringDic.Remove("HasValue");
			}
			queryStringDic.Add("PageIndex", index);
			var prePagePath = "javascript:void(0);";
		    if (index != 1)
		    {
			    queryStringDic["PageIndex"]= index - 1;
			    prePagePath = queryStringDic.Select(e => $"{e.Key}={e.Value}").Join("&");
			    prePagePath = $"{Request.Path}?{prePagePath}";
		    }
		    var prePage = $"<li class=\"prev  {(index == 1 ? "disabled":"")}\"><a href=\"{prePagePath}\">上一页</a></li>";
		    var rst = prePage;
			for (int i = index-2; i <= index+2; i++)
			{
				if (i <= 0 || i > totalPage)
				{
					continue;
				}
				queryStringDic["PageIndex"] = i;
				var path= queryStringDic.Select(e => $"{e.Key}={e.Value}").Join("&");
				var indexpath = $"<li><a href=\"{Request.Path}?{path}\">{i}</a></li>";
			    rst += indexpath;
		    }
		    var lastPagePath = "javascript:void(0);";
		    if (index != totalPage)
			{
				queryStringDic["PageIndex"] = index + 1;
				lastPagePath = queryStringDic.Select(e => $"{e.Key}={e.Value}").Join("&");
				lastPagePath = $"{Request.Path}?{lastPagePath}";
			}
			var lastPage = $"<li class=\"next  {(index == totalPage ? "disabled" : "")}\"><a href=\"{lastPagePath}\">下一页</a></li>";
		    rst += lastPage;
		    ViewBag.Page = rst;
	    }
	}
}
