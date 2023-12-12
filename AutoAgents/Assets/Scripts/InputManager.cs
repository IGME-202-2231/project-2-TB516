using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public void OnMouseClick(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started) AgentManager.Instance.SpawnObsticle(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
    }
}
