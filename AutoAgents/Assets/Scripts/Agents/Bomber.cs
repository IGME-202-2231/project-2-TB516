using UnityEngine;

public class Bomber : Agent
{
    public enum BomberState
    {
        Formation,
        Scatter
    }

    [Range(.2f, 50)][SerializeField] private float _seperateWeight = .2f;

    private BomberState _state;
    private AgentManager.Team _team;

    public AgentManager.Team Team
    {
        get => _team;
        set
        {
            _team = value;
        }
    }

    protected override void CalcSteeringForces()
    {
        switch (_state)
        {
            #region Formation
            case BomberState.Formation:

                _totalForce += Wander();
                _totalForce += Seperate(_team, _seperateWeight);
                _totalForce += StayInBounds(weight:_boundsForce);
                
                break;
            #endregion

            #region Scatter
            case BomberState.Scatter:
                break;
            #endregion
        }
        _totalForce += AvoidObsticles();
    }
}