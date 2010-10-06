
namespace MsnHistoryCore
{
    public interface ILogger
    {
        bool ShowTime { get; }
        void Write(string text);
    }
}
