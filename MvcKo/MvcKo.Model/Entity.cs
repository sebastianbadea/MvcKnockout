
namespace MvcKo.Model
{
    public class Entity: IObjectWithState
    {
        public ObjectState State { get; set; }

        public Entity()
        {
            State = ObjectState.Unchanged;
        }
    }
}
