using System.Collections.Generic;
using NFT1Forge.OSY.Controller.Bullet;
using NFT1Forge.OSY.Controller.Gameplay;
using NFT1Forge.OSY.Controller.Movement;
using UnityEngine;

public class PlayerHomingMissile : BaseBullet
{
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        ((HomingMissileMovement)Moveable).OnTargetLost = FindTarget;
    }
    /// <summary>
    /// 
    /// </summary>
    private void FindTarget()
    {
        List<Transform> targetList = new List<Transform>();
        targetList.AddRange(GameplayController.I.GetAllEnemiesTransform());
        int nearestIndex = -1;
        float nearestDistance = 9999f;
        for (int i = 0; i < targetList.Count; i++)
        {
            if (null == targetList[i]) continue;
            float distance = Vector3.Distance(transform.position, targetList[i].position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestIndex = i;
            }
        }
        if (nearestIndex > -1)
            SetTarget(targetList[nearestIndex]);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        ((HomingMissileMovement)Moveable).SetTarget(target);
    }
}
