using System.Threading.Tasks;

namespace CatFact
{
    public interface IFileConnector
    {
        Task SaveToFileAsync(string model);
    }
}