namespace Domain.DTO;

public class ResponseDto
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public ICollection<string>? Errors { get; set; }
}
