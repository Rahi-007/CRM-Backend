public sealed class AiChatRequest
{
    public string Question { get; set; } = string.Empty;

    public DateOnly From { get; set; }
    public DateOnly To { get; set; }

    public List<AiChatMessageDto> History { get; set; } = [];
}

public sealed class AiChatMessageDto
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}