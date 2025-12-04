namespace InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;

public class DistribucionCategoriaDto
{
    public string NombreCategoria { get; set; } = string.Empty;
    public int CantidadProductos { get; set; }
    public decimal ValorTotal { get; set; }
}