using System.ComponentModel.DataAnnotations;

namespace BigStudentsDiary.Domain.Models;

public class HomeWorks
{
    private HomeWorks(string homeWorkDescription, Guid homeWorkId)
    {
        HomeWorkDescription = homeWorkDescription;
        HomeWorkId = homeWorkId;
    }


    public string HomeWorkDescription { get; set; } = "";
    public Guid HomeWorkId { get; set; }

    public static HomeWorks Create(string homeWorkDescription, Guid homeWorkId)
    {
        return new HomeWorks(homeWorkDescription, homeWorkId);
    }
}