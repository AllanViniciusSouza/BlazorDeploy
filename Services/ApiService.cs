using BlazorDeploy.Models;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.ComponentModel.Design;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.JSInterop;
using BlazorDeploy.Pages;
using System.Net.Http.Json;
using System.Buffers.Text;
using static System.Net.WebRequestMethods;

namespace BlazorDeploy.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private readonly string _baseUrl = "https://n2v55gk9-7066.brs.devtunnels.ms/";
    private readonly ILogger<ApiService> _logger;

    JsonSerializerOptions _serializerOptions;

    public ApiService(HttpClient httpClient, ILogger<ApiService> logger, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jsRuntime = jsRuntime;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<ApiResponse<bool>> RegistrarUsuario(string nome, string email, 
                                                        string telefone, string password)
    {
        try
        {
            var register = new Register()
            {
                Nome = nome,
                Email = email,
                Telefone = telefone,
                Senha = password
            };
            var json = JsonSerializer.Serialize(register, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/Usuarios/Register", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
                };
            }
            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao registrar o usuário: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }

    public async Task<ApiResponse<bool>> Login(string email, string password)
    {
        try
        {
            var login = new Login()
            {
                Email = email,
                Senha = password
            };
            var json = JsonSerializer.Serialize(login, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/Usuarios/Login", content);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
                };
            }

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Token>(jsonResult, _serializerOptions);

            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "accesstoken", result.AccessToken);
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "usuarioid", result.UsuarioID.ToString());
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "usuarionome", result.UsuarioNome);

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro no login : {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }

    public async Task<ApiResponse<bool>> ConfirmarPedido(Pedido pedido)
    {
        try
        {
            var json = JsonSerializer.Serialize(pedido, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/Pedidos", content);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.StatusCode == HttpStatusCode.Unauthorized
                    ? "Unauthorized"
                    : $"Erro ao enviar requisição HTTP: {response.StatusCode}";

                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
                return new ApiResponse<bool> { Data = true };
            }
            return new ApiResponse<bool> { Data = true };
        }
        catch(Exception ex)
        {
            _logger.LogError($"Erro ao confirmar pedido: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }

    public async Task<ApiResponse<bool>> ConfirmarComanda(Comanda comanda)
    {
        try
        {
            var json = JsonSerializer.Serialize(comanda, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/Pedidos/PostComanda", content);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.StatusCode == HttpStatusCode.Unauthorized
                    ? "Unauthorized"
                    : $"Erro ao enviar requisição HTTP: {response.StatusCode}";

                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
                return new ApiResponse<bool> { Data = true };
            }
            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao confirmar pedido: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }

    public async Task<ApiResponse<bool>> UploadImagemUsuario(byte[] imageArray)
    {
        try
        {
            var content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(imageArray), "imagem", "image.jpg");
            var response = await PostRequest("api/usuarios/uploadfotousuario", content);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.StatusCode == HttpStatusCode.Unauthorized
                    ? "Unauthorized"
                    : $"Erro ao enviar requisição HTTP: {response.StatusCode}";

                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
                return new ApiResponse<bool> { ErrorMessage = errorMessage };
            }

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao fazer upload da imagem do usuário: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }

    public async Task<HttpResponseMessage> PostRequest(string uri, HttpContent content) 
    { 
        var enderecoUrl = _baseUrl + uri;
        try
        {
            var result = await _httpClient.PostAsync(enderecoUrl, content);
            return result;
        }
        catch (Exception ex)
        {
            //log o erro ou trate conforme necessario
            _logger.LogError($"Erro ao enviar requisição Post para {uri}: {ex.Message}");
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }

    public async Task<(bool Data, string? ErroMessage)> AtualizaQuantidadeItemCarrinho(int produtoId, string acao)
    {
        try
        {
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var response = await PutRequest($"api/ItensCarrinhoCompra?produtoId={produtoId}&acao={acao}", content);
            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }
            else
            {
                if(response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    string erroMessage = "Unauthorized";
                    _logger.LogWarning(erroMessage);
                    return (false, erroMessage);
                }
                string generalErrorMessage = $"Erro na requisição: {response.ReasonPhrase}";
                _logger.LogError(generalErrorMessage);
                return (false, generalErrorMessage);
            }
        }
        catch(HttpRequestException ex)
        {
            string errorMessage = $"Erro de requisição HTTP: {ex.Message}";
            _logger.LogError(ex, errorMessage);
            return (false, errorMessage);
        }
        catch(Exception ex)
        {
            string errorMessage = $"Erro inesperado: {ex.Message}";
            _logger.LogError(ex, errorMessage);
            return (false, errorMessage);
        }
    }

    public async Task<(bool Data, string? ErroMessage)> AtualizaQuantidadeItemComanda(int produtoId, string acao, string idComanda)
    {
        try
        {
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var response = await PutRequest($"api/ItensComanda?produtoId={produtoId}&acao={acao}&idComanda={idComanda}", content);
            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    string erroMessage = "Unauthorized";
                    _logger.LogWarning(erroMessage);
                    return (false, erroMessage);
                }
                string generalErrorMessage = $"Erro na requisição: {response.ReasonPhrase}";
                _logger.LogError(generalErrorMessage);
                return (false, generalErrorMessage);
            }
        }
        catch (HttpRequestException ex)
        {
            string errorMessage = $"Erro de requisição HTTP: {ex.Message}";
            _logger.LogError(ex, errorMessage);
            return (false, errorMessage);
        }
        catch (Exception ex)
        {
            string errorMessage = $"Erro inesperado: {ex.Message}";
            _logger.LogError(ex, errorMessage);
            return (false, errorMessage);
        }
    }

    private async Task<HttpResponseMessage> PutRequest(string uri, HttpContent content)
    {
        var enderecoUrl = AppConfig.BaseUrl + uri;
        try
        {
            AddAuthorizationHeader();
            var result = await _httpClient.PutAsync(enderecoUrl, content);
            return result;
        }
        catch(Exception ex)
        {
            _logger.LogError($"Erro ao enviar requisição PUT para {uri}: {ex.Message}");
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }

    public async Task<ApiResponse<bool>> AdicionaItemNoCarrinho(CarrinhoCompra carrinhoCompra)
    {
        try
        {
            var json = JsonSerializer.Serialize(carrinhoCompra, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/ItensCarrinhoCompra", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
                };
            }
            return new ApiResponse<bool> { Data = true };
        }
        catch(Exception ex)
        {
            _logger.LogError($"Erro ao adicionar item no carrinho: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }

    public async Task<ApiResponse<Comanda>> CriarComanda(Comanda comanda)
    {
        try
        {
            var json = JsonSerializer.Serialize(comanda, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/comandas", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
                return new ApiResponse<Comanda>
                {
                    ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
                };
            }
            // 🛠️ Ler a resposta JSON e desserializar para o objeto Clientes
            var responseContent = await response.Content.ReadAsStringAsync();
            var novaComanda = JsonSerializer.Deserialize<Comanda>(responseContent, _serializerOptions);

            return new ApiResponse<Comanda> { Data = novaComanda };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Comanda> { ErrorMessage = ex.Message };
        }
    }

    public async Task<ApiResponse<bool>> AdicionaItemNaComanda(ComandaItem novoItem)
    {
        try
        {
            var json = JsonSerializer.Serialize(novoItem, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Agora estamos enviando o item para uma comanda específica
            var response = await PostRequest($"api/ItensComanda", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
                };
            }
            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao adicionar item na comanda: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }


    public async Task<(List<Categoria>? Categorias, string? ErrorMessage)> GetCategorias()
    {
        return await GetAsync<List<Categoria>>("api/categorias");
    }

    public async Task<(List<Produto>?Produtos,string?ErrorNessage)>GetProdutos(string tipoProduto, string categoriaId)
    {
        string endpoint = $"api/Produtos?tipoProduto={tipoProduto}&categoriaId={categoriaId}";
        return await GetAsync<List<Produto>>(endpoint);
    }

    public async Task<(List<Produto>? Produtos, string? ErrorNessage)> GetTodosProdutos()
    {
        string endpoint = $"api/Produtos/GetTodosProdutos";
        return await GetAsync<List<Produto>>(endpoint);
    }

    public async Task<ApiResponse<bool>> AdicionarProdutoAsync(Produto produto)
    {
        try
        {
            var json = JsonSerializer.Serialize(produto, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/produtos", content);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.StatusCode == HttpStatusCode.Unauthorized
                    ? "Unauthorized"
                    : $"Erro ao enviar requisição HTTP: {response.StatusCode}";

                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
                return new ApiResponse<bool> { Data = true };
            }
            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao criar produto: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }

    // Método para excluir um produto
    public async Task<bool> ExcluirProdutoAsync(int idProduto)
    {
        var response = await _httpClient.DeleteAsync($"api/produtos/{idProduto}");
        return response.IsSuccessStatusCode;
    }

    public async Task<(Produto? ProdutoDetalhe, string? ErrorMessage)> GetProdutoDetalhe(int produtoId)
    {
        string endpoint = $"api/produtos/{produtoId}";
        return await GetAsync<Produto>(endpoint);
    }

    public async Task<(List<CarrinhoCompraItem>? CarrinhoCompraItems, string? ErrorMessage)> GetItensCarrinhoCompra (int usuarioId)
    {
        var endpoint = $"api/ItensCarrinhoCompra/{usuarioId}";
        return await GetAsync<List<CarrinhoCompraItem>>(endpoint);
    }

    public async Task<(ImagemPerfil? ImagemPerfil, string? ErrorMessage)> GetImagemPerfilUsuario()
    {
        string endpoint = "api/usuarios/ImagemPerfilUsuario";
        return await GetAsync<ImagemPerfil>(endpoint);
    }

    public async Task<(List<PedidoPorUsuario>?, string? ErrorMessage)> GetPedidosPorUsuario(int usuarioId)
    {
        string endpoint = $"api/pedidos/PedidosPorUsuario/{usuarioId}";
        return await GetAsync<List<PedidoPorUsuario>>(endpoint);
    }

    public async Task<(List<Pedido>?, string? ErrorMessage)> GetPedidosHoje()
    {
        string endpoint = $"api/pedidos/PedidosHoje";
        return await GetAsync<List<Pedido>>(endpoint);
    }

    public async Task<(List<Pedido>?, string? ErrorMessage)> GetPedidosPorData(DateTime data)
    {
        string endpoint = $"api/pedidos/data/{data:yyyy-MM-dd}";
        return await GetAsync<List<Pedido>>(endpoint);
    }

    public async Task<(List<Comanda>?, string? ErrorMessage)> GetComandasAbertas()
    {
        string endpoint = $"api/comandas/ComandasAbertas";
        return await GetAsync<List<Comanda>>(endpoint);
    }

    public async Task<(List<Comanda>?, string? ErrorMessage)> GetComandas()
    {
        string endpoint = $"api/comandas/comandas";
        return await GetAsync<List<Comanda>>(endpoint);
    }

    public async Task<(Comanda?, string? ErrorMessage)> GetComandaNumero(int? numeroComanda)
    {
        string endpoint = $"api/comandas/ComandaNumero/{numeroComanda}";
        return await GetAsync<Comanda>(endpoint);
    }

    public async Task<(List<PedidoDetalhe>?, string? ErrorMessage)> GetPedidoDetalhes(int pedidoId)
    {
        string endpoint = $"api/pedidos/DetalhesPedido/{pedidoId}";
        return await GetAsync<List<PedidoDetalhe>>(endpoint);
    }

    public async Task<(List<ComandaPorUsuario>?, string? ErrorMessage)> GetComandaPorUsuario(int usuarioId)
    {
        string endpoint = $"api/comandas/ComandasPorUsuario/{usuarioId}";
        return await GetAsync<List<ComandaPorUsuario>>(endpoint);
    }

    public async Task<(List<ComandaDetalhe>?, string? ErrorMessage)> GetComandaDetalhes(string IdComanda)
    {
        string endpoint = $"api/comandas/DetalhesComanda/{IdComanda}";
        return await GetAsync<List<ComandaDetalhe>>(endpoint);
    }

    public async Task<(Comanda?, string? ErrorMessage)> GetComandaPorId (string IdComanda)
    {
        string endpoint = $"api/Comandas/ComandaPorId/{IdComanda}";
        return await GetAsync<Comanda>(endpoint);
    }

    public async Task<(List<ComandaItem>? ComandaItems, string? ErrorMessage)> GetItensComanda(string IdComanda)
    {
        string endpoint = $"api/ItensComanda/{IdComanda}";
        return await GetAsync<List<ComandaItem>>(endpoint);
    }

    public async Task<(List<Estoque>?, string? ErrorMessage)> GetEstoque()
    {
        string endpoint = $"api/estoque/";
        return await GetAsync<List<Estoque>>(endpoint);
    }

    public async Task<(Estoque?, string? ErrorMessage)> GetProdutoEstoque(int id)
    {
        string endpoint = $"api/estoque/{id}";
        return await GetAsync<Estoque>(endpoint);
    }

    public async Task<ApiResponse<bool>> AtualizarEstoque(Estoque estoque)
    {
        try
        {
            var json = JsonSerializer.Serialize(estoque, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PutRequest($"api/estoque/{estoque.Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao atualizar estoque: {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Erro ao atualizar estoque: {response.StatusCode}",
                    Data = false
                };
            }

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro na atualização do estoque: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message, Data = false };
        }
    }

    // 🔄 Obter todas as movimentações
    public async Task<(List<MovimentacaoEstoque>?, string? ErrorMessage)> GetMovimentacoesAsync()
    {
        string endpoint = $"api/movimentacaoestoque/";
        return await GetAsync<List<MovimentacaoEstoque>>(endpoint);
    }

    // 📦 Registrar uma movimentação de estoque
    public async Task<ApiResponse<bool>> MovimentarEstoqueAsync(MovimentacaoEstoque movimentacao)
    {
        try
        {
            var json = JsonSerializer.Serialize(movimentacao, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/movimentacaoestoque/", content);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = response.StatusCode == HttpStatusCode.Unauthorized
                    ? "Unauthorized"
                    : $"Erro ao enviar requisição HTTP: {response.StatusCode}";

                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
                return new ApiResponse<bool> { Data = true };
            }
            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao criar produto: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message };
        }
    }

    public async Task<ApiResponse<bool>> AtualizarComanda(Comanda comanda)
    {
        try
        {
            var json = JsonSerializer.Serialize(comanda, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PutRequest($"api/comandas/{comanda.IdComanda}", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao atualizar comanda: {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Erro ao atualizar comanda: {response.StatusCode}",
                    Data = false
                };
            }

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro na atualização da comanda: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message, Data = false };
        }
    }

    public async Task<bool> DeletarComanda(string idComanda)
    {
        var response = await _httpClient.DeleteAsync($"api/comandas/{idComanda}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AtualizarProduto (int id, Produto produto)
    {
        try
        {
            var json = JsonSerializer.Serialize(produto, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PutRequest($"api/produtos/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao atualizar produto: {response.StatusCode}");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro na atualização do produto: {ex.Message}");
            return false;
        }
    }


    private async Task<(T? Data, string?ErrorMessage)>GetAsync<T>(string endpoint)
    {
        try
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync(AppConfig.BaseUrl + endpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<T>(responseString, _serializerOptions);
                return (data ?? Activator.CreateInstance<T>(), null);
            }
            else
            {
                if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    string errorMessage = "Unauthorized";
                    _logger.LogWarning(errorMessage);
                    return (default, errorMessage);
                }

                string generalErrorMessage = $"Erro na requisição:{response.ReasonPhrase}";
                _logger.LogError(generalErrorMessage);
                return (default, generalErrorMessage);
            }
        }
        catch (HttpRequestException ex)
        {
            string errorMessage = $"Erro de requisição HTTP: {ex.Message}";
            _logger.LogError(ex, errorMessage);
            return (default, errorMessage);
        }
        catch (JsonException ex)
        {
            string errorMessage = $"Erro de desserialização JSON: {ex.Message}";
            _logger.LogError(ex, errorMessage);
            return (default, errorMessage);
        }
        catch (Exception ex)
        {
            string errorMessage = $"Erro inesperado: {ex.Message}";
            _logger.LogError(ex, errorMessage);
            return (default, errorMessage);
        }

    }

    private async void AddAuthorizationHeader()
    {
        var token = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "accesstoken");

        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

         /// <summary>
         /// Obtém todos os clientes.
         /// </summary>
        public async Task<(List<Clientes>?, string? ErrorMessage)> GetClientesAsync()
        {
            string endpoint = $"api/clientes";
            return await GetAsync<List<Clientes>>(endpoint);
        }

        /// <summary>
        /// Obtém um cliente pelo ID.
        /// </summary>
        public async Task<(Clientes?, string? ErrorMessage)> GetClienteByIdAsync(int id)
        {
            string endpoint = $"api/clientes/{id}";
            return await GetAsync<Clientes>(endpoint);
        }

        /// <summary>
        /// Obtém um cliente pelo Nome.
        /// </summary>
        public async Task<(Clientes?, string? ErrorMessage)> GetClienteByNameAsync(string nome)
        {
            string endpoint = $"api/clientes/byname/{nome}";
            return await GetAsync<Clientes>(endpoint);
        }

    /// <summary>
    /// Adiciona um novo cliente.
    /// </summary>
    public async Task<ApiResponse<Clientes>> AddClienteAsync(Clientes cliente)
        {
        try
        {
            var json = JsonSerializer.Serialize(cliente, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/clientes/Register", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
                return new ApiResponse<Clientes>
                {
                    ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
                };
            }
            // 🛠️ Ler a resposta JSON e desserializar para o objeto Clientes
            var responseContent = await response.Content.ReadAsStringAsync();
            var novoCliente = JsonSerializer.Deserialize<Clientes>(responseContent, _serializerOptions);

            return new ApiResponse<Clientes> { Data = novoCliente };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Clientes> {ErrorMessage = ex.Message };
        }
        }

    /// <summary>
    /// Atualiza um cliente existente.
    /// </summary>
    //public async Task<(List<Clientes>?, string? ErrorMessage)> UpdateClienteAsync(int id, Clientes cliente)
    //{
    //try
    //{
    //    var json = JsonSerializer.Serialize(cliente, _serializerOptions);
    //    var content = new StringContent(json, Encoding.UTF8, "application/json");

    //    var response = await PostRequest($"api/clientes/{id}", content);

    //    return await PutRequest<List<Clientes>>(endpoint);
    //var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", cliente);
    //    return response.IsSuccessStatusCode;
    //}

    /// <summary>
    /// Deleta um cliente pelo ID.
    /// </summary>
    //public async Task<(List<Clientes>?, string? ErrorMessage)> DeleteClienteAsync(int id)
    //{
    //string endpoint = $"api/clientes//{id}";
    //return await GetAsync<List<Clientes>>(endpoint);
    //var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
    //    return response.IsSuccessStatusCode;
    //}
    //}
    public async Task<bool> DeleteClienteAsync(int id)
    {
        string endpoint = $"api/clientes/{id}";
        var response = await DeleteRequest(endpoint);
        return response.IsSuccessStatusCode;
    }

    public async Task<HttpResponseMessage> DeleteRequest(string uri)
    {
        var enderecoUrl = _baseUrl + uri;
        try
        {
            var result = await _httpClient.DeleteAsync(enderecoUrl);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao enviar requisição DELETE para {uri}: {ex.Message}");
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }

    public async Task<ApiResponse<bool>> RegistrarCascoEmprestado(TodosRegistrosCascos registroCasco)
    {
        try
        {
            var json = JsonSerializer.Serialize(registroCasco, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/CascosEmprestados", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
                };
            }

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool> { Data = true };
        }
    }
    public async Task<(List<TodosRegistrosCascos>?, string? ErrorMessage)> GetCascosEmprestados()
    {
        string endpoint = $"api/CascosEmprestados/TodosRegistros";
        return await GetAsync<List<TodosRegistrosCascos>>(endpoint);
    }
    public async Task<(List<DetalhesRegistroCasco>?, string? ErrorMessage)> GetCascoDetalhes(int cascoId)
    {
        string endpoint = $"api/CascosEmprestados/DetalhesRegistro/{cascoId}";
        return await GetAsync<List<DetalhesRegistroCasco>>(endpoint);
    }

    public async Task<(bool Success, string ErrorMessage)> DeleteCascosEmprestados(int id)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/CascosEmprestados/{id}");

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }
            else
            {
                string errorMsg = await response.Content.ReadAsStringAsync();
                return (false, errorMsg);
            }
        }
        catch (Exception ex)
        {
            return (false, $"Erro ao conectar à API: {ex.Message}");
        }
    }
    public async Task<ApiResponse<bool>> DevolverCasco(TodosRegistrosCascos registroCascos)
    {
        try
        {
            var json = JsonSerializer.Serialize(registroCascos, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PutRequest($"api/CascosEmprestados/{registroCascos.Id}", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao atualizar o registro: {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Erro ao atualizar o registro: {response.StatusCode}",
                    Data = false
                };
            }

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro na atualização da o registro: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message, Data = false };
        }
    }

    public async Task<(List<Caixa>?, string? ErrorMessage)> ListarCaixasAsync()
    {
        string endpoint = $"api/caixa";
        return await GetAsync<List<Caixa>>(endpoint);
    }

    public async Task<(List<Caixa>?, string? ErrorMessage)> BuscarCaixaPorIdAsync(int id)
    {
        string endpoint = $"api/caixa/{id}";
        return await GetAsync<List<Caixa>>(endpoint);
    }

    public async Task<ApiResponse<bool>> AbrirCaixaAsync(AbrirCaixaDto dto)
    {
        try
        {
            var json = JsonSerializer.Serialize(dto, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostRequest("api/caixa/abrir", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Erro ao enviar requisição HTTP: {response.StatusCode}"
                };
            }

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool> { Data = false };
        }
    }

    public async Task<ApiResponse<bool>> FecharCaixaAsync(int id, FecharCaixaDto dto)
    {
        try
        {
            var json = JsonSerializer.Serialize(dto, _serializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PutRequest($"api/caixa/fechar/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Erro ao fechar o caixa: {response.StatusCode}");
                return new ApiResponse<bool>
                {
                    ErrorMessage = $"Erro ao fechar o caixa: {response.StatusCode}",
                    Data = false
                };
            }

            return new ApiResponse<bool> { Data = true };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao fechar o caixa: {ex.Message}");
            return new ApiResponse<bool> { ErrorMessage = ex.Message, Data = false };
        }
    }

}

