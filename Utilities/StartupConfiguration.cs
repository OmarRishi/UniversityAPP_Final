using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UniversityAPP.Dto;
using UniversityAPP.Mapping;

namespace UniversityAPP.Utilities
{
    public static class StartupConfiguration
    {
        public static void APPConfiguration(this IServiceCollection Service)
        {
            Service.AddSwaggerGen(setup =>
             {
                 // Include 'SecurityScheme' to use JWT Authentication
                 var jwtSecurityScheme = new OpenApiSecurityScheme
                 {
                     BearerFormat = "JWT",
                     Name = "JWT Authentication",
                     In = ParameterLocation.Header,
                     Type = SecuritySchemeType.Http,
                     Scheme = JwtBearerDefaults.AuthenticationScheme,
                     Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                     Reference = new OpenApiReference
                     {
                         Id = JwtBearerDefaults.AuthenticationScheme,
                         Type = ReferenceType.SecurityScheme
                     }
                 };

                 setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                 setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                    { jwtSecurityScheme, Array.Empty<string>() }
                 });

             });

            IMapper mapper = MyMapper.InitializeAutoMapper();

            Service.AddSingleton(mapper);
            //Service.AddSingleton<ILogger<LogMiddleware>, Logger<LogMiddleware>>();
            Service.AddLogging();

        }
        public static void AddAuthentication(this IServiceCollection Service, JwtData jwtData)
        {
            Service.AddAuthentication(option =>
            {
                // For Check Token Scheme
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // Redirect To Login Form 
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                // Validate token data 
                option.SaveToken = true;
                // Set Https Required
                option.RequireHttpsMetadata = true;
                // Set Valid Token Data To validate By it
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtData.ValidIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtData.ValidAudiance,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtData.SecretyKey ?? "")),
                };
            });
        }


    }
}
