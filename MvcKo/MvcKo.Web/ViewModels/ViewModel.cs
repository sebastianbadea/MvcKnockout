using MvcKo.Model;

namespace MvcKo.Web.ViewModels
{
    public class ViewModel: IObjectWithState
    {
        public ObjectState State { get; set;}

        public ViewModel()
        {
            State = ObjectState.Unchanged;
        }
    }
}