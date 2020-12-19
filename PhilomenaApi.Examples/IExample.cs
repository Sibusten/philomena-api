using System.Threading.Tasks;

namespace Philomena.Api.Examples
{
    public interface IExample
    {
        string Description { get; }

        Task RunExample();
    }
}
