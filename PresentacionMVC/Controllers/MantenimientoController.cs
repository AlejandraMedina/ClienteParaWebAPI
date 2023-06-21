using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PresentacionMVC.Models;
using System.Net.Http.Headers;
using System.Runtime.Intrinsics.X86;

namespace PresentacionMVC.Controllers
{
    public class MantenimientoController : Controller
    {
        public IConfiguration Conf { get; set; }

        public string URLBaseApiMantenimientos { get; set; }

        public MantenimientoController(IConfiguration conf)
        {

            Conf = conf;
            URLBaseApiMantenimientos = Conf.GetValue<String>("ApiMantenimiento");
        }
        // GET: MantenimientoController
        public ActionResult Index()
        {

            if (HttpContext.Session.GetString("token") == null) RedirectToAction("Login", "Usuarios");

            HttpClient cliente = new HttpClient();

            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            Task<HttpResponseMessage> tarea1 = cliente.GetAsync(URLBaseApiMantenimientos);
            tarea1.Wait();

            HttpResponseMessage respuesta = tarea1.Result;

            String cuerpo = LeerContenido(respuesta);

            return View();

         }


        // GET: MantenimientoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MantenimientoController/Create
        public ActionResult CreateMantenimiento()
        {
            if (HttpContext.Session.GetString("token") == null) return RedirectToAction("Login", "Usuarios");

            return View();
        }

        // POST: MantenimientoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMantenimiento(MantenimientoViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpClient cliente = new HttpClient();

                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

                    var tarea = cliente.PostAsJsonAsync(URLBaseApiMantenimientos, vm);
                    tarea.Wait();

                    if (tarea.Result.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var tarea2 = tarea.Result.Content.ReadAsStringAsync();
                        tarea2.Wait();
                        ViewBag.Mesaje = tarea2.Result;
                    }

                }
                else
                {
                    ViewBag.Mensaje = "Los datos ingresados no son válidos";
                }
                return RedirectToAction(nameof(Index));
            }

            catch (Exception ex)
            {
                ViewBag.Mensaje = "Ocurrió un error inesperado.";
                return View();
            }
        }

        // GET: MantenimientoController/Edit/5
        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("token") == null) return RedirectToAction("Login", "Usuarios");
            return View();
        }

        // POST: MantenimientoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, MantenimientoViewModel mant)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(mant);
            }
        }

        // GET: MantenimientoController/Delete/5
        public ActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("token") == null) return RedirectToAction("Login", "Usuarios");
            return View();
        }

        // POST: MantenimientoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, MantenimientoViewModel mant)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(mant);
            }
        }

        private string LeerContenido(HttpResponseMessage respuesta)
        {
            HttpContent contenido = respuesta.Content;
            Task<string> tarea2 = contenido.ReadAsStringAsync();
            tarea2.Wait();
            ViewBag.Mensaje = tarea2.Result;
            return tarea2.Result;
        }

        // GET: MantenimientoController
        public ActionResult ListarMantenimientosDeCabaña(int Id)
        {
            if (HttpContext.Session.GetString("token") == null) return RedirectToAction("Login", "Usuarios");


            HttpClient cliente = new HttpClient();

            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            Task<HttpResponseMessage> tarea1 = cliente.GetAsync(URLBaseApiMantenimientos);
            tarea1.Wait();

            HttpResponseMessage respuesta = tarea1.Result;

            String cuerpo = LeerContenido(respuesta);

            if (respuesta.IsSuccessStatusCode)  // Es un status 200 indica todo se ejecutó correctamente.
            {

                List<MantenimientoViewModel> mantenimientos = JsonConvert.DeserializeObject<List<MantenimientoViewModel>>(cuerpo);

                if (mantenimientos == null || mantenimientos.Count == 0)
                {

                    ViewBag.Mensaje = "No hay mantenimientos ingresados para mostrar.";

                }
                else
                {

                    List<MantenimientoViewModel> aux = new List<MantenimientoViewModel>();

                    foreach (MantenimientoViewModel m in mantenimientos)
                    {

                        if (m.CabaniaId == Id)
                        {
                            aux.Add(m);
                        }
                    }

                    @ViewBag.Mantenimientos = aux;
                }
            }
            return View();
        }


        // GET: MantenimientoController/
        //public ActionResult MantenimientosPorCabañaPorFechas(int id)
        //{
        //    if (HttpContext.Session.GetString("token") == null) return RedirectToAction("Login", "Usuarios");

        //    return View(new List<MantenimientoViewModel>());
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //// POST: MantenimientoController/
        //public ActionResult MantenimientosPorCabañaPorFechas(DateTime inicio, DateTime fin, int id)
        //{
        //    if (HttpContext.Session.GetString("token") == null) return RedirectToAction("Login", "Usuarios");
        //    //IEnumerable<MantenimientoViewModel> mantenimientos = RepoMantenimientos.MantenimientosPorCabañaPorFechas(inicio, fin, id);


        //    if (mantenimientos.Count() == 0)
        //    {
        //        ViewBag.Mensaje = "No hay mantenimientos para la búsqueda realizada";
        //    }
        //    return View(mantenimientos);

        //}
    }
}
