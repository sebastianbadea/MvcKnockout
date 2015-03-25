using MvcKo.Model;
using System.Data.Entity;

namespace MvcKo.DataLayer
{
    public static class ContextHelpers
    {
        public static void ApplyStateChanges(this DbContext db)
        {
            foreach (var entry in db.ChangeTracker.Entries<IObjectWithState>())
            {
                var stateInfo = entry.Entity;
                entry.State = StateHelpers.ConvertState(stateInfo.State);
            }
        }
    }
}
