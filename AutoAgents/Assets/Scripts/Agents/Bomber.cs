using UnityEngine;

public class Bomber : Agent
{
    public enum BomberState
    {
        Formation,
        Scatter
    }

    [Range(0, 100)][SerializeField] float _coheasionWeight = 0;
    [Range(0, 100)][SerializeField] float _alignmentWeight = 0;

    [SerializeField] private BomberState _state;

    public BomberState State
    {
        get => _state;
        set { _state = value; }
    }

    protected override void CalcSteeringForces()
    {
        switch (_state)
        {
            #region Formation
            case BomberState.Formation:
                _totalForce += Wander(weight:_wanderWeight);
                _totalForce += Seperate(_team, _seperateWeight);
                _totalForce += Cohesion(_coheasionWeight);
                _totalForce += Alignment(_alignmentWeight);
                break;
            #endregion

            #region Scatter
            case BomberState.Scatter:
                _totalForce += Flee(FindClosestEnemy().GetFuturePosition(2));
                break;
            #endregion
        }
        _totalForce += AvoidObsticles(weight: 1000);
        _totalForce += StayInBounds(weight: _boundsForce);
    }
}