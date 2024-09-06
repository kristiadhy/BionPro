namespace Domain.DTO;

public class ResponseDto
{
    public bool IsSuccess { get; set; }
    public ICollection<string>? Error { get; set; }
}
