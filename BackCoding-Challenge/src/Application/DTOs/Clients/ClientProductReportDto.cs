
namespace BackCoding.Challenge.Application.DTOs.Clients
{
    public class ClientProductReportDto
    {
        public int cliente_id { get; set; }
        public string nombre_cliente { get; set; } = string.Empty;
        public int producto_id { get; set; }
        public string nombre_producto { get; set; } = string.Empty;
        public int sucursal_id { get; set; }
        public string nombre_sucursal { get; set; } = string.Empty;
        public string ciudad_sucursal { get; set; } = string.Empty;
    }
}
