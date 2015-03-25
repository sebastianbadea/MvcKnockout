
namespace MvcKo.Model
{
    public interface IObjectWithState
    {
        ObjectState State { get; set; }
    }

    public enum ObjectState { Unchanged =0, Added, Modified, Deleted }
}
