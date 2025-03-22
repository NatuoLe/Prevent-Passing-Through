/// <summary>
/// 通过在每次移动后向后投射射线，防止快速移动的物体穿过碰撞器
/// </summary>

using UnityEngine;

public class PreventPassingThrough2D : MonoBehaviour
{
    public LayerMask ObstaclesLayerMask; // 用于检测障碍物的图层掩码
    public float SkinWidth = 0.1f; // 调整碰撞体边界的变量
    public static float cooldownTime = 0.4f; // 冷却时间，避免重复检测
    private float lastCollisionTime = -cooldownTime; // 上一次碰撞的时间

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private Vector3 _positionLastFrame;
    private Vector2 _lastMovement;
    private Vector2 _lastBackVector;
    private float _lastMovementSquared;
    private float _squaredBoundsWidth;
    private float _adjustedSmallestBoundsWidth;

    private PersistentGizmoLine line;
    private GizmoPoint point;
    
    private Vector3 bluePos;
    private Ball _ball;
    private Vector3 end;
    /// <summary>
    /// 在 Start 中初始化对象
    /// </summary>
    protected virtual void Start()
    {
        Initialization();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
       
    }
    private void OnEnable()
    {
        _positionLastFrame = this.transform.position;
    }
    /// <summary>
    /// 获取 Rigidbody 并计算碰撞体的边界宽度
    /// </summary>
    protected virtual void Initialization()
    {
        point = GetComponent<GizmoPoint>();
        line = GetComponent<PersistentGizmoLine>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _ball = GetComponent<Ball>();
        _positionLastFrame = transform.position;

        // 计算碰撞体的最小边界宽度
        float smallestBoundsWidth = Mathf.Min(_collider.bounds.extents.x, _collider.bounds.extents.y);
        _adjustedSmallestBoundsWidth = smallestBoundsWidth * (1.0f - SkinWidth);
        _squaredBoundsWidth = smallestBoundsWidth * smallestBoundsWidth;
    }
    /// <summary>
    /// 在 Update 中检查上一次移动距离，如果需要则投射射线检测障碍物
    /// </summary>
    protected virtual void Update()
    {
        if (BallManager.Instance.DrawUpdate)
        {
            if (_ball.InHatchery)
            {
                return;
            }
            _lastMovement = this.transform.position - _positionLastFrame;
            _lastMovementSquared = _lastMovement.sqrMagnitude;

            // 如果移动距离大于碰撞体的边界宽度，可能会遗漏一些障碍物
            if (_lastMovementSquared > _squaredBoundsWidth)
            {
                float movementMagnitude = Mathf.Sqrt(_lastMovementSquared);// * (float)GetComponent<Ball>().ballData.Speed
                RaycastHit2D hitInfo = Physics2D.Raycast(_positionLastFrame, _lastMovement.normalized, movementMagnitude, ObstaclesLayerMask);
                // 画出射线的检测范围
                Vector2 end = (Vector2)_positionLastFrame + _lastMovement.normalized * movementMagnitude;
                Debug.DrawLine(_positionLastFrame, end, Color.cyan);
                if (hitInfo.collider != null)
                {
                    Debug.Log("<color=green>发生穿透</color>");
                    lastCollisionTime = Time.time;
                
                    BallManager.Instance.DrawPrevent(hitInfo,_positionLastFrame,_lastMovement.normalized * movementMagnitude);
                    _ball.OnPreventThrough();
                }
            }

            _positionLastFrame = this.transform.position;
        }
    }
    protected virtual void FixedUpdate()
    {
        if (BallManager.Instance.DrawFixedUpdate)
        {
            if (_ball.InHatchery)
            {
                return;
            }
            _lastMovement = this.transform.position - _positionLastFrame;
            _lastMovementSquared = _lastMovement.sqrMagnitude;

            // 如果移动距离大于碰撞体的边界宽度，可能会遗漏一些障碍物
            if (_lastMovementSquared > _squaredBoundsWidth)
            {
                float movementMagnitude = Mathf.Sqrt(_lastMovementSquared);// * (float)GetComponent<Ball>().ballData.Speed
                RaycastHit2D hitInfo = Physics2D.Raycast(_positionLastFrame, _lastMovement.normalized, movementMagnitude, ObstaclesLayerMask);
                // 画出射线的检测范围
                Vector2 end = (Vector2)_positionLastFrame + _lastMovement.normalized * movementMagnitude;
                Debug.DrawLine(_positionLastFrame, end, Color.cyan);
                if (hitInfo.collider != null)
                {
                    Debug.Log("<color=green>发生穿透</color>");
                    lastCollisionTime = Time.time;
                
                    BallManager.Instance.DrawPrevent(hitInfo,_positionLastFrame,_lastMovement.normalized * movementMagnitude);
                    _ball.OnPreventThrough();
                }
            }

            _positionLastFrame = this.transform.position;
        }
    }
}