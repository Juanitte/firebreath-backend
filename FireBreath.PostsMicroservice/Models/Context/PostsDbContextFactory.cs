using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FireBreath.PostsMicroservice.Models.Context
{
    public class PostsDbContextFactory : IDesignTimeDbContextFactory<PostsDbContext>
    {
        public PostsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostsDbContext>();

            // Usa aquí tu cadena de conexión real (mejor si la sacas de un archivo temporal .env local o de user-secrets)
            optionsBuilder.UseSqlServer("Server=sql-users,1433;Database=FireBreath_Users;User Id=sa;Password=Batraci0C0nPeluca;TrustServerCertificate=true;MultipleActiveResultSets=true;");

            return new PostsDbContext(optionsBuilder.Options);
        }
    }
}