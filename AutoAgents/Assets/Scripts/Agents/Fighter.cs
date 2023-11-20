using UnityEngine;

public class Fighter : Agent
{
    public enum FighterState
    {
        Formation,
        Combat
    }

    private FighterState _state;

    protected override void CalcSteeringForces()
    {
        switch (_state)
        {
            #region Formation
            case FighterState.Formation:

                break;
            #endregion

            #region Combat
            case FighterState.Combat:

                break;
            #endregion
        }
    }
}
