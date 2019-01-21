using Refit;
using System.Threading.Tasks;

namespace bitwarden_hibp
{
    [Headers("User-Agent: Bitwarden-export-password-checker")]
    public interface IHaveIBeenPawnedApi
    {
        [Get("/range/{hash}")]
        Task<string> SearchByRange(string hash);
    }
}
