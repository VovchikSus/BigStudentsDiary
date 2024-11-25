using BigStudentsDiary.Core.Implementations;
using BigStudentsDiary.Core.Interfaces;
using BigStudentsDiary.Domain.Interfaces;
using BigStudentsDiary.Domain.Models;
using BigStudentsDiary.Infrastructure.CreationObjectFromSql;
using Microsoft.Data.SqlClient;

namespace BigStudentsDiary.Infrastructure.Repositories
{
    public class GroupsRepository : RepositoryBase, IGroupRepository
    {
        public GroupsRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IOperationResult<int>> AddGroup(Groups group)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            var id = 1;
            await this.ExecuteNonQueryAsync(
                "INSERT INTO Groups (GroupId, GroupCode) VALUES ('@id', '@groupCode')",
                new SqlParameter("@id", id),
                new SqlParameter("@groupCode", group.GroupCode)
            );

            return new Success<int>(id);
        }

        public async Task<IOperationResult> DeleteGroup(int id)
        {
            var existing = (await this.ExecuteQueryAsync<Groups, GroupCreator>(
                "SELECT * FROM Groups WHERE GroupId = @id", new SqlParameter("@id", id))).FirstOrDefault();

            if (existing == null)
                return new ElementNotFound($"Не найдена группа с id {id}");

            await this.ExecuteNonQueryAsync("DELETE FROM Groups WHERE GroupId = @id", new SqlParameter("@id", id));
            return new Success();
        }

        public async Task<IOperationResult> EditGroup(Groups group)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            var existing = (await this.ExecuteQueryAsync<Groups, GroupCreator>(
                "SELECT * FROM Groups WHERE GroupId = @id", new SqlParameter("@id", group.GroupId))).FirstOrDefault();

            if (existing == null)
                return new ElementNotFound($"Группа с id {group.GroupId} не найдена");

            await this.ExecuteNonQueryAsync(
                "UPDATE Groups SET GroupCode = '@groupCode' WHERE GroupId = @id",
                new SqlParameter("@groupCode", group.GroupCode),
                new SqlParameter("@id", group.GroupId));

            return new Success();
        }

        public async Task<IOperationResult<IEnumerable<Groups>>> GetAllAsync(Func<Groups, bool> selectFunc = null)
        {
            var result = await ExecuteQueryAsync<Groups, GroupCreator>("SELECT * FROM Groups");

            if (selectFunc == null)
            {
                return new Success<IEnumerable<Groups>>(result);
            }
            else
            {
                return new Success<IEnumerable<Groups>>(result.Where(selectFunc));
            }
        }

        public async Task<IOperationResult<Groups>> GetByGroupCodeAsync(string groupCode)
        {
            if (string.IsNullOrEmpty(groupCode))
                throw new ArgumentNullException(nameof(groupCode));

            Console.WriteLine($"Executing query for GroupCode: {groupCode}");

            var result = (await this.ExecuteQueryAsync<Groups, GroupCreator>(
                "SELECT * FROM Groups WHERE GroupCode = @groupCode",
                new SqlParameter("@groupCode", groupCode))).FirstOrDefault();

            if (result == null)
                return new ElementNotFound<Groups>($"Группа с кодом {groupCode} не найдена");

            return new Success<Groups>(result);
        }
    }
}