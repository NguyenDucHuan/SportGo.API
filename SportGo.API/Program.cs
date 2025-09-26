using MailKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SportGo.API.MapperProfile;
using SportGo.Repository;
using SportGo.Repository.SMTPs.Models;
using SportGo.Repository.UnitOfWork;
using SportGo.Service.Services;
using SportGo.Service.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
// THÊM USING NÀY
using Microsoft.OpenApi.Models;

namespace SportGo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "SportGo API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Vui lòng nhập Access Token với tiền tố 'Bearer ' vào ô bên dưới.\n\nVí dụ: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            builder.Services.AddDbContext<SportGoDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Cấu hình các lớp Service và Repository
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddTransient<IMailSenderService, MailSenderService>();
            builder.Services.AddScoped(typeof(IUnitOfWork<SportGoDbContext>), typeof(UnitOfWork<SportGoDbContext>));

            // Cấu hình AutoMapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

            // Cần thiết để lấy thông tin request trong các service
            builder.Services.AddHttpContextAccessor();

            // Cấu hình xác thực JWT (Mã của bạn đã đúng)
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false; // Chỉ nên là false trong môi trường dev
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });


            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dbContext = services.GetRequiredService<SportGoDbContext>();
                    dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Đã xảy ra lỗi trong quá trình áp dụng CSDL migrations.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    // Hiển thị UI của Swagger
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SportGo API v1");
                });
            }

            app.UseHttpsRedirection();

            // QUAN TRỌNG: Thứ tự của các middleware này phải chính xác
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
