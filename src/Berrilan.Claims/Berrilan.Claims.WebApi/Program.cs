using Berrilan.Claims.WebApi;
using Berrilan.Claims.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Berrilan.Claims.Core.Features.Me;

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

app.MapGet($"/me", async ([FromServices] ISender sender) =>
{
    GetMeResponse response = await sender.Send(new GetMeQuery());
    return TypedResults.Ok(response);
})
.WithName("Me")
.RequireAuthorization()
.Produces(StatusCodes.Status401Unauthorized)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status500InternalServerError);

await app.RunAsync();

