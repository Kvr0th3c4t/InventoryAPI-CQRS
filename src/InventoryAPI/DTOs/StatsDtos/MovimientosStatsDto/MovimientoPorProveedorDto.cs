namespace InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;

public class MovimientoPorProveedorDto
{
    public string NombreProveedor { get; set; } = string.Empty;
    public int TotalMovimientos { get; set; }
}