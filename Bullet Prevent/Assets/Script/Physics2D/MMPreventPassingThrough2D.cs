/// <summary>
/// 通过在每次移动后向后投射射线，防止快速移动的物体穿过碰撞器
/// </summary>

using UnityEngine;

[AddComponentMenu("More Mountains/Tools/Movement/MMPreventPassingThrough2D")]
public class MMPreventPassingThrough2D : MonoBehaviour
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

    private float blueGizmoTime = 0f;
    private Vector3 bluePos;
    private Ball _ball;
    /// <summary>
    /// 在 Start 中初始化对象
    /// </summary>
    protected virtual void Start()
    {
        Initialization();
    }

    private void OnEnable()
    {
        blueGizmoTime = 0;
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
        _positionLastFrame = _rigidbody.position;

        // 计算碰撞体的最小边界宽度
        float smallestBoundsWidth = Mathf.Min(_collider.bounds.extents.x, _collider.bounds.extents.y);
        _adjustedSmallestBoundsWidth = smallestBoundsWidth * (1.0f - SkinWidth);
        _squaredBoundsWidth = smallestBoundsWidth * smallestBoundsWidth;
    }
    void OnDrawGizmos()
    {
        if (blueGizmoTime > 0.0f)
        {
            Gizmos.color = Color.blue; // 设置点的颜色
            Gizmos.DrawSphere(bluePos, 0.25f); // 绘制点
        }
    }
    /// <summary>
    /// 在 Update 中检查上一次移动距离，如果需要则投射射线检测障碍物
    /// </summary>
    protected virtual void Update()
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
            Debug.DrawRay(_positionLastFrame, _lastMovement.normalized * movementMagnitude, Color.red);
            // 计算终点
            Vector2 endpoint;
            if (hitInfo.collider != null)
            {
                // 如果射线击中物体，终点是击中的点
                endpoint = hitInfo.point;
                point.DrawPoint(hitInfo.point,3f);
                blueGizmoTime = 5f;
                bluePos = new Vector3(transform.position.x,transform.position.y);
                line.DrawLine(_positionLastFrame,_lastMovement.normalized * movementMagnitude,Color.red, 3);
            }
            else
            {
                // 如果射线没有击中物体，终点是射线的最远端
                endpoint = (Vector2)_positionLastFrame + _lastMovement.normalized * movementMagnitude;
            }

   
            
            if (hitInfo.collider != null)
            {
                Debug.Log("<color=green>发生穿透</color>");
                lastCollisionTime = Time.time;

                // 将对象放置在碰撞器的外面一点
                Vector2 adjustedPosition = hitInfo.point + hitInfo.normal * _adjustedSmallestBoundsWidth * 2;
                _rigidbody.MovePosition(adjustedPosition); // 使用 MovePosition 确保物理引擎正确处理

                /*// 调用自定义的 OnTriggerEnter2D 方法
                hitInfo.collider.GetComponent<IObstacle>().OnPassingThrough2D(GetComponent<Ball>(), hitInfo);*/
                _ball.OnPreventThrough();
            }
        }

        _positionLastFrame = this.transform.position;
    }
}