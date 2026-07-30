using MyFirstApi.Models;

namespace MyFirstApi.Interface
{
    public interface IItem
    {
        Task<string> GetItemsAsync();
    }
}
