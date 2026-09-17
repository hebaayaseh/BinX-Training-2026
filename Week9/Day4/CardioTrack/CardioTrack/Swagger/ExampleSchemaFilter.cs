using CardioTrack.DTOs.LabRequest;
using CardioTrack.DTOs.LogIn;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CardioTrack.Swagger
{
    /// <summary>
    /// Attaches full request/response examples to the OpenAPI schema for the
    /// endpoints a newcomer tries first.
    /// </summary>
    public class ExampleSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(LoginRequestDto))
            {
                schema.Example = new OpenApiObject
                {
                    ["email"] = new OpenApiString("doctor@cardiotrack.com"),
                    ["password"] = new OpenApiString("Doctor@2026")
                };
            }
            else if (context.Type == typeof(LoginResponseDto))
            {
                schema.Example = new OpenApiObject
                {
                    ["accessToken"] = new OpenApiString("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."),
                    ["refreshToken"] = new OpenApiString("q8Fh2k...base64...==")
                };
            }
            else if (context.Type == typeof(CreateLabRequestDto))
            {
                schema.Example = new OpenApiObject
                {
                    ["patientId"] = new OpenApiInteger(12),
                    ["appointmentId"] = new OpenApiInteger(45),
                    ["testNames"] = new OpenApiArray
                    {
                        new OpenApiString("CBC"),
                        new OpenApiString("Lipid Panel"),
                        new OpenApiString("Troponin I")
                    }
                };
            }
            else if (context.Type == typeof(LabRequestResponseDto))
            {
                schema.Example = new OpenApiObject
                {
                    ["id"] = new OpenApiInteger(101),
                    ["patientId"] = new OpenApiInteger(12),
                    ["patientName"] = new OpenApiString("Ahmad Nasser"),
                    ["requestedByDoctorId"] = new OpenApiInteger(3),
                    ["doctorName"] = new OpenApiString("Dr. Sara Khalil"),
                    ["appointmentId"] = new OpenApiInteger(45),
                    ["testName"] = new OpenApiString("CBC"),
                    ["status"] = new OpenApiString("Pending"),
                    ["requstedAt"] = new OpenApiString("2026-09-16T09:30:00Z")
                };
            }
        }
    }
}