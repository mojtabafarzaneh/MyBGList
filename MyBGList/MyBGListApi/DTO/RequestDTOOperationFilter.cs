using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MyBGListApi.DTO;
using Swashbuckle.AspNetCore.SwaggerGen;

public class RequestDTOOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var requestDtoParameter = context.ApiDescription.ParameterDescriptions
            .FirstOrDefault(p => p.Type.IsGenericType && p.Type.GetGenericTypeDefinition() == typeof(RequestDTO<>));

        if (requestDtoParameter != null && operation.Parameters != null)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "PageIndex",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema { Type = "integer", Default = new OpenApiInteger(0) },
                Description = "The index of the page to retrieve, starting from 0."
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "PageSize",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema { Type = "integer", Default = new OpenApiInteger(10), Minimum = 1, Maximum = 25 },
                Description = "The number of items per page, ranging from 1 to 25."
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "SortColumn",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema { Type = "string", Default = new OpenApiString("Name") },
                Description = "The column to sort the results by. Defaults to 'Name'."
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "SortOrder",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema { Type = "string", Default = new OpenApiString("ASC") },
                Description = "The order of sorting: ASC for ascending, DESC for descending."
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "FilterQuery",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema { Type = "string" },
                Description = "Optional filter query for searching items."
            });
        }
    }
}
