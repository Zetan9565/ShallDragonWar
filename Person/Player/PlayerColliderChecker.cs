using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyWeapon")
        {
            EnemyInfoAgent enemy = other.GetComponentInParent<EnemyInfoAgent>();
            PlayerInfoManager.Instance.BeAttack(enemy, other.GetComponentInParent<EnemySkillsAgent>().currentSkill,
                Vector3.Angle(transform.forward, Vector3.ProjectOnPlane(enemy.transform.position - transform.position, Vector3.up).normalized) < 90);
        }
    }
}
