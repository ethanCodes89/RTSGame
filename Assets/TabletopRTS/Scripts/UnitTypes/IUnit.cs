namespace TabletopRTS.Scripts.UnitBehavior
{
    public interface IUnit
    {
        int UnitRank { get; }
        int Speed { get; }
        int Health { get; set; }
        bool IsSelected { get; set; }
        
    }
}