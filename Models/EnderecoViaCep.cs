namespace BlazorDeploy.Models
{
    public class EnderecoViaCep
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string erro { get; set; } // usado se o cep for inválido
    }


}
