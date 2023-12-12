using UnityEngine;

public class Fighter : Agent
{
    public enum FighterState
    {
        Formation,
        Combat
    }

    private FighterState _state;

    public FighterState State
    {
        get => _state;
        set { _state = value; }
    }

    [Range(0, 100)][SerializeField] float _coheasionWeight = 0;


    protected override void CalcSteeringForces()
    {
        switch (_state)
        {
            #region Formation
            case FighterState.Formation:
                _totalForce += Wander(weight:_wanderWeight);
                _totalForce += Seperate(_team, _seperateWeight);
                _totalForce += Cohesion(_coheasionWeight);
                break;
            #endregion

            #region Combat
            case FighterState.Combat:
                _totalForce += Seek(FindClosestEnemy().GetFuturePosition(2));
                break;
            #endregion
        }

        _totalForce += AvoidObsticles(weight: 1000);
        _totalForce += StayInBounds(weight: _boundsForce);
    }
}
