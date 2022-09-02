using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Security.Claims;

var myConfig = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//Add AuthN bits...

builder.Services.AddMicrosoftIdentityWebAppAuthentication(myConfig,"AzureAd");

builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}
).AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GivenName", policy =>
    {
        policy.RequireClaim("given_name");
    });
}
);


/* ClaimToken myClaim = new ClaimToken();

ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
if (principal != null)
{
    ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
    if (identity != null)
    {
        foreach (Claim claim in identity.Claims)
        {
            myClaim.claim_type = claim.Type;
            myClaim.claim_value = claim.Value;
            //_logger.LogInformation($"{claim.Type} = {claim.Value}");
        }
    }
} */

// ...end authN bits

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

