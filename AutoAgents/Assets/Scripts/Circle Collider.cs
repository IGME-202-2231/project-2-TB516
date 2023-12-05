using UnityEngine;

public class CircleCollider : MonoBehaviour
{
    [SerializeField] protected float _circleRadius = 2;

    [SerializeField] protected Vector3 _circleHitboxOffset;

    protected Vector3 _circlePos;

    public Vector3 CirclePos
    {
        get => _circlePos;
        set
        {
            _circlePos = value;
        }
    }

    public float Radius => _circleRadius;

    protected void Start()
    {
        _circlePos = transform.position - _circleHitboxOffset;
    }

    protected void Update()
    {
        _circlePos = transform.position - _circleHitboxOffset;
    }

    public virtual bool IsCollidingWith(CircleCollider entity)
    {
        return (_circlePos - entity._circlePos).magnitude <= _circleRadius + entity._circleRadius;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_circlePos, _circleRadius);
    }
}
