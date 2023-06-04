namespace TabletopRTS.Scripts.UnitBehavior
{
    public interface IUnit
    {
        float Speed { get; }
        float Health { get; set; }
        bool IsSelected { get; set; }
    }
}