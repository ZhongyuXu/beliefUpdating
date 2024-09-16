using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class drawBalls : MonoBehaviour
{
    public GameObject Box;
    private void ExecuteTrigger(string trigger)
    {
        if (Box != null)
        {
            var animator = Box.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger(trigger);
            }
        }
    }

    public void OnClick(){
        ExecuteTrigger("TrDraw");
    }

}


