using BackCoding.Challenge.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BackCoding.Challenge.Infrastructure.Extensions
{
    public static class DatabaseSeedExtensions
    {
        public static void SeedBaseData(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BackCodingDbContext>();

            try
            {
                // Verificar si ya existen datos
                var hasData = db.Clients.Any(); // Cambiado a Clients

                if (hasData)
                {
                    Log.Information("Datos base ya existen, no se insertan duplicados.");
                    return;
                }

                Log.Information("Insertando datos base iniciales...");

                var sql = @"
                    INSERT INTO cliente (nombre, apellidos, ciudad, balance, telefono) VALUES
                    ('Roberth', 'Ortiz', 'Bogota', 500000, '3108889999'),
                    ('Camila', 'Ramirez', 'Medellin', 500000, '3112223344'),
                    ('Daniel', 'Garcia', 'Cali', 500000, '3125556677');

                    -- Productos
                    INSERT INTO producto (nombre, tipo_producto, min_amount) VALUES
                        ('FPV_BTG_PACTUAL_RECAUDADORA', 'FPV', 75000),
                        ('FPV_BTG_PACTUAL_ECOPETROL', 'FPV', 125000),
                        ('DEUDAPRIVADA', 'FIC', 50000);

                    -- Sucursales
                    INSERT INTO sucursal (nombre, ciudad) VALUES
                        ('Sucursal Bogota', 'Bogota'),
                        ('Sucursal Medellin', 'Medellin'),
                        ('Sucursal Cali', 'Cali');

                    -- Disponibilidad
                    INSERT INTO disponibilidad (id_producto, id_sucursal) VALUES
                        (1, 1),
                        (2, 2),
                        (3, 3);

                    -- Visitas
                    INSERT INTO visitan (id_cliente, id_sucursal, fecha_visita) VALUES
                        (1, 1, NOW()),
                        (2, 2, NOW()),
                        (3, 3, NOW());

                    -- Inscripciones
                    INSERT INTO inscripcion (id_cliente, id_producto) VALUES
                        (1, 1),
                        (2, 2),
                        (3, 3);
                ";

                db.Database.ExecuteSqlRaw(sql);
                Log.Information("Datos base insertados correctamente.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al insertar datos base iniciales: {Message}", ex.Message);
            }
        }
    }
}
