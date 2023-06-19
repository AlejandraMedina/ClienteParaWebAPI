using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using PresentacionMVC.Models;
using Microsoft.CodeAnalysis.Host;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace PresentacionMVC.Controllers
{

    public class TipoController : Controller
    {


        public IConfiguration Conf { get; set; }

        public string URLBaseApiTipos { get; set; }

        public TipoController(IConfiguration conf)
        {

            Conf = conf;
            URLBaseApiTipos = Conf.GetValue<String>("ApiTipos");
        }


        // GET: TipoController
        public ActionResult Index()        {

            if (HttpContext.Session.GetString("token") == null) RedirectToAction("Login", "Usuarios");

            HttpClient cliente = new HttpClient();

            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            Task<HttpResponseMessage> tarea1 = cliente.GetAsync(URLBaseApiTipos);
            tarea1.Wait();

            HttpResponseMessage respuesta = tarea1.Result;
           
            String cuerpo = LeerContenido(respuesta);

            if (respuesta.IsSuccessStatusCode)  // Es un status 200 indica todo se ejecutó correctamente.
            {

                
                List<TipoViewModel> tipos= JsonConvert.DeserializeObject<List<TipoViewModel>>(cuerpo);

                if (tipos == null || tipos.Count == 0)
                {

                    ViewBag.Mensaje = "No hay tipos ingresados para mostrar.";
                    
                }
                else
                {
                    return View(tipos);
                
                }
            }
            else 
            {
                ViewBag.Mensaje = cuerpo;

            }

            return View(new List<TipoViewModel>());
        }




        // GET: TipoController/Details/5
        public ActionResult Details(int id)
        {
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            if (HttpContext.Session.GetString("token") == null) return RedirectToAction("Login", "Usuarios");
            try
            {
                TipoViewModel vm = BuscarTipo(id);
                return View(vm);
            } 
            catch(Exception ex )      
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }            
          
        }



        private TipoViewModel BuscarTipo(int id) 
        {
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
           
            string url = URLBaseApiTipos + id;
            var tarea = cliente.GetAsync(url);
            tarea.Wait();
            string cuerpo = LeerContenido(tarea.Result);

            if (tarea.Result.IsSuccessStatusCode)
            {
                
                TipoViewModel tipo = JsonConvert.DeserializeObject<TipoViewModel>(cuerpo);
                return tipo;
            }
            else 
            {
                throw new Exception(cuerpo);
            }

        }


        // GET: TipoController/Create
        public ActionResult CreateTipo()
        {
           
            if (HttpContext.Session.GetString("token") == null) return RedirectToAction("Login", "Usuarios");

            return View();
        }

        // POST: TipoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTipo(TipoViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpClient cliente = new HttpClient();

                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

                    var tarea = cliente.PostAsJsonAsync(URLBaseApiTipos, vm);
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




        // GET: TipoController/Edit/5
        public ActionResult EditTipo(int id)
        {


            if (HttpContext.Session.GetString("token") == null) return RedirectToAction("Login", "Usuarios");
          
            try
            {
                TipoViewModel vm = BuscarTipo(id);
                return View(vm);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }

        }


        // POST: TipoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTipo(int id, TipoViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string url = URLBaseApiTipos + vm.Id;
                    HttpClient cliente = new HttpClient();
                   
                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    Task<HttpResponseMessage> tarea = cliente.PutAsJsonAsync(URLBaseApiTipos, vm);
                    tarea.Wait();
                    HttpResponseMessage respuesta = tarea.Result;

                    if (respuesta.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Mensaje = LeerContenido(respuesta);

                    }
                }
                else
                {
                    ViewBag.Mensaje = "Los datos ingresados no son válidos";
                }
            }
            catch(Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(vm);
            }
            return View(vm);
        }

        // GET: TipoController/Delete/5
        public ActionResult DeleteTipo(int id)
        {
            if (HttpContext.Session.GetString("token") == null) return RedirectToAction("Login", "Usuarios");
            try
            {
                TipoViewModel vm = BuscarTipo(id);
                return View(vm);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        // POST: TipoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTipo(int id, IFormCollection collection)
        {
            try
            {
                HttpClient cliente = new HttpClient();                
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                string url = URLBaseApiTipos + id;
                var tarea = cliente.DeleteAsync(url);
                tarea.Wait();

                HttpResponseMessage respuesta = tarea.Result;
                if (respuesta.IsSuccessStatusCode)
                {

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Mensaje = LeerContenido(respuesta);
                    return View();
                }
               
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "No fue posible eliminar el tipo";
                return View();
            }

        }


        // GET: TipoController/Edit/5
        public ActionResult BuscarTipoPorNombre()
        {
            if (HttpContext.Session.GetString("token") == null) return RedirectToAction("Login", "Usuarios");
            return View();
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

        // POST: TipoController/
      //  [HttpPost]
       // [ValidateAntiForgeryToken]
       // public ActionResult BuscarTipoPorNombre(string nombre)
        //{
         // if (HttpContext.Session.GetString("token") == null) return RedirectToAction("Login", "Usuarios");

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

    //}
}
