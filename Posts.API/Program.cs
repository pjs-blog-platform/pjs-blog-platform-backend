
using Microsoft.EntityFrameworkCore;
using Posts.API.Database;
using Posts.API.Services;

namespace Posts.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DI
            builder.Services.AddTransient<IPostService, PostService>();
            builder.Services.AddSingleton<IPostMapper, PostMapper>();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<PostsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("PostsDatabase")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
