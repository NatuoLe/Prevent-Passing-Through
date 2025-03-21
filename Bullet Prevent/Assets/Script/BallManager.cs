using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using ThGold.Pool;
using UnityEngine;

public class BallManager : MonoBehaviour
{
   private GameObjectPool ballPool;
   public GameObject ball;

   private void Start()
   {
      ballPool = new GameObjectPool(10, ball, this.transform);
   }
   [Button("Shoot")]
   public void Shoot(float speed)
   {
      Ball _ball = ballPool.Get().GetComponent<Ball>();
      _ball.transform.position = new Vector3(0, 0, 0);
      _ball.InitPoolObject(speed,ballPool);
   }
}
