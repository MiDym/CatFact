using System.Threading.Tasks;

namespace CatFact
{
    public interface IApiConnector
    {
        Task<CatFactModel> GetCatFactAsync();
    }
}