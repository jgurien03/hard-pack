using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private bool isPlayingTrick1; // To track if "trick1" animation is playing
    public SphereController sphere;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isPlayingTrick1 = false;
    }

    private void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        // Check if "trick1" animation should be played
        if (Input.GetKey(KeyCode.Z) && !sphere.getGrounded && !isPlayingTrick1)
        {
            animator.Play("trick1");
            isPlayingTrick1 = true;
            return;
        }

        // If the character is grounded, then we can play turning animations
        if (sphere.getGrounded)
        {
            isPlayingTrick1 = false;
            
            // Handle left and right turning
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                animator.Play("leanLeft");
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                animator.Play("leanRight");
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                animator.Play("leanLeftBack");
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                animator.Play("leanRightBack");
            }
            // If no turning keys are pressed and no transitional animation is playing, revert to idle
            else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                // Check if transitional animations are not playing
                if (!stateInfo.IsName("leanLeftBack") && !stateInfo.IsName("leanRightBack"))
                {
                    animator.Play("idle");
                }
            }
        }
    }
}
