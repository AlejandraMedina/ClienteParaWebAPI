using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentacionMVC.Models;

namespace PresentacionMVC.Controllers
{

    public class UsuarioController : Controller
    {


        public IConfiguration Conf { get; set; }


        public UsuarioController(IConfiguration conf)
        {
            Conf = conf;          
        }


        // GET: UsuarioController
        [HttpGet]
        public ActionResult Login()
        {            
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UsuarioViewModel vm)
        {
            

                String url = Conf.GetValue<string>("ApiUsuarios");
                HttpClient client = new HttpClient();




            var tarea = client.PostAsJsonAsync(url,vm);
                tarea.Wait();

                var tarea2 = tarea.Result.Content.ReadAsStringAsync();
                tarea2.Wait();

                string body = tarea2.Result;

                if (tarea.Result.IsSuccessStatusCode)                    
                {
                    HttpContext.Session.SetString("token", body);
                    HttpContext.Session.SetString("usuarioLogueado", "si");
                return RedirectToAction("Index", "Tipo");
                }
                else 
                {
                    ViewBag.Mensaje = body;
                    return View(vm);
                }             
         
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.SetString("usuarioLogueado", "no");
            return RedirectToAction("login");
        }



        private string LeerContenido(HttpResponseMessage respuesta)
        {
            HttpContent contenido = respuesta.Content;
            Task<string> tarea2 = contenido.ReadAsStringAsync();
            tarea2.Wait();
            ViewBag.Mensaje = tarea2.Result;
            return tarea2.Result;
        }

    }
}
