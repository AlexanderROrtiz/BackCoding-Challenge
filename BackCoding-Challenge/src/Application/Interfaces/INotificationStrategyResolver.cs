
namespace BackCoding.Challenge.Application.Interfaces
{
    public interface INotificationStrategyResolver
    {
        INotificationService Resolve(string type);
    }
}
