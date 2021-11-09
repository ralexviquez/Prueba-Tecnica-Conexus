namespace Conexus.FrontEnd.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Net.Http.Headers;
    using Models;
    using Newtonsoft.Json;

    public class ApiServices : IApiServices
    {
        #region Metodos



        public async Task<Response> GetList<T>(string urlBase, string servicePrefix, string controller)
        {
            try
            {
                //crea cliente Http
                var client = new HttpClient();
                //se carga la direccion base
                client.BaseAddress = new Uri(urlBase);
                //arma la url
                var url = string.Format("{0}{1}", servicePrefix, controller);
                //ejecuta la consulta
                var response = await client.GetAsync(url);
                //lee la respuesta
                var result = await response.Content.ReadAsStringAsync();

                //si genera un error (diferente a 200)
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };

                }
                //deserializa la lista
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list,
                };

            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }

        }
        public async Task<Response> Get<T>(string urlBase, string servicePrefix, string controller)
        {
            try
            {
                //crea cliente Http
                var client = new HttpClient();
                //se carga la direccion base
                client.BaseAddress = new Uri(urlBase);
                //arma la url
                var url = string.Format("{0}{1}", servicePrefix, controller);
                //ejecuta la consulta
                var response = await client.GetAsync(url);
                //lee la respuesta
                var result = await response.Content.ReadAsStringAsync();

                //si genera un error (diferente a 200)
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };

                }
                //deserializa la lista
                var list = JsonConvert.DeserializeObject<T>(result);
                //se retorna el objeto de respuesta 
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list,
                };

            }
            catch (Exception ex)
            {
                //se retorna el objeto de respuesta error
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }

        }

       
        public async Task<Response> PostSimple<T>(string apiRecursos,
                                                    string servicePrefix,
                                                    string controlador,
                                                    T model)
        {
            try
            {

                var request = JsonConvert.SerializeObject(model);
                var body = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(apiRecursos);
                var url = string.Format("{0}{1}", servicePrefix, controlador);
                var response = await client.PostAsync(url, body);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString()
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var returno = JsonConvert.DeserializeObject(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = returno,
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                }; ;
            }
        }

        public async Task<Response> Post<T>(string apiRecursos,
                                            string servicePrefix,
                                            string controlador,
                                            T model)
        {

            try
            {
                var request = JsonConvert.SerializeObject(model);
                var body = new StringContent(request, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                // ObservableCollection<String> _post;
                client = new HttpClient();
                client.BaseAddress = new Uri(apiRecursos);
                var url = string.Format(
                    "{0}{1}",
                    servicePrefix,
                    controlador
                    );

                var response = await client.PostAsync(url, body);
                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content == "\"Ya existe\"")
                    {
                        return new Response
                        {
                            IsSuccess = false,
                            Message = "Ya esta registrado"
                        };
                    }
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString()
                    };
                }


                var result = await response.Content.ReadAsStringAsync();
                var returno = JsonConvert.DeserializeObject<T>(result);

                // _post = new ObservableCollection<T>(posts);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = returno
                };
            }
            catch (Exception ex)
            {

                return new Response
                {

                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }


        public async Task<Response> Put<T>(string apiRecursos,
                                            string servicePrefix,
                                            string controladorUsuario,
                                            T model,
                                            string key)
        {

            try
            {
                var request = JsonConvert.SerializeObject(model);
                var body = new StringContent(request, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                // ObservableCollection<String> _post;
                client = new HttpClient();
                client.BaseAddress = new Uri(apiRecursos);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controladorUsuario,
                    key
                    );

                var response = await client.PutAsync(url, body);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString()
                    };
                }


                var result = await response.Content.ReadAsStringAsync();
                var returno = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = returno
                };
            }
            catch (Exception ex)
            {

                return new Response
                {

                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> Delete<T>(string apiRecursos,
                                               string servicePrefix,
                                               string controladorUsuario,
                                               T model,
                                               string key)
        {

            try
            {
                HttpClient client = new HttpClient();
                client = new HttpClient();
                client.BaseAddress = new Uri(apiRecursos);
                var url = string.Format(
                    "{0}{1}/{2}",
                    servicePrefix,
                    controladorUsuario,
                    key
                    );

                var response = await client.DeleteAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString()
                    };
                }


                var result = await response.Content.ReadAsStringAsync();
                var returno = JsonConvert.DeserializeObject<T>(result);

                // _post = new ObservableCollection<T>(posts);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = returno
                };
            }
            catch (Exception ex)
            {

                return new Response
                {

                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        #endregion

    }//fin class



}//fin namespace
