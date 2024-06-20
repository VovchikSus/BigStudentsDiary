namespace BigStudentsDiary.Domain.Models;

public class Floors
{
    private Floors(string floor, int floorId)
    {
        Floor = floor;
        FloorId = floorId;
    }


    public string Floor { get; set; } = "";
    public int FloorId { get; set; }


    public static Floors Create(string floor, int floorId)
    {
        return new Floors(floor, floorId);
    }
}