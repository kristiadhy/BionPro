using Domain.Parameters;

namespace Web.Services.Features;

public class DataResponse<T> where T : class
{
    //These two properties will never be null because they are initialized when the data is returned. Please check the back end.
    public List<T> Items { get; set; } = default!;
    public MetaData MetaData { get; set; } = default!;
}
