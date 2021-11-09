using Conexus.Common.Entities;
using Conexus.FrontEnd.Models;
using Conexus.FrontEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conexus.FrontEnd.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IApiServices _apiServices;
        private readonly IConfiguration _configuration;
        private string ApiUrlBase;
        private string ApiServicePrefix;
        private string ApiController;
        public ClientesController(IApiServices apiServices, IConfiguration configuration)
        {
            _apiServices = apiServices;
            _configuration = configuration;

            ApiUrlBase = _configuration["Api:urlBase"];
            ApiServicePrefix = _configuration["Api:servicePrefix"];
            ApiController = _configuration["Api:clienteController"];
        }
        // GET: ClienteController
        public async Task<ActionResult> Index()
        {
            Response response = await _apiServices.GetList<Cliente>(ApiUrlBase, ApiServicePrefix, ApiController);

            if (!response.IsSuccess)
            {
                return NotFound();
            }
            List<Cliente> clientes = (List<Cliente>)response.Result;

            return View(clientes);
        }

        // GET: ClienteController/Details/5
        public ActionResult Products(int id)
        {
            return View();
        }

        // GET: ClienteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Cliente cliente)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string categoriaController = _configuration["Api:categroriaController"];


                Response response = await _apiServices.Post<Cliente>(ApiUrlBase, ApiServicePrefix, ApiController, cliente);

                if (!response.IsSuccess)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClienteController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Response response = await _apiServices.Get<Cliente>(ApiUrlBase, ApiServicePrefix, ApiController + "/" + id);

            if (!response.IsSuccess)
            {
                return NotFound();
            }

            Cliente cliente = (Cliente)response.Result;

            return View(cliente);
        }

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Cliente cliente)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Response response2 = await _apiServices.Put<Cliente>(ApiUrlBase, ApiServicePrefix, ApiController, cliente, id.ToString());

                if (!response2.IsSuccess)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClienteController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {

            Response response = await _apiServices.Get<Cliente>(ApiUrlBase, ApiServicePrefix, ApiController + "/" + id);

            if (!response.IsSuccess)
            {
                return NotFound();
            }
            Cliente cliente = (Cliente)response.Result;

            return View(cliente);
        }

        // POST: ClienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Cliente cliente)
        {

            try
            {

                Response response2 = await _apiServices.Delete<Cliente>(ApiUrlBase, ApiServicePrefix, ApiController, cliente, id.ToString());

                if (!response2.IsSuccess)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
