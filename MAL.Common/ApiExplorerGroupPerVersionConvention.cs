using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace MAL.Common
{
    public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNamespace = controller.ControllerType.Namespace;
            var apiVersion = controllerNamespace.Split('.').Last().ToLower();

            controller.ApiExplorer.GroupName = apiVersion;
        }
    }
}
