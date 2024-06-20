namespace BigStudentsDiary.Domain.Models;

public class Departments
{
    private Departments(int departmentId, string departmentCode, string departmentName)
    {
        DepartmentId = departmentId;
        DepartmentCode = departmentCode;
        DepartmentName = departmentName;
    }


    public int DepartmentId { get; set; }
    public string DepartmentCode { get; set; } = "";
    public string DepartmentName { get; set; } = "";


    public static Departments Create(int departmentId, string departmentCode, string departmentName)
    {
        return new Departments(departmentId, departmentCode, departmentName);
    }
}