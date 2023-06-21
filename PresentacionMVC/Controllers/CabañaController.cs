using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PresentacionMVC.Models;
using System.Net.Http.Headers;

namespace PresentacionMVC.Controllers
{
    public class CabañaController : Controller
    {
        public IConfiguration Conf { get; set; }

        public string URLBaseApiCabaña { get; set; }
        public string URLBaseApiTipo { get; set; }

        public CabañaController(IConfiguration conf)
        {

            Conf = conf;
            URLBaseApiCabaña = Conf.GetValue<String>("ApiCabaña");
            URLBaseApiTipo = Conf.GetValue<String>("ApiTipo");
        }

        // GET: CabañaController
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("token") == null) RedirectToAction("Login", "Usuarios");

            HttpClient cliente = new HttpClient();

            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            Task<HttpResponseMessage> tarea1 = cliente.GetAsync(URLBaseApiCabaña);
            tarea1.Wait();

            HttpResponseMessage respuesta = tarea1.Result;

            String cuerpo = LeerContenido(respuesta);

            if (respuesta.IsSuccessStatusCode)  // Es un status 200 indica todo se ejecutó correctamente.
            {


                List<CabañaViewModel> cabañas = JsonConvert.DeserializeObject<List<CabañaViewModel>>(cuerpo);

                if (cabañas == null || cabañas.Count == 0)
                {

                    ViewBag.Mensaje = "No hay cabañas ingresados para mostrar.";

                }
                else
                {
                    return View(cabañas);

                }
            }
            else
            {
                ViewBag.Mensaje = cuerpo;

            }

            return View(new List<CabañaViewModel>());
        }

        // GET: CabañaController/Details/5
        public ActionResult Details(int id)
        {
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            if (HttpContext.Session.GetString("token") == null) return RedirectToAction("Login", "Usuarios");
            try
            {
                CabañaViewModel vm = BuscarCabaña(id);
                return View(vm);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        private CabañaViewModel BuscarCabaña(int id)
        {
            HttpClient cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            string url = URLBaseApiCabaña + id;
            var tarea = cliente.GetAsync(url);
            tarea.Wait();
            string cuerpo = LeerContenido(tarea.Result);

            if (tarea.Result.IsSuccessStatusCode)
            {

                CabañaViewModel cabaña = JsonConvert.DeserializeObject<CabañaViewModel>(cuerpo);
                return cabaña;
            }
            else
            {
                throw new Exception(cuerpo);
            }

        }

        // GET: CabañaController/Create
        public ActionResult CreateCabaña()
        {
            if (HttpContext.Session.GetString("token") == null) return RedirectToAction("Login", "Usuarios");

            //aca necesito traerme los tipos voy a la api o puedo consumir el tipo controller el index 

       

            HttpClient cliente = new HttpClient();

            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            Task<HttpResponseMessage> tarea1 = cliente.GetAsync(URLBaseApiTipo);
            tarea1.Wait();

            HttpResponseMessage respuesta = tarea1.Result;

            String cuerpo = LeerContenido(respuesta);

            if (respuesta.IsSuccessStatusCode)  // Es un status 200 indica todo se ejecutó correctamente.
            {


                List<TipoViewModel> tipos = JsonConvert.DeserializeObject<List<TipoViewModel>>(cuerpo);

                if (tipos == null || tipos.Count == 0)
                {

                    ViewBag.Mensaje = "No hay tipos ingresados para mostrar.";

                }
                else
                {
                    @ViewBag.Tipos = tipos;

                }
            }
           
            
            return View();
        }

        // POST: CabañaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCabaña(CabañaViewModel cabaña)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpClient cliente = new HttpClient();

                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

                    var tarea = cliente.PostAsJsonAsync(URLBaseApiCabaña, cabaña);
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

        // GET: CabañaController/Edit/5
        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("token") == null) return RedirectToAction("Login", "Usuarios");

            try
            {
                CabañaViewModel vm = BuscarCabaña(id);
                return View(vm);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }

        }

        // POST: CabañaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CabañaViewModel cabaña)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string url = URLBaseApiCabaña + cabaña.Id;
                    HttpClient cliente = new HttpClient();

                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    Task<HttpResponseMessage> tarea = cliente.PutAsJsonAsync(URLBaseApiCabaña, cabaña);
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
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(cabaña);
            }
            return View(cabaña);
        
        }

        // GET: CabañaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CabañaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CabañaViewModel cabaña)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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











        //// GET: CabañasController/Create
        //public ActionResult CabañasPorCantMaxPersonas()
        //{

        //    HttpContext.Session.SetString("Menu", "no");
        //    IEnumerable<Tipo> tipos = ListadoTipos.ObtenerListado();
        //    ViewBag.Tipos = tipos;

        //    return View(new List<Cabaña>());
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        ////POST: CabañaController/Details/
        //public ActionResult CabañasPorCantMaxPersonas(int MaxPersonas)
        //{

        //    IEnumerable<Cabaña> cabañas = RepoCabañas.CabañasPorCantMaxPersonas(MaxPersonas);


        //    if (cabañas.Count() == 0)
        //    {
        //        ViewBag.Mensaje = "No hay cabañas disponibles para esta cantidad de personas";
        //    }
        //    return View(cabañas);
        //}


        //// GET: CabañasController/Create
        //public ActionResult CabañasPorTexto()
        //{

        //    return View(new List<Cabaña>());
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //// POST: CabañaController/Details/
        //public ActionResult CabañasPorTexto(string txt)
        //{

        //    IEnumerable<Cabaña> cabañas = RepoCabañas.CabañasPorTexto(txt.Trim());

        //    if (cabañas.Count() == 0)
        //    {
        //        ViewBag.Mensaje = "No hay cabañas para la búsqueda realizada";
        //    }
        //    return View(cabañas);

        //}



        //// GET: CabañasController/Create
        //public ActionResult CabañasHabilitadas()
        //{
        //    IEnumerable<Cabaña> cabañas = RepoCabañas.CabañasHabilitadas();

        //    return View(cabañas);
        //}



        //// GET: CabañasController/Cabañas por tipo
        //public ActionResult CabañasPorTipo()
        //{

        //    HttpContext.Session.SetString("Menu", "no");
        //    IEnumerable<Tipo> tipos = ListadoTipos.ObtenerListado();
        //    ViewBag.Tipos = tipos;

        //    return View(new List<Cabaña>());

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //// POST: CabañaController/Details/
        //public ActionResult CabañasPorTipo(int IdTipo)
        //{

        //    IEnumerable<Cabaña> cabañas = RepoCabañas.CabañasPorTipo(IdTipo);
        //    IEnumerable<Tipo> tipos = ListadoTipos.ObtenerListado();
        //    ViewBag.Tipos = tipos;


        //    if (cabañas.Count() == 0)
        //    {
        //        ViewBag.Mensaje = "No hay cabañas de este tipo para mostrar";
        //    }
        //    return View(cabañas);

        //}

    }
}
