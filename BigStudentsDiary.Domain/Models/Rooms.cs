namespace BigStudentsDiary.Domain.Models;

public class Rooms
{
    private Rooms(string room, int roomId)
    {
        Room = room;
        RoomId = roomId;
    }

    public string Room { get; set; } = "";
    public int RoomId { get; set; }

    public static Rooms Create(string room, int roomId)
    {
        return new Rooms(room, roomId);
    }
}