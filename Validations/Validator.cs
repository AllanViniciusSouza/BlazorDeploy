using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorDeploy.Validations;

public class Validator : IValidator
{
    public string NomeErro { get; set; } = "";
    public string EmailErro { get; set; } = "";
    public string TelefoneErro { get; set; } = "";
    public string SenhaErro { get; set; } = "";
    public string EnderecoErro { get; set; } = "";


    private const string NomeVazioErroMsg = "Por favor, informe o seu nome.";
    private const string NomeInvalidoErroMsg = "Por favor, informe um nome válido.";
    private const string EmailVazioErroMsg = "Por favor, informe um email.";
    private const string EmailInvalidoErroMsg = "Por favor, informe um email válido.";
    private const string TelefoneVazioErroMsg = "Por favor, informe um telefone.";
    private const string TelefoneInvalidoErroMsg = "Por favor, informe um telefone válido.";
    private const string SenhaVazioErroMsg = "Por favor, informe a senha.";
    private const string SenhaInvalidoErroMsg = "A senha deve conter pelo menos 8 caracteres, incluindo letras e números.";
    private const string EnderecoVazioErroMsg = "Por favor, informe o endereço.";
    private const string EnderecoInvalidoErroMsg = "Por favor, informe um endereço válido.";


    public Task<bool> ValidarUsuario(string nome, string email, string telefone, string senha, string endereco)
    {
        var inNomeValido = ValidarNome(nome);
        var isEmailValido = ValidarEmail(email);
        var isTelefoneValido = ValidarTelefone(telefone);
        var isSenhaValida = ValidarSenha(senha);
        var isEnderecoValido = ValidarEndereco(endereco);

        return Task.FromResult(inNomeValido && isEmailValido && isTelefoneValido && isSenhaValida && isEnderecoValido);
    }

    public Task<bool> ValidarCliente(string nome, string telefone, string endereco)
    {
        var isNomeValido = ValidarNome(nome);
        var isTelefoneValido = ValidarTelefone(telefone);
        var isEnderecoValido = ValidarEndereco(endereco);

        return Task.FromResult(isNomeValido && isTelefoneValido && isEnderecoValido);
    }

    private bool ValidarNome(string nome)
    {
        if (string.IsNullOrEmpty(nome))
        {
            NomeErro = NomeVazioErroMsg;
            return false;
        }

        if(nome.Length < 3)
        {
            NomeErro = NomeInvalidoErroMsg;
        }

        NomeErro = "";
        return true;
    }

    private bool ValidarEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            EmailErro = EmailVazioErroMsg;
            return false;
        }

        if(!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
        {
            EmailErro = EmailInvalidoErroMsg;
            return false;
        }

        EmailErro = "";
        return true;
    }

    private bool ValidarTelefone(string telefone)
    {
        if (string.IsNullOrEmpty(telefone))
        {
            TelefoneErro = TelefoneInvalidoErroMsg;
            return false;
        }

        if(telefone.Length < 12)
        {
            TelefoneErro= TelefoneInvalidoErroMsg;
            return false;
        }

        TelefoneErro = "";
        return true;
    }

    private bool ValidarSenha(string senha)
    {
        if (string.IsNullOrEmpty(senha))
        {
            SenhaErro = SenhaInvalidoErroMsg;
            return false;
        }

        if(senha.Length < 8 || !Regex.IsMatch(senha, @"[a-zA-Z]") || !Regex.IsMatch(senha, @"\d"))
        {
            SenhaErro= SenhaInvalidoErroMsg;
            return false;
        }

        SenhaErro = "";
        return true;
    }

    private bool ValidarEndereco(string endereco)
    {
        if (string.IsNullOrWhiteSpace(endereco))
        {
            EnderecoErro = EnderecoVazioErroMsg;
            return false;
        }

        // Exige pelo menos "rua, número"
        // Ex: "Rua das Flores, 123"
        var partes = endereco.Split(',');
        if (partes.Length < 2 || string.IsNullOrWhiteSpace(partes[0]) || string.IsNullOrWhiteSpace(partes[1]))
        {
            EnderecoErro = "Por favor, informe um endereço com rua e número. Ex: Rua Exemplo, 123";
            return false;
        }

        EnderecoErro = "";
        return true;
    }


}
