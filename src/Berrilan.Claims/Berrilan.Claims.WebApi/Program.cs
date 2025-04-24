using Berrilan.Claims.WebApi;
using Berrilan.Claims.Core;
using Microsoft.AspNetCore.Mvc;
using Berrilan.Claims.Core.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddJsonTransforms();
builder.Services.AddDatabase(builder.Configuration.GetConnectionString("AppConnection"));
builder.Services.AddSecurityLayer();
builder.Services.AddExceptionHandlers();
builder.Services.AddCoreServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IUserContext, UserContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet($"/me", async (HttpContext ctx) =>
{
    IUserContext userContext = ctx.RequestServices.GetRequiredService<IUserContext>();
    string token = ctx.Request.Headers.Authorization.ToString().Substring(7);
    UserInfo userInfo = await userContext.GetUserInfo(token)
        ?? throw new CredentialNotValidException($"User not authorized");
       
    return TypedResults.Ok(userInfo);
})
.WithName("Me")
.RequireAuthorization()
.Produces(StatusCodes.Status401Unauthorized)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status500InternalServerError);

await app.RunAsync();

