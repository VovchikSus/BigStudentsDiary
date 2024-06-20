namespace BigStudentsDiary.Domain.Models;

public class Disciplines
{
    private Disciplines(string discipline, int disciplineId)
    {
        Discipline = discipline;
        DisciplineId = disciplineId;
    }


    public string Discipline { get; set; } = "";
    public int DisciplineId { get; set; }


    public static Disciplines Create(string discipline, int disciplineId)
    {
        return new Disciplines(discipline, disciplineId);
    }
}