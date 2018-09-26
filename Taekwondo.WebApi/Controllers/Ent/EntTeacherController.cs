using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Taekwondo.WebApi.Filters;

namespace Taekwondo.WebApi.Controllers.Ent
{
	/// <inheritdoc />
	/// <summary>
	/// 课程
	/// </summary>
	[Auth]
	[Route("api/ent/teacher/[action]")]
	public class EntTeacherController:BaseController
    {
    }
}
