using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Common.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Filters
{
    public class ApplySummariesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null) return;
            if (controllerActionDescriptor.Parameters.Count == 0)
            {
                return;
            }
            var dtoType = controllerActionDescriptor.Parameters[0].ParameterType;

            var dic = new Dictionary<string, string>();

            var propertyInfos = dtoType.GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.Name == "ResponseModel")
                {
                    var responseProperties = propertyInfo.PropertyType.GetProperties();
                    foreach (var responseProperty in responseProperties)
                    {
                        var respDesc = responseProperty.GetCustomAttribute<DescriptionAttribute>()?.Description;
                        if (respDesc == null)
                        {
                            continue;
                        }
                        dic.Add($"ResponseModel.{responseProperty.Name}", respDesc);
                    }
                    continue;
                }
                var desc = propertyInfo.GetCustomAttribute<DescriptionAttribute>()?.Description;
                if (desc == null)
                {
                    continue;
                }
                dic.Add(propertyInfo.Name, desc);
                
            }
            foreach (IParameter t in operation.Parameters)
            {
                string value;
                dic.TryGetValue(t.Name, out value);
                if (t.Description.IsNullOrEmpty()&&!value.IsNullOrEmpty())
                {
                    t.Description = value;
                }
            }
            var parameters = operation.Parameters.Where(e=>e.Name.Contains("ResponseModel")|| e.Name.Contains("QueryString")).ToList();
            foreach (var parameter in parameters)
            {
                operation.Parameters.Remove(parameter);
            }
            if (context.ApiDescription.ControllerAttributes().All(e => e.GetType().Name != "AuthAttribute"))
            {
                parameters = operation.Parameters.Where(e => e.Name.Contains("RobotID") || e.Name.Contains("Token"))
                    .ToList();
                foreach (var parameter in parameters)
                {
                    operation.Parameters.Remove(parameter);
                }
            }
        }
    }
}
