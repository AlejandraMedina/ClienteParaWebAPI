
namespace PresentacionMVC.Models
{
    public class MantenimientoViewModel
    {

    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public string Descripcion { get; set; }


    public double Costo { get; set; }


    public string Funcionario { get; set; }

    public CabañaViewModel Cabania { get; set; }

    public int CabaniaId { get; set; }

    }

}
