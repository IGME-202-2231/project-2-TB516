using UnityEngine;

public class Bomber : Agent
{
    public enum BomberState
    {
        Formation,
        Scatter
    }

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
                _totalForce += Seperate(_team, 2);
                _totalForce += StayInBounds();
                
                break;
            #endregion

            #region Scatter
            case BomberState.Scatter:
                break;
            #endregion
        }
    }
}
