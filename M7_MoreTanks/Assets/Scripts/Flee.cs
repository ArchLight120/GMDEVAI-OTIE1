using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : NPCBaseFSM
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Health hp = NPC.GetComponent<Health>();
        if (hp != null && hp.currentHP / hp.maxHP < 0.5f)
        {
            Vector3 dir = NPC.transform.position - opponent.transform.position;
            Vector3 fleeTarget = NPC.transform.position + dir.normalized * 5.0f;

            NPC.transform.rotation = Quaternion.Slerp(NPC.transform.rotation,
                Quaternion.LookRotation(fleeTarget - NPC.transform.position),
                rotSpeed * Time.deltaTime);

            NPC.transform.position += NPC.transform.forward * speed * Time.deltaTime;
        }
    }
}

