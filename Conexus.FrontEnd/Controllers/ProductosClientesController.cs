using Conexus.Common.Entities;
using Conexus.FrontEnd.Models;
using Conexus.FrontEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conexus.FrontEnd.Controllers
{
    public class ProductosClientesController : Controller
    {
        private readonly IApiServices _apiServices;
        private readonly IConfiguration _configuration;
        private string ApiUrlBase;
        private string ApiServicePrefix;
        private string ApiController;
        public ProductosClientesController(IApiServices apiServices, IConfiguration configuration)
        {
            _apiServices = apiServices;
            _configuration = configuration;

            ApiUrlBase = _configuration["Api:urlBase"];
            ApiServicePrefix = _configuration["Api:servicePrefix"];
            ApiController = _configuration["Api:ProductosClientesController"];
        }

        // GET: ProductosClientesController
        public async Task<ActionResult> Index(int ClienteId)
        {
            Response response = await _apiServices.GetList<ProductosCliente>(ApiUrlBase, ApiServicePrefix, ApiController + "/GetProductosClientes/" + ClienteId);

            if (!response.IsSuccess)
            {
                return NotFound();
            }
            List<ProductosCliente> productosClientes = (List<ProductosCliente>)response.Result;


            ViewData["ClienteId"] = ClienteId;
            return View(productosClientes);
        }


        // GET: ProductosClientesController/Create
        public async Task<ActionResult> Create(int ClienteId)
        {
            ProductosClientesViewModel productoC = new ProductosClientesViewModel();
            productoC.Productos = await GetComboProductos(ClienteId);
            ViewData["ClienteId"] = ClienteId;
            productoC.cliente = new Cliente { Id = ClienteId };
                
            return View(productoC);
        }

        // POST: ProductosClientesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductosClientesViewModel productosClientesViewModel)
        {
            try
            {
                if (productosClientesViewModel.ProductoId <= 0)
                {
                    return BadRequest(ModelState);
                }


                string productoController = _configuration["Api:productoController"];

                Response response = await _apiServices.Get<Producto>(ApiUrlBase, ApiServicePrefix, productoController + "/" + productosClientesViewModel.ProductoId);

                if (!response.IsSuccess)
                {
                    return NotFound();
                }
                Producto producto = (Producto)response.Result;
                producto.categoria = null;

                string clienteController = _configuration["Api:clienteController"];
                Response responseC = await _apiServices.Get<Cliente>(ApiUrlBase, ApiServicePrefix, clienteController + "/" + productosClientesViewModel.cliente.Id);

                if (!response.IsSuccess)
                {
                    return NotFound();
                }
                Cliente cliente = (Cliente)responseC.Result;


                ProductosCliente productosCliente = new ProductosCliente {
                     cliente = cliente,
                     producto = producto
                };

                Response response2 = await _apiServices.Post<ProductosCliente>(ApiUrlBase, ApiServicePrefix, ApiController, productosCliente);

                if (!response2.IsSuccess)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index), new { ClienteId  = cliente.Id });
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductosClientesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Response response = await _apiServices.Get<ProductosCliente>(ApiUrlBase, ApiServicePrefix, ApiController + "/" + id);

            if (!response.IsSuccess)
            {
                return NotFound();
            }
            ProductosCliente productosCliente = (ProductosCliente)response.Result;

            ProductosClientesViewModel productoVM = new ProductosClientesViewModel();
            productoVM.Id = productosCliente.Id;
            productoVM.cliente = productosCliente.cliente;
            productoVM.producto = productosCliente.producto;
            productoVM.ProductoId = productosCliente.producto.Id;
            productoVM.Productos = await GetComboProductos(productosCliente.cliente.Id);

            return View(productoVM);
        }

        // POST: ProductosClientesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ProductosClientesViewModel productosClientesViewModel )
        {
            try
            {
                if (productosClientesViewModel.ProductoId <= 0)
                {
                    return BadRequest(ModelState);
                }

                string productoController = _configuration["Api:productoController"];

                Response response = await _apiServices.Get<Producto>(ApiUrlBase, ApiServicePrefix, productoController + "/" + productosClientesViewModel.ProductoId);

                if (!response.IsSuccess)
                {
                    return NotFound();
                }
                Producto producto = (Producto)response.Result;
                producto.categoria = null;

                string clienteController = _configuration["Api:clienteController"];
                Response responseC = await _apiServices.Get<Cliente>(ApiUrlBase, ApiServicePrefix, clienteController + "/" + productosClientesViewModel.cliente.Id);

                if (!response.IsSuccess)
                {
                    return NotFound();
                }
                Cliente cliente = (Cliente)responseC.Result;


                ProductosCliente productosCliente = new ProductosCliente
                {
                    Id = id,
                    cliente = cliente,
                    producto = producto
                };

                Response response2 = await _apiServices.Put<ProductosCliente>(ApiUrlBase, ApiServicePrefix, ApiController, productosCliente, productosCliente.Id.ToString());

                if (!response2.IsSuccess)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index), new { ClienteId = cliente.Id });
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductosClientesController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Response response = await _apiServices.Get<ProductosCliente>(ApiUrlBase, ApiServicePrefix, ApiController + "/" + id);

            if (!response.IsSuccess)
            {
                return NotFound();
            }
            ProductosCliente producto = (ProductosCliente)response.Result;

            return View(producto);
        }

        // POST: ProductosClientesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ProductosCliente productosCliente)
        {

            try
            {

                Response response2 = await _apiServices.Delete<ProductosCliente>(ApiUrlBase, ApiServicePrefix, ApiController, productosCliente, id.ToString());

                if (!response2.IsSuccess)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index), new { ClienteId = productosCliente.cliente.Id });
            }
            catch
            {
                return View();
            }
        }


        private async Task<IEnumerable<SelectListItem>> GetComboProductos(int ClienteId)
        {
            Response response = await _apiServices.GetList<Producto>(ApiUrlBase, ApiServicePrefix, ApiController + "/GetProductosNoRegistrados/" + ClienteId);
            //si la respueta es false
            if (!response.IsSuccess)
            {
                return new List<SelectListItem>();
            }
            List<Producto> productos = (List<Producto>)response.Result;

            List<SelectListItem> list = productos.Select(x => new SelectListItem
            {
                Text = x.Nombre,
                Value = $"{x.Id}"
            })
                .OrderBy(x => x.Text)
                .ToList();
            if (list.Count() > 0)
            {
                list.Insert(0, new SelectListItem
                {
                    Text = "[Seleccione un Producto...]",
                    Value = "0"
                });
            }
            else
            {
                list.Insert(0, new SelectListItem
                {
                    Text = "[No hay productos disponibles]",
                    Value = "0"
                });
            }


            return list;
        }
    }
}
