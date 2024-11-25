namespace BigStudentsDiary.Domain.Models;

public class Students
{
    private Students(Guid studentId, string name, string surname, string studentLogin, string studentPassword,
        int groupId)
    {
        StudentId = studentId;
        Name = name;
        Surname = surname;
        StudentLogin = studentLogin;
        StudentPassword = studentPassword;
        GroupId = groupId;
    }

    public Guid StudentId { get; set; }
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public string StudentLogin { get; set; } = "";
    public string StudentPassword { get; set; } = "";
    public int GroupId { get; set; }


    public static Students Create(Guid studentId, string name, string surname, string studentLogin,
        string studentPassword,
        int groupId)
    {
        return new Students(studentId, name, surname, studentLogin,
            studentPassword,
            groupId);
    }
}