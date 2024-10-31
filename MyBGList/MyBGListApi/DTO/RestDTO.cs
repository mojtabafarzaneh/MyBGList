using System.Reflection.Metadata.Ecma335;

namespace MyBGListApi.DTO;

public class RestDTO<T>
{
    public T Data { get; set; } = default!;

    public int? PageSize { get; set; }
    public int? PageIndex { get; set; }
    public int? RecordCount { get; set; }
    public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();


}