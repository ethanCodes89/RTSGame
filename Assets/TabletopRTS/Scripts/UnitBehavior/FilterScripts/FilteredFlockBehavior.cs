using TabletopRTS.UnitBehavior;

namespace TabletopRTS.Flocking
{
    public abstract class FilteredFlockBehavior : FlockBehavior
    {
        public ContextFilter filter;
    }   
}
