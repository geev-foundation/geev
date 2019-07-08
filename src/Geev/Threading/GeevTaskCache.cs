using System.Threading.Tasks;

namespace Geev.Threading
{
    public static class GeevTaskCache
    {
        public static Task CompletedTask { get; } = Task.FromResult(0);
    }
}
