namespace Domain.DTO;

public class ApiResponseDto<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
}
