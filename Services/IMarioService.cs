using Web_Programming_Assignment_5.Entities;

namespace Web_Programming_Assignment_5.Services
{
    public interface IMarioService
    {
        public Task<MoveEntity?> MakeActionAsync(string action);
    }
}
