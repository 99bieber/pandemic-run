using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public enum SIDE { left, mid, right }

public class Player : MonoBehaviour
{
    public SIDE m_side = SIDE.mid;
    float newXPos = 0f;
    [HideInInspector]
    public bool swipeLeft, swipeRight, swipeUp, swipeDown;
    public float xValue;
    private CharacterController characterController;
    private CapsuleCollider capsuleCollider;
    private Animator animator;
    private float xDodge;
    public float speedDodge;
    public float jumpPower = 7f;
    private float y;
    public bool inJump, inRoll;
    public float runSpeed = 7f;
    private float colHeight;
    private float colCenterY;
    public bool alive;
    public float yDeathCondition;
    private float yTransform = 0;
    public EnergyBar energyBar;
    public ScoreManager scoreManager;
    public AudioSource audioCollect;
    public AudioSource audioDeath;
    int swipUp = 0, swipDown = 0;



    //swipe
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    // Start is called before the first frame update
    void Start()
    {
        DontDestroy.instance.gameObject.GetComponent<AudioSource>().UnPause();
        alive = true;
        characterController = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        colHeight = characterController.height;
        colCenterY = characterController.center.y;
        transform.position = new Vector3(0, 0.1f, 0);
        energyBar = GetComponent<EnergyBar>();

        //swipe
        dragDistance = Screen.height * 10 / 100; //dragDistance is 15% height of the screen
    }

    void Update()
    {

        SwipeMovement();
        KeyMovement();

        Vector3 moveVector = new Vector3(xDodge - transform.position.x, y * Time.deltaTime, runSpeed * Time.deltaTime);
        xDodge = Mathf.Lerp(xDodge, newXPos, Time.deltaTime * speedDodge);
        characterController.Move(moveVector);
        Jump();
        Roll();
    }
    public void Jump()
    {
        if (characterController.isGrounded)
        {

            // if (animator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
            // {
            //     animator.Play("Landing");
            //     inJump = false;
            // }
            inJump = false;
            if (swipeUp || swipUp == 1)
            {
                y = jumpPower;
                animator.CrossFadeInFixedTime("Jump", 0.1f);
                inJump = true;
            }
        }
        else
        {
            y -= jumpPower * 2 * Time.deltaTime;
            // if (characterController.velocity.y < -0.1f)
            //     animator.Play("Falling");
        }
        swipUp = 0;
        swipeUp = false;
    }

    internal float rollCounter;
    public void Roll()
    {
        rollCounter -= Time.deltaTime;
        if (rollCounter < 0f)
        {
            rollCounter = 0f;
            characterController.center = new Vector3(0, colCenterY, 0);
            characterController.height = colHeight;

            capsuleCollider.center = new Vector3(0, colCenterY, 0);
            capsuleCollider.height = colHeight;
            inRoll = false;
        }
        if (swipeDown && !animator.GetCurrentAnimatorStateInfo(0).IsName("Roll") || swipDown == 1 && !animator.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
        {
            rollCounter = 1f;
            y -= 10f;
            characterController.center = new Vector3(0, colCenterY / 2f, 0);
            characterController.height = colHeight / 2f;
            capsuleCollider.center = new Vector3(0, colCenterY / 2f, 0);
            capsuleCollider.height = colHeight / 2f;

            animator.CrossFadeInFixedTime("Roll", 0.1f);
            inRoll = true;
        }
        swipDown = 0;
        swipeDown = false;
    }

    public void SwipeMovement()
    {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            Debug.Log("Right Swipe");
                            if (m_side == SIDE.mid)
                            {
                                newXPos = xValue;
                                m_side = SIDE.right;
                                animator.Play("Standing Dodge Right");
                            }
                            else if (m_side == SIDE.left)
                            {
                                newXPos = 0;
                                m_side = SIDE.mid;
                                animator.Play("Standing Dodge Right");
                            }
                        }
                        else
                        {   //Left swipe
                            Debug.Log("Left Swipe");
                            if (m_side == SIDE.mid)
                            {
                                newXPos = -xValue;
                                m_side = SIDE.left;
                                animator.Play("Standing Dodge Left");
                            }
                            else if (m_side == SIDE.right)
                            {
                                newXPos = 0;
                                m_side = SIDE.mid;
                                animator.Play("Standing Dodge Left");
                            }
                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            Debug.Log("Up Swipe");
                            // swipeUp = true;
                            swipUp = 1;
                        }
                        else
                        {   //Down swipe
                            Debug.Log("Down Swipe");
                            swipDown = 1;
                        }
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                    Debug.Log("Tap");
                }
            }
        }
    }

    public void KeyMovement()
    {
        swipeLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        swipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        swipeUp = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        swipeDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
        if (swipeLeft && !inRoll)
        {
            if (m_side == SIDE.mid)
            {
                newXPos = -xValue;
                m_side = SIDE.left;
                animator.Play("Standing Dodge Left");
            }
            else if (m_side == SIDE.right)
            {
                newXPos = 0;
                m_side = SIDE.mid;
                animator.Play("Standing Dodge Left");
            }
        }
        if (swipeRight && !inRoll)
        {
            if (m_side == SIDE.mid)
            {
                newXPos = xValue;
                m_side = SIDE.right;
                animator.Play("Standing Dodge Right");
            }
            else if (m_side == SIDE.left)
            {
                newXPos = 0;
                m_side = SIDE.mid;
                animator.Play("Standing Dodge Right");
            }
        }
    }

    public void Death()
    {
        audioDeath.Play();
        this.enabled = false;
        animator.SetBool("Alive", false);
        runSpeed = 0;
        energyBar.energy = 0;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || animator.GetCurrentAnimatorStateInfo(0).IsName("Roll")
         || animator.GetCurrentAnimatorStateInfo(0).IsName("Standing Dodge Left") || animator.GetCurrentAnimatorStateInfo(0).IsName("Standing Dodge Right"))
        {
            animator.Play("Death");
        }
        if (yDeathCondition == 1)
        {
            yTransform = 1;
        }
        else
        {
            yTransform = 0.1f;

        }
        transform.position = new Vector3(transform.position.x, yTransform, transform.position.z);
        DontDestroy.instance.gameObject.GetComponent<AudioSource>().Pause();
        Invoke("LoadMenuScene", 4);

    }
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            audioDeath.Play();
            // Debug.Log("Bisa");
            yDeathCondition = 1f;
            Death();
        }
        else if (hit.gameObject.CompareTag("Virus"))
        {
            audioCollect.Play();
            energyBar.energy -= 30;
            scoreManager.score -= 500;

            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.CompareTag("Water"))
        {
            audioCollect.Play();
            energyBar.energy += 30;
            scoreManager.score += 100;
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.CompareTag("Mask"))
        {
            audioCollect.Play();
            energyBar.energy += 40;
            scoreManager.score += 200;
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.CompareTag("Sanitizer"))
        {
            audioCollect.Play();
            energyBar.energy += 40;
            scoreManager.score += 300;
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.CompareTag("Syringe"))
        {
            audioCollect.Play();
            energyBar.energy += 60;
            scoreManager.score += 500;
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.CompareTag("Vitamin"))
        {
            audioCollect.Play();
            energyBar.energy += 40;
            scoreManager.score += 300;
            Destroy(hit.gameObject);
        }
    }

}
