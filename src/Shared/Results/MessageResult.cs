namespace Shared.Results;

public class MessageResult(string? mensagem)
{
    public string? Mensagem { get; set; } = mensagem;
}