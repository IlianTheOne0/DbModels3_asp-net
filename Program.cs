using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages
(
    options =>
    {
        options.RootDirectory = "/";
        options.Conventions.AddFolderRouteModelConvention
        (
            "/",
            model =>
            {
                foreach (var selector in model.Selectors)
                {
                    var template = selector.AttributeRouteModel?.Template;
                    if (!string.IsNullOrEmpty(template))
                    {
                        template = template.Replace("Features/", "", StringComparison.OrdinalIgnoreCase);
                        template = template.Replace("/Pages", "", StringComparison.OrdinalIgnoreCase);
                        template = template.Replace("Pages", "", StringComparison.OrdinalIgnoreCase);

                        if (template.StartsWith("Pages/", StringComparison.OrdinalIgnoreCase))
                        {
                            template = template.Replace("Pages/", "", StringComparison.OrdinalIgnoreCase);
                        }

                        selector.AttributeRouteModel!.Template = template;
                    }
                }
            }
        );
    }
);

//builder.Services.AddRazorPages
//(
//    options =>
//    {
//        options.RootDirectory = "/";

//        options.Conventions.AddPageRoute("/Pages/Index", "");
//        options.Conventions.AddPageRoute("/Features/Clients/Pages/Index", "Index");
//        options.Conventions.AddPageRoute("/Features/Clients/Pages/Test", "Test");

//        options.Conventions.AddFolderRouteModelConvention
//        (
//            "/Features", model =>
//            {
//                foreach (var selector in model.Selectors)
//                {
//                    if (selector.AttributeRouteModel?.Template != null)
//                    {
//                        selector.AttributeRouteModel.Template = selector.AttributeRouteModel.Template
//                            .Replace("Features/", "")
//                            .Replace("/Pages", "");
//                    }
//                }
//            }
//        );
//    }
//);

builder.Services.AddDbContext<AppDbContext>
(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

//app.MapGet
//(
//    "/",
//    context =>
//    {
//        context.Response.Redirect("/Clients");
//        return Task.CompletedTask;
//    }
//);

app.Run();