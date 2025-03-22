using System;
using System.Collections;
using System.Collections.Generic;
using ThGold.Pool;
using UnityEngine;

public class Ball : MonoBehaviour, PoolObject
{
    public Vector2 direction = Vector2.right; // 移动方向，默认向右
    public float speed = 0f; // 移动速度
    public float time = 0f; // 时间接口

    private GameObjectPool Pool;
    public bool InHatchery = true;

    private void Update()
    {
        if (InHatchery)
        {
            return;
        }
        // 每帧更新位置，使小球按照指定方向和速度移动
        transform.position = (Vector2)transform.position + direction.normalized * speed * Time.deltaTime;

        // 更新时间接口
        time += Time.deltaTime;
        if (BallManager.Instance.DrawUpdate)
        {
            BallManager.Instance.Draw(transform);
        }
    }

    private void FixedUpdate()
    {
        if (BallManager.Instance.DrawFixedUpdate)
        {
            BallManager.Instance.Draw(transform);
        }
    }
    
    public void InitPoolObject(float newSpeed, GameObjectPool pool)
    {
        transform.position = new Vector3(0, 0, 0);
        speed = newSpeed;
        this.Pool = pool;
        InHatchery = false;
        StartCoroutine(DelayedCollect(2f));
    }

    private System.Collections.IEnumerator DelayedCollect(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Collect();
    }

    private void Collect()
    {
        Debug.Log("回收了");
        InHatchery = true;
        Pool.Return(this.gameObject);
        transform.position = new Vector3(0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("撞上了");
            transform.position = new Vector3(0, 0, 0);
            InHatchery = true;
            Pool.Return(this.gameObject);
        }
    }

    public void OnPreventThrough()
    {
        transform.position = new Vector3(0, 0, 0);
        InHatchery = true;
        Pool.Return(this.gameObject);
    }
    public void InitPoolObject(GameObjectPool pool)
    {
    }
}