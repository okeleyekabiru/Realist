using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using DeviceDetectorNET.Results.Device;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Plugins;
using Plugins.Cloudinary;
using Plugins.DeviceAuthentication;
using Plugins.JwtHandler;
using Plugins.Mail;
using Plugins.Redis.Cache;
using Plugins.Youtube;
using Realist.Api.SignalR;
using Realist.Data.Infrastructure;
using Realist.Data.Model;
using Realist.Data.Repo;
using Realist.Data.Services;
using Realist.Data.ViewModels;

namespace Realist.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            // if any problem occur with autoMapper check here
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper(typeof(User),typeof(UserReturnModel),typeof(Post),typeof(Comment));
            services.AddScoped<IPost, PostRepo>();
            services.AddScoped<IComment, CommentRepo>();
            services.AddScoped<IUserInfo, UserInfoRepo>();
            services.AddScoped<IBot, BotRepo>();
            services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);


            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("RealistConnection"));

            });
            services.AddScoped<IReply,ReplyRepo>();
            services.AddSignalR();
            services.AddScoped<IDeviceAuth, DeviceAuthentication>();
            services.AddTransient<IMailService, EmailService>();
            services.AddScoped<IUser, UserRepo>();
            services.AddScoped<IJwtSecurity, JwtGenrator>();
            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireNonAlphanumeric = true;


            }).AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders(); ;
            services.Configure<CloudinarySettings>(Configuration.GetSection("Cloudinary"));
            services.AddScoped<IPhoto, PhotoRepo>();
            services.AddScoped<IYoutube, Youtube>();
            services.AddScoped<IVideo, VideoRepo>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            services.AddScoped<IRedis, Redis>();
            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = "localhost:6379";

            });
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("jwtHandler").Value));
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };


                });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                // app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseRouting();
                app.UseAuthorization();
                app.UseDefaultFiles();
                app.UseStaticFiles();
                app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                        endpoints.MapHub<CommentHub>("/comment");

                    }
               
                );
          

            }

        }
    }
