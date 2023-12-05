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

    private BomberState _state;

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
                _totalForce += Seperate(_team, 1000);
                _totalForce += Seperate((AgentManager.Team)((((int)_team)+1) % 2), 1000);
                break;
            #endregion
        }
        _totalForce += AvoidObsticles();
        _totalForce += StayInBounds(weight: _boundsForce);
    }
}