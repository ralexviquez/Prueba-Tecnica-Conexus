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
using Conexus.FrontEnd.Models;

namespace Conexus.FrontEnd.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IApiServices _apiServices;
        private readonly IConfiguration _configuration;
        private string ApiUrlBase;
        private string ApiServicePrefix;
        private string ApiController;
        public ProductosController(IApiServices apiServices, IConfiguration configuration)
        {
            _apiServices = apiServices;
            _configuration = configuration;

            ApiUrlBase = _configuration["Api:urlBase"];
            ApiServicePrefix = _configuration["Api:servicePrefix"];
            ApiController = _configuration["Api:productoController"];
        }
        // GET: ProductosController
        public async Task<ActionResult> Index()
        {
            Response response = await _apiServices.GetList<Producto>(ApiUrlBase, ApiServicePrefix, ApiController);
            
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            List<Producto> productos = (List<Producto>)response.Result;

            return View(productos);
        }

        

        // GET: ProductosController/Create
        public async Task<ActionResult> Create()
        {
            ProductoViewModel producto = new ProductoViewModel();
            producto.Categorias = await GetComboCategorias();
            return View(producto);
        }

        // POST: ProductosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductoViewModel producto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string categoriaController = _configuration["Api:categroriaController"];

                Response response = await _apiServices.Get<Categoria>(ApiUrlBase, ApiServicePrefix, categoriaController + "/" + producto.CategoriaId);
                
                if (!response.IsSuccess)
                {
                    return NotFound();
                }
                Categoria categoria = (Categoria)response.Result;

                Response response2 = await _apiServices.Post<Producto>(ApiUrlBase, ApiServicePrefix, ApiController,
                    new Producto { 
                    Nombre = producto.Nombre,
                    categoria = categoria
                    } );

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

        // GET: ProductosController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Response response = await _apiServices.Get<Producto>(ApiUrlBase, ApiServicePrefix, ApiController + "/" + id);

            if (!response.IsSuccess)
            {
                return NotFound();
            }
            Producto producto = (Producto)response.Result;

            ProductoViewModel productoVM = new ProductoViewModel();
            productoVM.Id = producto.Id;
            productoVM.Nombre = producto.Nombre;
            productoVM.Categorias = await GetComboCategorias();
            productoVM.CategoriaId = producto.categoria.Id;
            productoVM.categoria = producto.categoria;

            return View(productoVM);

        }

        // POST: ProductosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ProductoViewModel productoVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Producto producto = productoVM;

                string categoriaController = _configuration["Api:categroriaController"];

                Response response = await _apiServices.Get<Categoria>(ApiUrlBase, ApiServicePrefix, categoriaController + "/" + productoVM.CategoriaId);

                if (!response.IsSuccess)
                {
                    return NotFound();
                }
                Categoria categoria = (Categoria)response.Result;
                producto.categoria = categoria;

                Response response2 = await _apiServices.Put<Producto>(ApiUrlBase, ApiServicePrefix, ApiController, producto, id.ToString());

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

        // GET: ProductosController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Response response = await _apiServices.Get<Producto>(ApiUrlBase, ApiServicePrefix, ApiController + "/" + id);

            if (!response.IsSuccess)
            {
                return NotFound();
            }
            Producto producto = (Producto)response.Result;
            ProductoViewModel productoVM = new ProductoViewModel
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                CategoriaId = producto.categoria.Id,
                categoria = producto.categoria
            };

            return View(productoVM);
        }

        // POST: ProductosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ProductoViewModel productoVM)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Producto producto = productoVM;

                string categoriaController = _configuration["Api:categroriaController"];

                Response response = await _apiServices.Get<Categoria>( ApiUrlBase, ApiServicePrefix, categoriaController + "/" + productoVM.CategoriaId );

                if (!response.IsSuccess)
                {
                    return NotFound();
                }
                Categoria categoria = (Categoria)response.Result;
                producto.categoria = categoria;

                Response response2 = await _apiServices.Delete<Producto>(ApiUrlBase, ApiServicePrefix, ApiController, producto, id.ToString());

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


        private async Task<IEnumerable<SelectListItem>> GetComboCategorias()
        {
            string categoriaController = _configuration["Api:categroriaController"];
            Response response = await _apiServices.GetList<Categoria>(ApiUrlBase, ApiServicePrefix, categoriaController);
            //si la respueta es false
            if (!response.IsSuccess)
            {
                return new List<SelectListItem>();
            }
            List<Categoria> categorias = (List<Categoria>)response.Result;
            ViewData["listCategorias"] = categorias;

            List<SelectListItem> list = categorias.Select(x => new SelectListItem
            {
                Text = x.Descripcion,
                Value = $"{x.Id}"
            })
                .OrderBy(x => x.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione una categoría...]",
                Value = "0"
            });

            return list;
        }

    }
}
