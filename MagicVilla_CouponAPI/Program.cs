using MagicVilla_CouponAPI.Data;
using MagicVilla_CouponAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/helloworld/{id:int}", (int id) =>
{
    return Results.Ok("id!!!!"+id);
});
app.MapPost("/helloworld2", () => Results.Ok("Hello World2"));


app.MapGet("/api/coupon", () => {
     return Results.Ok(CouponStore.couponList);
    }).WithName("GetCoupons");

app.MapGet("/api/coupon{id:int}", (int id) => {
    return Results.Ok(CouponStore.couponList.FirstOrDefault(u => u.Id==id));
}).WithName("GetCoupon");

app.MapPost("/api/coupon", ([FromBody] Coupon coupon) => {
    if (coupon.Id != 0 || string.IsNullOrEmpty(coupon.Name)) 
    {
        return Results.BadRequest("Invalid ID or Coupon Name");
    }

    if (CouponStore.couponList.FirstOrDefault(u=>u.Name.ToLower()== coupon.Name.ToLower()) != null)
    {
        return Results.BadRequest("Coupon Name Alreadt Exisits");
    }   

     coupon.Id = CouponStore.couponList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
    CouponStore.couponList.Add(coupon);
    // return Results.Ok(coupon);
    //return Results.Created($"/api/coupon/{coupon.Id}", coupon);
    return Results.CreatedAtRoute("GetCoupon", new { id = coupon.Id } ,coupon);
}).WithName("CreateCoupon");

app.MapPut("/api/coupon", () =>
{

});

app.MapDelete("/api/coupon{id:int}", (int id) =>
{

});


app.UseHttpsRedirection();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
