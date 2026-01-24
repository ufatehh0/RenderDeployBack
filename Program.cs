using CodeDungeonAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 2. Swagger-ə JWT Dəstəyinin Əlavə Edilməsi (Authorize düyməsi üçün)
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "CodeDungeon API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Tokeninizi daxil edin (Misal: 12345abcdef)"
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

// 3. JWT Tənzimləməsi
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            // QEYD: Key ən az 32 simvol olmalıdır (HmacSha256 üçün)
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("bu_cox_gizli_ve_uzun_bir_key_olmalidir_123!")),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// 4. Swagger-i Render-də (Production-da) görmək üçün şərtdən çıxarırıq
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CodeDungeon API v1");
    c.RoutePrefix = "swagger"; // API linkinin sonuna /swagger yazanda açılsın
});

// 5. HTTPS Yönləndirməsini yalnız Development-də saxlayırıq (Render logundakı xəta üçün)
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();