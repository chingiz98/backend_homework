using System.Text;
using BackendHomework.BusinessLogic.Accounts;
using BackendHomework.BusinessLogic.Auth;
using BackendHomework.BusinessLogic.User;
using BackendHomework.Commands;
using BackendHomework.Consumers;
using BackendHomework.Models;
using BackendHomework.Services;
using BackendHomework.Services.Interfaces;
using BackendHomework.TokenApp;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace BackendHomework
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountsService, AccountsService>();

            services.AddScoped<SignUpRequestHandler>();
            services.AddScoped<LogInRequestHandler>();

            services.AddScoped<CreateAccountRequestHandler>();
            services.AddScoped<CloseAccountRequestHandler>();
            services.AddScoped<MakeDepositRequestHandler>();
            services.AddScoped<GetAccountsRequestHandler>();
            services.AddScoped<MakeTransactionRequestHandler>();
            services.AddScoped<RenameAccountRequestHandler>();
            services.AddScoped<GetTransactionsRequestHandler>();

            services.AddScoped<ProfileRequestsHandler>();
            services.AddScoped<UpdateUserInfoRequestHandler>();
            
            services.AddScoped<MakeDepositConsumer>();
            services.AddScoped<RenameAccountConsumer>();
            services.AddScoped<UpdateUserInfoConsumer>();
            
            services.AddMassTransit(x =>
            {
                x.AddConsumer<MakeDepositConsumer>();
                x.AddConsumer<RenameAccountConsumer>();
                x.AddConsumer<UpdateUserInfoConsumer>();
                x.AddBus(provider => MassTransit.Bus.Factory.CreateUsingInMemory(cfg =>
                {
                    cfg.ReceiveEndpoint("deposit-queue", ep =>
                    {
                        ep.ConfigureConsumer<MakeDepositConsumer>(provider);
                        EndpointConvention.Map<MakeDepositCommand>(ep.InputAddress);
                    });
                    cfg.ReceiveEndpoint("account-rename-queue", ep =>
                    {
                        ep.ConfigureConsumer<RenameAccountConsumer>(provider);
                        EndpointConvention.Map<RenameAccountCommand>(ep.InputAddress);
                    });
                    cfg.ReceiveEndpoint("update-user-info-queue", ep =>
                    {
                        ep.ConfigureConsumer<UpdateUserInfoConsumer>(provider);
                        EndpointConvention.Map<UpdateUserInfoCommand>(ep.InputAddress);
                    });
                }));
                x.AddRequestClient<MakeDepositCommand>();
                x.AddRequestClient<RenameAccountCommand>();
                x.AddRequestClient<UpdateUserInfoCommand>();
            });
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });
            
            services.AddSingleton<IHostedService, BusService>();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseAuthentication();
            //перехват исключений и отправка ошибки клиенту
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        var ex = error.Error;
                        await context.Response.WriteAsync(new ErrorDto()
                        {
                            Code = -1,
                            Message = ex.Message
                        }.ToString(), Encoding.UTF8);
                    }
                });
            });
            
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}