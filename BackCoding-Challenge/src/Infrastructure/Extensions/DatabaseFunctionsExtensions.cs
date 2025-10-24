using BackCoding.Challenge.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Text;

namespace BackCoding.Challenge.Infrastructure.Extensions
{
    public static class DatabaseFunctionsExtensions
    {
        public static void EnsureFunctionsCreated(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BackCodingDbContext>();

            var sql = new StringBuilder();

            sql.AppendLine("CREATE EXTENSION IF NOT EXISTS \"pgcrypto\";");

            sql.AppendLine(@"
            CREATE OR REPLACE FUNCTION fn_clientes_productos_sucursales()
            RETURNS TABLE (
                cliente_id INT,
                nombre_cliente VARCHAR(200),
                producto_id INT,
                nombre_producto VARCHAR(200),
                sucursal_id INT,
                nombre_sucursal VARCHAR(200),
                ciudad_sucursal VARCHAR(200)
            )
            AS $$
            BEGIN
                RETURN QUERY
                SELECT 
                    c.id_cliente,
                    CONCAT(c.nombre, ' ', c.apellidos)::VARCHAR(200) AS nombre_cliente,
                    p.id_producto,
                    p.nombre::VARCHAR(200) AS nombre_producto,
                    s.id_sucursal,
                    s.nombre::VARCHAR(200) AS nombre_sucursal,
                    s.ciudad::VARCHAR(200) AS ciudad_sucursal
                FROM cliente c
                JOIN inscripcion i ON c.id_cliente = i.id_cliente
                JOIN producto p ON p.id_producto = i.id_producto
                JOIN disponibilidad d ON d.id_producto = p.id_producto
                JOIN visitan v ON v.id_cliente = c.id_cliente AND v.id_sucursal = d.id_sucursal
                JOIN sucursal s ON s.id_sucursal = d.id_sucursal
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM disponibilidad d2
                    WHERE d2.id_producto = p.id_producto
                    AND d2.id_sucursal NOT IN (
                        SELECT v2.id_sucursal FROM visitan v2 WHERE v2.id_cliente = c.id_cliente
                    )
                );
            END;
            $$ LANGUAGE plpgsql;");

            try
            {
                db.Database.ExecuteSqlRaw(sql.ToString());
                Log.Information("Función fn_clientes_productos_sucursales creada o actualizada correctamente.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creando la función fn_clientes_productos_sucursales.");
            }
        }
    }
}
