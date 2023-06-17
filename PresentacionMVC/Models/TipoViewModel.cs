
using System.ComponentModel.DataAnnotations;


namespace PresentacionMVC.Models
{
    public class TipoViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "No se ingreso un nombre para el tipo")]
        public string Nombre { get; set; }
       

        public string Descripcion { get; set; }

        public float Costo { get; set; }


    }
}
