namespace InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;

public class MovimientoPorDiaDto
{
    public DateTimeOffset Fecha { get; set; }
    public int TotalMovimientos { get; set; }
}