using Microsoft.EntityFrameworkCore;
using RecruitmentApp.Extensions;
using RecruitmentApp.Features.Contacts.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.ConfigureTokenManagement(builder.Configuration);
builder.Services.ConfigurePasswordPolicy(builder.Configuration);

builder.Services.ConfigureAuthentication(builder.Configuration);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var appDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    appDbContext.Database.Migrate();
    
    var authDbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    authDbContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
