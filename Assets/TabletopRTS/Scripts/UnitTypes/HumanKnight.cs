using UnityEngine;

namespace TabletopRTS.UnitBehavior
{
    public class HumanKnight : MonoBehaviour, IUnit
    {
        [SerializeField] 
        private int unitRank;
        [SerializeField]
        private int health;
        [SerializeField]
        private int speed;
        private bool isSelected;
    
        public int UnitRank { get { return unitRank; } }
        public int Health { get { return health;  } set { health = value; } } 
        public int Speed { get { return speed;  } set { speed = value; } }
        public bool IsSelected { get { return isSelected;  } set { isSelected = value; } }
    }   
}
