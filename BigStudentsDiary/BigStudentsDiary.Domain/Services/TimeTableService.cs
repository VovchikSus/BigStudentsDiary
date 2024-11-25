using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Interfaces.IRepositories;
using BigStudentsDiary.Domain.Models;

namespace BigStudentsDiary.Domain.Services;

public class TimeTableService
{
    private readonly ITimeTableRepository _timeTableRepository;
    private readonly IDisciplinesRepository _disciplinesRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IBuildingRepository _buildingRepository;

    public TimeTableService(ITimeTableRepository timeTableRepository, IDisciplinesRepository disciplinesRepository,
        IDepartmentRepository departmentRepository, IRoomRepository roomRepository,
        IBuildingRepository buildingRepository)
    {
        _timeTableRepository = timeTableRepository;
        _disciplinesRepository = disciplinesRepository;
        _departmentRepository = departmentRepository;
        _roomRepository = roomRepository;
        _buildingRepository = buildingRepository;
    }

    public async Task<IEnumerable<FullTimeTableDto>> GetFullTimeTableByGroup(int groupId, DateTime dayDate)
    {
        var timetableResult = await _timeTableRepository.GetTimeTableByGroup(groupId, dayDate);

        if (!timetableResult.Successful)
        {
            throw new Exception("Failed to get timetable");
        }

        var timetable = timetableResult.Result;
        var disciplineIds = timetable.Select(tt => tt.DisciplineId).Distinct();
        var departmentIds = timetable.Select(tt => tt.DepartmentId).Distinct();
        var roomIds = timetable.Select(tt => tt.RoomId).Distinct();
        var buildingIds = timetable.Select(tt => tt.BuildingId).Distinct();

        var disciplinesResult = await _disciplinesRepository.GetAllAsync(d => disciplineIds.Contains(d.DisciplineId));
        var departmentsResult = await _departmentRepository.GetAllAsync(d => departmentIds.Contains(d.DepartmentId));
        var roomsResult = await _roomRepository.GetAllAsync(r => roomIds.Contains(r.RoomId));
        var buildingResult = await _buildingRepository.GetAllAsync(b => buildingIds.Contains(b.BuildingId));

        if (!disciplinesResult.Successful)
        {
            throw new Exception("Failed to get disciplines");
        }

        if (!departmentsResult.Successful)
        {
            throw new Exception("Failed to get departments");
        }

        if (!roomsResult.Successful)
        {
            throw new Exception("Failed to get rooms");
        }

        if (!buildingResult.Successful)
        {
            throw new Exception("Failed to get rooms");
        }

        var disciplines =
            disciplinesResult.Result.ToDictionary<Disciplines, int, string>(d => d.DisciplineId, d => d.Discipline);
        var departments =
            departmentsResult.Result.ToDictionary<Departments, int, string>(d => d.DepartmentId, d => d.DepartmentName);
        var rooms = roomsResult.Result.ToDictionary<Rooms, int, string>(r => r.RoomId, r => r.Room);
        var building =
            buildingResult.Result.ToDictionary<Buildings, int, string>(b => b.BuildingId, b => b.Building);

        var fullTimeTable = timetable.Select(tt => new FullTimeTableDto
        {
            LessonId = tt.LessonId,
            DayDate = tt.DayDate,
            Number = tt.Number,
            Building = building[tt.BuildingId],
            Discipline = disciplines[tt.DisciplineId],
            DayOfWeekName = tt.DayOfWeekName,
            TimeStart = tt.TimeStart,
            TimeEnd = tt.TimeEnd,
            TimeRange = tt.TimeRange,
            IsSession = tt.IsSession,
            GroupId = tt.GroupId,
            BuildingId = tt.BuildingId,
            Room = rooms[tt.RoomId],
            Department = departments[tt.DepartmentId],
            SemesterId = tt.SemesterId,
            TypeId = tt.TypeId,
            TeacherInternalId = tt.TeacherInternalId
        });

        return fullTimeTable;
    }

    public async Task<IEnumerable<FullTimeTableDto>> GetFullTimeTableByGroup(int groupId)
    {
        var timetableResult = await _timeTableRepository.GetTimeTableByGroup(groupId);

        if (!timetableResult.Successful)
        {
            throw new Exception("Failed to get timetable");
        }

        var timetable = timetableResult.Result;
        var disciplineIds = timetable.Select(tt => tt.DisciplineId).Distinct();
        var departmentIds = timetable.Select(tt => tt.DepartmentId).Distinct();
        var roomIds = timetable.Select(tt => tt.RoomId).Distinct();
        var buildingIds = timetable.Select(tt => tt.BuildingId).Distinct();

        var disciplinesResult = await _disciplinesRepository.GetAllAsync(d => disciplineIds.Contains(d.DisciplineId));
        var departmentsResult = await _departmentRepository.GetAllAsync(d => departmentIds.Contains(d.DepartmentId));
        var roomsResult = await _roomRepository.GetAllAsync(r => roomIds.Contains(r.RoomId));
        var buildingResult = await _buildingRepository.GetAllAsync(b => buildingIds.Contains(b.BuildingId));

        if (!disciplinesResult.Successful)
        {
            throw new Exception("Failed to get disciplines");
        }

        if (!departmentsResult.Successful)
        {
            throw new Exception("Failed to get departments");
        }

        if (!roomsResult.Successful)
        {
            throw new Exception("Failed to get rooms");
        }

        if (!buildingResult.Successful)
        {
            throw new Exception("Failed to get rooms");
        }

        var disciplines =
            disciplinesResult.Result.ToDictionary<Disciplines, int, string>(d => d.DisciplineId, d => d.Discipline);
        var departments =
            departmentsResult.Result.ToDictionary<Departments, int, string>(d => d.DepartmentId, d => d.DepartmentName);
        var rooms = roomsResult.Result.ToDictionary<Rooms, int, string>(r => r.RoomId, r => r.Room);
        var building =
            buildingResult.Result.ToDictionary<Buildings, int, string>(b => b.BuildingId, b => b.Building);

        var fullTimeTable = timetable.Select(tt => new FullTimeTableDto
        {
            LessonId = tt.LessonId,
            DayDate = tt.DayDate,
            Number = tt.Number,
            Building = building[tt.BuildingId],
            Discipline = disciplines[tt.DisciplineId],
            DayOfWeekName = tt.DayOfWeekName,
            TimeStart = tt.TimeStart,
            TimeEnd = tt.TimeEnd,
            TimeRange = tt.TimeRange,
            IsSession = tt.IsSession,
            GroupId = tt.GroupId,
            BuildingId = tt.BuildingId,
            Room = rooms[tt.RoomId],
            Department = departments[tt.DepartmentId],
            SemesterId = tt.SemesterId,
            TypeId = tt.TypeId,
            TeacherInternalId = tt.TeacherInternalId
        });

        return fullTimeTable;
    }
    
     public async Task<IEnumerable<FullTimeTableDto>> GetFullTimeTableByTeacher(int teacherId, DateTime dayDate)
    {
        var timetableResult = await _timeTableRepository.GetTimeTableByTeacher(teacherId, dayDate);

        if (!timetableResult.Successful)
        {
            throw new Exception("Failed to get timetable");
        }

        var timetable = timetableResult.Result;
        var disciplineIds = timetable.Select(tt => tt.DisciplineId).Distinct();
        var departmentIds = timetable.Select(tt => tt.DepartmentId).Distinct();
        var roomIds = timetable.Select(tt => tt.RoomId).Distinct();
        var buildingIds = timetable.Select(tt => tt.BuildingId).Distinct();

        var disciplinesResult = await _disciplinesRepository.GetAllAsync(d => disciplineIds.Contains(d.DisciplineId));
        var departmentsResult = await _departmentRepository.GetAllAsync(d => departmentIds.Contains(d.DepartmentId));
        var roomsResult = await _roomRepository.GetAllAsync(r => roomIds.Contains(r.RoomId));
        var buildingResult = await _buildingRepository.GetAllAsync(b => buildingIds.Contains(b.BuildingId));

        if (!disciplinesResult.Successful)
        {
            throw new Exception("Failed to get disciplines");
        }

        if (!departmentsResult.Successful)
        {
            throw new Exception("Failed to get departments");
        }

        if (!roomsResult.Successful)
        {
            throw new Exception("Failed to get rooms");
        }

        if (!buildingResult.Successful)
        {
            throw new Exception("Failed to get rooms");
        }

        var disciplines =
            disciplinesResult.Result.ToDictionary<Disciplines, int, string>(d => d.DisciplineId, d => d.Discipline);
        var departments =
            departmentsResult.Result.ToDictionary<Departments, int, string>(d => d.DepartmentId, d => d.DepartmentName);
        var rooms = roomsResult.Result.ToDictionary<Rooms, int, string>(r => r.RoomId, r => r.Room);
        var building =
            buildingResult.Result.ToDictionary<Buildings, int, string>(b => b.BuildingId, b => b.Building);

        var fullTimeTable = timetable.Select(tt => new FullTimeTableDto
        {
            LessonId = tt.LessonId,
            DayDate = tt.DayDate,
            Number = tt.Number,
            Building = building[tt.BuildingId],
            Discipline = disciplines[tt.DisciplineId],
            DayOfWeekName = tt.DayOfWeekName,
            TimeStart = tt.TimeStart,
            TimeEnd = tt.TimeEnd,
            TimeRange = tt.TimeRange,
            IsSession = tt.IsSession,
            GroupId = tt.GroupId,
            BuildingId = tt.BuildingId,
            Room = rooms[tt.RoomId],
            Department = departments[tt.DepartmentId],
            SemesterId = tt.SemesterId,
            TypeId = tt.TypeId,
            TeacherInternalId = tt.TeacherInternalId
        });

        return fullTimeTable;
    }
public async Task<IEnumerable<FullTimeTableDto>> GetFullTimeTableByTeacher(int teacherId)
    {
        var timetableResult = await _timeTableRepository.GetTimeTableByTeacher(teacherId);

        if (!timetableResult.Successful)
        {
            throw new Exception("Failed to get timetable");
        }

        var timetable = timetableResult.Result;
        var disciplineIds = timetable.Select(tt => tt.DisciplineId).Distinct();
        var departmentIds = timetable.Select(tt => tt.DepartmentId).Distinct();
        var roomIds = timetable.Select(tt => tt.RoomId).Distinct();
        var buildingIds = timetable.Select(tt => tt.BuildingId).Distinct();

        var disciplinesResult = await _disciplinesRepository.GetAllAsync(d => disciplineIds.Contains(d.DisciplineId));
        var departmentsResult = await _departmentRepository.GetAllAsync(d => departmentIds.Contains(d.DepartmentId));
        var roomsResult = await _roomRepository.GetAllAsync(r => roomIds.Contains(r.RoomId));
        var buildingResult = await _buildingRepository.GetAllAsync(b => buildingIds.Contains(b.BuildingId));

        if (!disciplinesResult.Successful)
        {
            throw new Exception("Failed to get disciplines");
        }

        if (!departmentsResult.Successful)
        {
            throw new Exception("Failed to get departments");
        }

        if (!roomsResult.Successful)
        {
            throw new Exception("Failed to get rooms");
        }

        if (!buildingResult.Successful)
        {
            throw new Exception("Failed to get rooms");
        }

        var disciplines =
            disciplinesResult.Result.ToDictionary<Disciplines, int, string>(d => d.DisciplineId, d => d.Discipline);
        var departments =
            departmentsResult.Result.ToDictionary<Departments, int, string>(d => d.DepartmentId, d => d.DepartmentName);
        var rooms = roomsResult.Result.ToDictionary<Rooms, int, string>(r => r.RoomId, r => r.Room);
        var building =
            buildingResult.Result.ToDictionary<Buildings, int, string>(b => b.BuildingId, b => b.Building);

        var fullTimeTable = timetable.Select(tt => new FullTimeTableDto
        {
            LessonId = tt.LessonId,
            DayDate = tt.DayDate,
            Number = tt.Number,
            Building = building[tt.BuildingId],
            Discipline = disciplines[tt.DisciplineId],
            DayOfWeekName = tt.DayOfWeekName,
            TimeStart = tt.TimeStart,
            TimeEnd = tt.TimeEnd,
            TimeRange = tt.TimeRange,
            IsSession = tt.IsSession,
            GroupId = tt.GroupId,
            BuildingId = tt.BuildingId,
            Room = rooms[tt.RoomId],
            Department = departments[tt.DepartmentId],
            SemesterId = tt.SemesterId,
            TypeId = tt.TypeId,
            TeacherInternalId = tt.TeacherInternalId
        });

        return fullTimeTable;
    }
}