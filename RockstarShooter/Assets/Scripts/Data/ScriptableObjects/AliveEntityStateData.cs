using Characters.Player.Data.States.Airborne;
using Characters.Player.Data.States.Grounded;
using UnityEngine;

namespace Characters.Player.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AliveEntityStateData", menuName = "StateMachine/StateData")]
    public class AliveEntityStateData : ScriptableObject
    {
        [field: SerializeField] public PlayerGroundedData GroundedData { get; private set; }
        [field: SerializeField] public AliveEntityAirborneData AirborneData { get; private set; }
    }
}