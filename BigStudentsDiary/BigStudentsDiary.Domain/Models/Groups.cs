namespace BigStudentsDiary.Domain.Models;

public class Groups
{
    private Groups(int groupId, string groupCode)
    {
        GroupId = groupId;
        GroupCode = groupCode;
    }

    public int GroupId { get; set; }
    public string GroupCode { get; set; } = "";

    public static Groups Create(int groupId, string groupCode)
    {
        return new Groups(groupId, groupCode);
    }
}
