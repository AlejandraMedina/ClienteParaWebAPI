using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using PresentacionMVC.Models;
using Microsoft.CodeAnalysis.Host;



namespace PresentacionMVC.Controllers
{

    public class TipoController : Controller
    {

       

      
        public TipoController()
        { 
              

        }


        // GET: TipoController
        public ActionResult Index()        {


            return View();
        }

        // GET: TipoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TipoController/Create
        public ActionResult CreateTipo()
        {
            return View();
        }

        // POST: TipoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTipo(TipoViewModel t)
        {
            try
            {
                //t.Validar();
               // AltaTipo.Alta(t);
                
                return RedirectToAction(nameof(Index));
            }
          
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Oops! Ocurrió un error inesperado";
                return View();
            }
        }

        // GET: TipoController/Edit/5
        public ActionResult EditTipo(int id)
        {               
            
            return View();
        }

        // POST: TipoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTipo(int id, TipoViewModel t)
        {
            try
            {
               //No implementada no se pide
               
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TipoController/Delete/5
        public ActionResult DeleteTipo(int id)
        {
            return View();
        }

        // POST: TipoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTipo(int id, IFormCollection collection)
        {
           
            try
            {
               
                ViewBag.Mensaje = "El tipo fue eliminado con éxito";
                return RedirectToAction(nameof(Index));

            }         
             catch (Exception ex)
            {
                ViewBag.Mensaje = "No es posible eliminar el tipo ya que tiene cabañas asociadas";

                return View();
            }
          
            
            
        }


        // GET: TipoController/Edit/5
        public ActionResult BuscarTipoPorNombre()
        {

            return View();
        }


        // POST: TipoController/
      //  [HttpPost]
       // [ValidateAntiForgeryToken]
       // public ActionResult BuscarTipoPorNombre(string nombre)
        //{

        //    try
        //    {

        //       //Tipo tipo = RepoTipo.BuscarTipoPorNombre(nombre);

        //        return View(tipo);
        //    }
        //    catch
        //    {
        //        ViewBag.Mensaje = "No existe un tipo con el nombre ingresado";
        //        return View();
        //    }
           
        //}
       
    }
}
