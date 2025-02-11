using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TodoApi;
using Microsoft.OpenApi.Models;
using System.Text;
// using IdentityModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using System.IdentityModel.Tokens.Jwt;
using TodoApi.models;
using TodoApi.nuun;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<Application>(builder.Configuration.GetSection(nameof(Application)));

builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationHandler>();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Added as service
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
    new MySqlServerVersion(new Version(8, 0, 33)))
    .EnableSensitiveDataLogging());


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
        Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? string.Empty))
        };
    });

// הוספת שירות CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  // אפשר כל מקור
            .AllowAnyHeader()    // אפשר כל כותרת
            .AllowAnyMethod();   // אפשר כל מתודה (GET, POST וכו')
    });
});

builder.Services.AddControllers();

var app = builder.Build();

// שימוש ב-CORS
app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
});

if (builder.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
// app.MapControllers();

app.MapGet("/", () => "Hello World!");
app.MapGet("/todos", async (ToDoDbContext dbContext) =>
{
    var task = await dbContext.Items.ToListAsync();
    return Results.Ok(task);
});
app.MapPost("/todos", async (ToDoDbContext dbContext, [FromBody] CreateTaskModel createTaskModel) =>
{
    try
    {
        if (string.IsNullOrEmpty(createTaskModel.TaskName))
        {
            throw new ArgumentNullException(nameof(createTaskModel.TaskName), "Task name cannot be null or empty.");
        }
        var task = new Item(createTaskModel.TaskName);
        // הוספת משימה חדשה
        dbContext.Items.Add(task);
        await dbContext.SaveChangesAsync();
        return Results.Created($"/todos/{task.Name}", task);
    }
    catch (Exception ex)
    {
        return Results.Problem($"An error occurred: {ex.Message}. Inner Exception: {ex.InnerException?.Message}");
    }
});

app.MapPut("/todos/{id}", async (int id, ToDoDbContext dbContext, [FromBody] UpdateTaskModel updateModel) =>
{
    try
    {
        // חפש את המשימה לפי ה-ID
        var existingTask = await dbContext.Items.FindAsync(id);

        // אם לא נמצאה המשימה עם ה-ID הנתון, החזר שגיאה
        if (existingTask == null)
        {
            throw new Exception("The task with the given ID was not found.");
        }        // עדכון הערכים של המשימה
        existingTask.IsComplete = updateModel.IsComplete;

        // שמור את השינויים
        await dbContext.SaveChangesAsync();

        // החזר את המשימה המעודכנת בתגובה
        return Results.Ok(existingTask);
    }
    catch (Exception ex)
    {
        return Results.Problem($"An error occurred: {ex.Message}. Inner Exception: {ex.InnerException?.Message}");
    }
});
app.MapDelete("/todos/{id}", async (int id, ToDoDbContext dbContext) =>
{
    try
    {
        // חפש את המשימה לפי ה-ID
        var existingTask = await dbContext.Items.FindAsync(id);

        // אם לא נמצאה המשימה עם ה-ID הנתון, החזר שגיאה
        if (existingTask == null)
        {
            throw new Exception("The task with the given ID was not found.");
        }
        // מחיקת המשימה
        dbContext.Items.Remove(existingTask);

        // שמירת השינויים במסד הנתונים
        await dbContext.SaveChangesAsync();

        // החזר הודעה על הצלחה
        return Results.Ok($"Task with ID {id} has been deleted.");
    }
    catch (Exception ex)
    {
        return Results.Problem($"An error occurred: {ex.Message}. Inner Exception: {ex.InnerException?.Message}");
    }
});

app.Run();
