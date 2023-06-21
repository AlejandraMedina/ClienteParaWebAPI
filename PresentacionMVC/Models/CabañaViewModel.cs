
namespace PresentacionMVC.Models
{
    public class CabañaViewModel
    {

        public int Id { get; set; }

       
        public string Nombre { get; set; }

        public string Descripcion { get; set; }


        public int NumHabitacion { get; set; }

        public int TipoId { get; set; }

        public TipoViewModel Tipo { get; set; }


        public bool Jacuzzi { get; set; }

        public bool Habilitada { get; set; }

        public int PersonasMax { get; set; }
        public IFormFile Foto { get; set; }



    }
}
