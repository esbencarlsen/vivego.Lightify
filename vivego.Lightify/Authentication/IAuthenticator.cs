using System.Threading.Tasks;

namespace vivego.Lightify.Authentication
{
	public interface IAuthenticator
	{
		Task<string> GetAuthToken();
	}
}