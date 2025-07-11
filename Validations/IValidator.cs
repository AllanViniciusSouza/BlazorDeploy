namespace BlazorDeploy.Validations;

public interface IValidator
{
    string NomeErro { get; set; }
    string EmailErro {  get; set; }
    string TelefoneErro {  get; set; }
    string SenhaErro {  get; set; }
    string EnderecoErro { get; set; }
    Task<bool> ValidarUsuario(string nome, string email, string telefone, string senha, string endereco);

    Task<bool> ValidarCliente(string nome, string telefone, string endereco);
}
