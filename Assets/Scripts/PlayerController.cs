using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public CameraController cam;
    public float BaseMoveSpeed = 1f;
    public float SprintMoveSpeed = 2f;
    public bool moving;
    public bool sprinting;
    public bool rolling;
    public float rollBoostPercent = 1.5f;

    private Animator anim;
    private Vector2 inputAxes;
    private Vector3 locomotion;
    public bool moveKeys; // true when any W,A,S,D keys are pressed
    public float moveSpeed;
    public float rollMoveSpeed;
    private float speedPercent;
    private const float speedPercent_Max = 2f;

    private void Awake()
    {
        moveSpeed = BaseMoveSpeed;
        anim = GetComponent<Animator>();
        inputAxes = Vector2.zero;
        moving = false;
        sprinting = false;
        rolling = false;
    }

    private void Update()
    {
        // get player input
        moveKeys = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        inputAxes.x = Input.GetAxis("Horizontal");
        inputAxes.y = Input.GetAxis("Vertical");
        // player doesn't walk faster diagonally
        if (inputAxes.magnitude > 1) inputAxes = inputAxes.normalized; 

        moving = inputAxes.magnitude > 0.05f; // deadzone

        // set moveSpeed and speedPercent floats 
        sprinting = Input.GetKey(KeyCode.LeftShift) && moving;
        if (Input.GetKeyDown(KeyCode.Space) && !rolling && moving && moveKeys) 
        {
            anim.SetTrigger("RollTrigger");
            rolling = true;
            //Debug.Log("START ROLL");
        }
        if (!sprinting)
        {
            rollMoveSpeed = moveSpeed + rollBoostPercent * anim.GetFloat("RollBoostAmt");
            speedPercent = Mathf.Lerp(speedPercent, Mathf.Clamp01(inputAxes.magnitude), 0.1f);
            moveSpeed = Mathf.Lerp(moveSpeed, BaseMoveSpeed, 0.3f);
        }
        else
        {
            rollMoveSpeed = moveSpeed + rollBoostPercent * anim.GetFloat("RollBoostAmt");
            //speedPercent = Mathf.Clamp01(inputAxes.magnitude);
            speedPercent = Mathf.Lerp(speedPercent, 2 , 0.1f);
            moveSpeed = Mathf.Lerp(moveSpeed, SprintMoveSpeed, 0.3f);
        }
        anim.SetFloat("SpeedPercent", speedPercent);

    }

    private void FixedUpdate()
    {
        if (moving)
        {
            if (rolling)
            {
                locomotion = new Vector3(inputAxes.x, 0, inputAxes.y) * rollMoveSpeed * Time.deltaTime;
                //Debug.Log(moveSpeed);
            }
            else
            {
                //float rollMoveSpeed = moveSpeed + 5 * anim.GetFloat("RollBoostAmt");
                locomotion = new Vector3(inputAxes.x, 0, inputAxes.y) * moveSpeed * Time.deltaTime;
                //Debug.Log(rollMoveSpeed + "  " + anim.GetFloat("RollBoostAmt"));
            }
            locomotion = cam.lookDirection * locomotion;
            transform.position += locomotion;
            Vector3 lookAt = new Vector3(inputAxes.x, 0, inputAxes.y);
            lookAt = cam.lookDirection * lookAt;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAt, Vector3.up), 0.1f); // 0.1 = turn speed

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + 0.75f * Vector3.up, 10f * locomotion);
        Gizmos.color = Color.white;
        //Gizmos.DrawRay(transform.position + 0.1f * Vector3.up, new Vector3(inputAxes.x, 0f, inputAxes.y) * 0.25f);
    }

    public void EndRoll()
    {
        rolling = false;
        //Debug.Log("END ROLL");
    }

 



}
