// <auto-generated/>
#pragma warning disable
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using Wolverine.Http;

namespace Internal.Generated.WolverineHandlers
{
    // START: GET_name_name
    public class GET_name_name : Wolverine.Http.HttpHandler
    {
        private readonly Wolverine.Http.WolverineHttpOptions _options;

        public GET_name_name(Wolverine.Http.WolverineHttpOptions options) : base(options)
        {
            _options = options;
        }



        public override async System.Threading.Tasks.Task Handle(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            var name = (string)httpContext.GetRouteValue("name");
            // Just saying hello in the code! Also testing the usage of attributes to customize endpoints
            var result_of_SimpleStringRouteArgument = WolverineWebApi.TestEndpoints.SimpleStringRouteArgument(name);
            await WriteString(httpContext, result_of_SimpleStringRouteArgument);
        }

    }

    // END: GET_name_name
    
    
}
