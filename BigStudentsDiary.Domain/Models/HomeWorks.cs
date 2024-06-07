using System.ComponentModel.DataAnnotations;

namespace BigStudentsDiary.Domain.Models;

public class HomeWorks
{
    private HomeWorks(Guid homeWorkId, string homeWorkDescription)
    {
        HomeWorkId = homeWorkId;
        HomeWorkDescription = homeWorkDescription;
    }

    public Guid HomeWorkId { get; set; }
    public string HomeWorkDescription { get; set; } = "";

    public static HomeWorks Create(Guid homeWorkId, string homeWorkDescription)
    {
        return new HomeWorks(homeWorkId, homeWorkDescription);
    }
}