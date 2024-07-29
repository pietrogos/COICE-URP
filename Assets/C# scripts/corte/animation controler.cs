using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animationcontroler : MonoBehaviour
{
    private Animator mAnimator;
    public float cooldownTime = 5.0f; // Cooldown period in seconds
    private bool isCooldown = false; // Flag to check if cooldown is active

    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mAnimator != null)
        {
            if (Input.GetKeyUp(KeyCode.F) && !isCooldown)
            {
                mAnimator.SetTrigger("bater");
                StartCoroutine(TriggerCooldown());
            }
        }
    }

    private IEnumerator TriggerCooldown()
    {
        isCooldown = true; // Start the cooldown
        yield return new WaitForSeconds(cooldownTime); // Wait for cooldown period
        isCooldown = false; // End the cooldown
    }
}
