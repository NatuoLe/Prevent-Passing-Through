using UnityEngine;

namespace ThGold.Pool
{
    public interface PoolObject
    {
        /// <summary>
        /// 找到Pool
        /// </summary>
        public void InitPoolObject(GameObjectPool pool);
    }
}