using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animationcontroler : MonoBehaviour
{

    private Animator mAnimator;
    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();

       
    }

    private void Awake()
    {
        mAnimator.SetTrigger("Pegar");
    }



    // Update is called once per frame
    void Update()
    {
        if(mAnimator != null)
        {
            if(Input.GetKeyUp(KeyCode.F))
            {
                mAnimator.SetTrigger("bater");
            
            }

        }
    }
}
