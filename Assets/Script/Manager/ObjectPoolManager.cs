using System.Collections.Generic;
using NFT1Forge.OSY.Controller;
using NFT1Forge.OSY.DataModel;
using UnityEngine;

namespace NFT1Forge.OSY.Manager
{
    public class ObjectPoolManager : Singleton<ObjectPoolManager>
    {
        private bool m_IsInitialized = false;
        private Transform m_ObjectPoolHolder;
        private Transform m_ActiveObjectHolder;
        private readonly Dictionary<ObjectType, Transform> m_PoolDict = new Dictionary<ObjectType, Transform>();

        /// <summary>
        /// Initializing manager
        /// </summary>
        public void Initialize()
        {
            if (!m_IsInitialized)
            {
                CreateObjectHolder();
            }
        }
        /// <summary>
        /// Create transforms to hold game objects
        /// </summary>
        private void CreateObjectHolder()
        {
            GameObject pool = new GameObject();
            pool.name = "ObjectPoolHolder";
            pool.transform.SetParent(transform);
            m_ObjectPoolHolder = pool.transform;
            m_IsInitialized = true;
        }
        /// <summary>
        /// Set globat active object holder
        /// </summary>
        /// <param name="activeObjectHolder"></param>
        public void SetGlobalObjectHolder(Transform activeObjectHolder)
        {
            m_ActiveObjectHolder = activeObjectHolder;
        }
        /// <summary>
        /// 
        /// </summary>
        private void CheckInit(ObjectType objectType)
        {
            if (!m_IsInitialized)
                Initialize();

            if (!m_PoolDict.ContainsKey(objectType))
                CreatePool(objectType);
        }
        /// <summary>
        /// Get object transform from pool by type. Create new if not exist.
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public Transform GetObject(ObjectType objectType, string objectName, Transform newParent)
        {
            CheckInit(objectType);
            Transform resultTransform = m_PoolDict[objectType].Find(objectName);
            if (null == resultTransform)
            {
                resultTransform = CreateObject(objectType, objectName);
            }
            resultTransform.SetParent(newParent);
            resultTransform.localPosition = Vector3.zero;
            resultTransform.localRotation = Quaternion.identity;
            resultTransform.localScale = Vector3.one;
            resultTransform.SetActive(true);
            return resultTransform;
        }
        /// <summary>
        /// Get object by base class and make it a child of global active object transform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectType"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public T GetObject<T>(ObjectType objectType, string objectName, Transform parent = null) where T : BaseObjectController
        {
            CheckInit(objectType);

            T component;
            Transform resultObject = m_PoolDict[objectType].Find(objectName);
            if (null == resultObject)
            {
                component = CreateObjectController(objectType, objectName) as T;
            }
            else
            {
                component = resultObject.GetComponent<T>();
            }
            if (null == parent)
                component.transform.SetParent(m_ActiveObjectHolder);
            else
                component.transform.SetParent(parent);
            return component;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        private void CreatePool(ObjectType objectType)
        {
            GameObject typeHolder = new GameObject();
            typeHolder.name = objectType.ToString();
            typeHolder.transform.SetParent(m_ObjectPoolHolder);
            m_PoolDict[objectType] = typeHolder.transform;
        }
        /// <summary>
        /// Create new object
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        private Transform CreateObject(ObjectType objectType, string objectName)
        {
            Object prefab = Resources.Load(objectName);
            GameObject newObject = Instantiate(prefab, m_PoolDict[objectType]) as GameObject;
            newObject.name = objectName;
            return newObject.transform;
        }
        /// <summary>
        /// Create new object then return its BaseObjectController class
        /// </summary>
        private BaseObjectController CreateObjectController(ObjectType objectType, string objectName)
        {
            BaseObjectController prefab = Resources.Load<BaseObjectController>(objectName);
            BaseObjectController newObject = Instantiate(prefab, m_PoolDict[objectType]);
            newObject.name = objectName;
            return newObject;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnObject"></param>
        /// <param name="objectType"></param>
        public void ReturnToPool(BaseObjectController returnObject, ObjectType objectType)
        {
            returnObject.transform.SetParent(m_PoolDict[objectType]);
            returnObject.gameObject.SetActive(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnObject"></param>
        /// <param name="objectType"></param>
        public void ReturnToPool(Transform returnObject, ObjectType objectType)
        {
            returnObject.SetParent(m_PoolDict[objectType]);
            returnObject.gameObject.SetActive(false);
        }
    }
}
