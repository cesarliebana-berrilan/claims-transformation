using Berrilan.Claims.Core.Persistence;
using Berrilan.Claims.WebApi;
using Berrilan.Claims.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Berrilan.Claims.Core.Features.Me;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>((sp, options) => options
    .UseNpgsql(builder.Configuration.GetConnectionString("AppConnection"))
    .UseSnakeCaseNamingConvention());

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddExceptionHandlers();
builder.Services.AddCoreServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet($"/me", async ([FromServices] ISender sender) =>
{
    GetMeResponse response = await sender.Send(new GetMeQuery());
    return TypedResults.Ok(response);
})
.WithName("Me")
.Produces(StatusCodes.Status401Unauthorized)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status500InternalServerError);

await app.RunAsync();

