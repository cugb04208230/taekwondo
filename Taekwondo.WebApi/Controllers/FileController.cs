using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Common.Data;
using Common.Di;
using Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taekwondo.Data;
using Taekwondo.WebApi.Filters;

namespace Taekwondo.WebApi.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// 文件管理
	/// </summary>
	[Route("api/[controller]/[action]")]
	public class FileController:BaseController
    {
		/// <summary>
		/// 上传文件返回系统文件标识
		/// </summary>
		/// <returns></returns>
		[HttpPost]
	    public IActionResult Upload()
		{
			var files = SaveFiles();
			return this.Success(files);
		}

		/// <summary>
		/// 获取文件
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
	    [HttpGet]
		[Route("{file}")]
	    public IActionResult Mp4(string file)
		{
			FileStream fileStream = new FileStream(GetFilePackagePath(file),FileMode.Open);
		    return File(fileStream, "video/mpeg4",file);
	    }


	    /// <summary>
	    /// 获取文件
	    /// </summary>
	    /// <param name="file"></param>
	    /// <returns></returns>
	    [HttpGet]
	    [Route("{file}")]
	    public IActionResult Image(string file)
	    {
		    var path = GetImageNetPath(file);
		    return File(GetPicStream(path), "image/jpeg");
	    }


	    private Stream GetPicStream(string path)
	    {
		    try
		    {

			    WebRequest request = WebRequest.Create(path);
			    WebResponse response = request.GetResponse();
			    Stream reader = response.GetResponseStream();
			    return reader;
		    }
		    catch (Exception e)
		    {
			    WebRequest request = WebRequest.Create(GetImageNetPath("headpic.jpg"));
			    WebResponse response = request.GetResponse();
			    Stream reader = response.GetResponseStream();
			    return reader;
		    }
	    }

		/// <summary>
		/// 数据库模型
		/// </summary>
		/// <returns></returns>
		[HttpGet]
	    public IActionResult DbDescription()
		{
			var sqlCloumn =
				"EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{0}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{1}', @level2type=N'COLUMN',@level2name=N'{2}'";
			var sqlTable =
				"EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{0}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{1}'";
			var dbContext = ObjectContainer.Instance.Resolver<DataBaseContext>();
			XDocument xml = XDocument.Load("D:\\DoWork\\SVN\\Taekwondo\\API\\Taekwondo.WebApi\\Taekwondo.Data.xml");
			var elements = xml.Elements("doc").Elements("members").Elements("member").Where(e=>e.FirstAttribute.Value.Contains(":Taekwondo.Data.Entities.")&&!e.FirstAttribute.Value.StartsWith("M")&& !e.FirstAttribute.Value.EndsWith("Repository") && !e.FirstAttribute.Value.EndsWith("Query")).AsEnumerable().ToList();
			var tables = new List<TableDesc>();
			var table = new TableDesc();
			elements.ForEach(element =>
			{
				var xElement = element.Element("summary");
				if (element.FirstAttribute.Value.StartsWith("T"))
				{
					if (table.Desc.IsNotNullOrEmpty())
					{
						tables.Add(table);
					}
					table = new TableDesc
					{
						TableName = element.FirstAttribute.Value.Split(".").Last(),
						Desc = xElement.Value.Trim(),
						Cloumns = new List<CloumnDesc>()
					};
				}
				else
				{
					var cloumn = new CloumnDesc
					{
						CloumnName = element.FirstAttribute.Value.Split(".").Last(),
						Desc = xElement.Value.Trim()
					};
					table.Cloumns.Add(cloumn);
				}
			});
			var list = new List<string>();
			tables.ForEach(item =>
			{
				try
				{
					var sqlT = sqlTable.Formats(item.Desc, ToPlural(item.TableName));
					dbContext.Database.ExecuteSqlCommand(sqlT);
				}
				catch (Exception e)
				{
				}
				item.Cloumns.ForEach(cl =>
				{
					try
					{

						var sqlC = sqlCloumn.Formats(cl.Desc, ToPlural(item.TableName), cl.CloumnName);
						dbContext.Database.ExecuteSqlCommand(sqlC);
					}
					catch (Exception e)
					{
					}
				});
			});
			return this.Success("");
		}
	    /// <summary>
	    /// 单词变成单数形式
	    /// </summary>
	    /// <param name="word"></param>
	    /// <returns></returns>
	    private string ToSingular(string word)
	    {
		    Regex plural1 = new Regex("(?<keep>[^aeiou])ies$");
		    Regex plural2 = new Regex("(?<keep>[aeiou]y)s$");
		    Regex plural3 = new Regex("(?<keep>[sxzh])es$");
		    Regex plural4 = new Regex("(?<keep>[^sxzhyu])s$");

		    if (plural1.IsMatch(word))
			    return plural1.Replace(word, "${keep}y");
		    else if (plural2.IsMatch(word))
			    return plural2.Replace(word, "${keep}");
		    else if (plural3.IsMatch(word))
			    return plural3.Replace(word, "${keep}");
		    else if (plural4.IsMatch(word))
			    return plural4.Replace(word, "${keep}");

		    return word;
	    }
	    /// <summary>
	    /// 单词变成复数形式
	    /// </summary>
	    /// <param name="word"></param>
	    /// <returns></returns>
	    private string ToPlural(string word)
	    {
		    Regex plural1 = new Regex("(?<keep>[^aeiou])y$");
		    Regex plural2 = new Regex("(?<keep>[aeiou]y)$");
		    Regex plural3 = new Regex("(?<keep>[sxzh])$");
		    Regex plural4 = new Regex("(?<keep>[^sxzhy])$");

		    if (plural1.IsMatch(word))
			    return plural1.Replace(word, "${keep}ies");
		    else if (plural2.IsMatch(word))
			    return plural2.Replace(word, "${keep}s");
		    else if (plural3.IsMatch(word))
			    return plural3.Replace(word, "${keep}es");
		    else if (plural4.IsMatch(word))
			    return plural4.Replace(word, "${keep}s");

		    return word;
	    }
	}

	public class TableDesc
	{
		public string TableName { set; get; }

		public string Desc { set; get; }

		public List<CloumnDesc> Cloumns { set; get; }
	}

	public class CloumnDesc
	{
		public string CloumnName { set; get; }

		public string Desc { set; get; }

	}
}
