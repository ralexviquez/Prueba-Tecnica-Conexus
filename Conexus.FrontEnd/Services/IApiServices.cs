using Conexus.FrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conexus.FrontEnd.Services
{
    public interface IApiServices
    {
        Task<Response> GetList<T>(string urlBase, string servicePrefix, string controller);
        Task<Response> Get<T>(string urlBase, string servicePrefix, string controller);
        Task<Response> PostSimple<T>(string apiRecursos,
                                                   string servicePrefix,
                                                   string controlador,
                                                   T model);
        Task<Response> Post<T>(string apiRecursos,
                                          string servicePrefix,
                                          string controlador,
                                          T model);
        Task<Response> Put<T>(string apiRecursos,
                                          string servicePrefix,
                                          string controladorUsuario,
                                          T model,
                                          string key);
        Task<Response> Delete<T>(string apiRecursos,
                                             string servicePrefix,
                                             string controladorUsuario,
                                             T model,
                                             string key);
    }
}
