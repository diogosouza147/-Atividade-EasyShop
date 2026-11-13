namespace EasyShop.Services;

/// <summary>Erro de regra de negócio, com mensagem pronta para mostrar ao usuário.</summary>
public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }
}
