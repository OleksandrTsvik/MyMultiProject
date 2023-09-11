using Application.Core;

namespace Application.Duties.DTOs;

public class DutyParams : PagingParams
{
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? DateCreation { get; set; }
    public string BackgroundColor { get; set; }
    public string FontColor { get; set; }
}
