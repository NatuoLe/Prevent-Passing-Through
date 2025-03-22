using System;
using System.Collections;
using System.Collections.Generic;
using ThGold.Pool;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public static BallManager Instance;
    private GameObjectPool ballPool;
    private GameObjectPool pointPool;
    [SerializeField] private PersistentGizmoLine line;
    [SerializeField] private GizmoPoint PreventPoint;
    public GameObject ball;

    public GameObject point;

    public bool DrawUpdate,DrawFixedUpdate;


    private void Start()
    {
        Instance = this;
        ballPool = new GameObjectPool(10, ball, this.transform);
        pointPool = new GameObjectPool(10, point, this.transform);
    }
    
    public void Shoot100(float speed)
    {
        pointPool.ReturnAll();
        Ball _ball = ballPool.Get().GetComponent<Ball>();
        _ball.transform.position = new Vector3(0, 0, 0);
        _ball.InitPoolObject(100, ballPool);
    }
    public void Shoot10(float speed)
    {
        pointPool.ReturnAll();
        Ball _ball = ballPool.Get().GetComponent<Ball>();
        _ball.transform.position = new Vector3(0, 0, 0);
        _ball.InitPoolObject(10, ballPool);
    }
    public void Draw(Transform transform)
    {
        GizmoPoint point = pointPool.Get().GetComponent<GizmoPoint>();
        point.Inited = false;
        point.DrawPoint(transform.position, 99f);
        point.InitPoolObject(pointPool);
        point.Inited = true;
    }

    public void DrawPrevent(RaycastHit2D hitInfo, Vector3 start, Vector3 end)
    {
        PreventPoint.color = Color.yellow;
        PreventPoint.DrawPoint(hitInfo.point, 15f);
        line.DrawLine(start, end, Color.yellow, 15);
        Debug.Log("射线命中:" + hitInfo.collider.gameObject.name);
    }
}