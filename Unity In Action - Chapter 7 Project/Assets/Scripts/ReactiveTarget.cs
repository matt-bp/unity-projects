using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
    public void ReactToHit()
    {
        var behavior = GetComponent<WanderingAI>();
        if (behavior != null)
        {
            behavior.IsAlive = false;
        }
        
        StartCoroutine(Die());
    }

    private IEnumerator<YieldInstruction> Die()
    {
        this.transform.Rotate(-75, 0, 0);

        yield return new WaitForSeconds(1.5f);

        Destroy(this.gameObject);
    }
}