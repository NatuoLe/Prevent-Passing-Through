using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ThGold.Pool {
    public class GameObjectPool {
        public int initialSize;

        public List<GameObject> PoolList;
        public GameObject prefab;
        public Transform Parent;

        /// <summary>
        /// 孵化池
        /// </summary>
        public Transform Hatchery;
        public GameObjectPool(int initSize, GameObject prefab, Transform parent,Transform hatchery  = null) {
            this.prefab = prefab;
            this.Parent = parent;
            PoolList = new List<GameObject>();
            Hatchery = hatchery;
            //Debug.Log("Init" + prefab.gameObject.name);
            initialSize = initSize;
            for (int i = 0; i < initialSize; i++) {
                GameObject obj = GameObject.Instantiate(prefab);
                obj.gameObject.SetActive(false);
                if (Hatchery)
                {
                    obj.transform.SetParent(Hatchery);
                }
                else
                {
                    obj.transform.SetParent(parent);
                }
               
                PoolList.Add(obj);
            }
        }

        public virtual GameObject Get() {
            foreach (GameObject obj in PoolList) {
                if (!obj.gameObject.activeSelf) {
                    obj.transform.SetParent(Parent);
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }

            GameObject newObj = GameObject.Instantiate(prefab,Hatchery);
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(Parent);
            PoolList.Add(newObj);

            return newObj;
        }

        public virtual void Return(GameObject obj) {
            obj.gameObject.SetActive(false);
            PoolList.Remove(obj);
            PoolList.Add(obj);
            if (Hatchery)
            {
                obj.transform.SetParent(Hatchery);
            }
        }

        public List<GameObject> GetAllObjects() {
            return PoolList;
        }

        public void ReturnAll()
        {
            foreach (var t in PoolList)
            {
                Return(t);
            }
        }
    }
}