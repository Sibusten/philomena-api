using System.Threading.Tasks;

namespace Sibusten.Philomena.Api.Examples
{
    public interface IExample
    {
        string Description { get; }

        Task RunExample();
    }
}
