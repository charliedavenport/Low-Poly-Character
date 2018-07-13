using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public CameraController cam;
    public float BaseMoveSpeed = 1f;
    public float SprintMoveSpeed = 2f;
    public bool sprinting;

    private Animator anim;
    private Vector2 inputAxes;
    private Vector3 locomotion;
    public float moveSpeed;
    private float speedPercent;
    private const float speedPercent_Max = 2f;

    private void Awake()
    {
        moveSpeed = BaseMoveSpeed;
        anim = GetComponent<Animator>();
        inputAxes = Vector2.zero;
        sprinting = false;
    }

    private void Update()
    {
        // get player input
        inputAxes.x = Input.GetAxis("Horizontal");
        inputAxes.y = Input.GetAxis("Vertical");
        // set moveSpeed and speedPercent floats 
        sprinting = Input.GetKey(KeyCode.LeftShift) && (inputAxes.magnitude > 0);
        if (!sprinting)
        {
            speedPercent = Mathf.Lerp(speedPercent, Mathf.Clamp01(inputAxes.magnitude), 0.1f);
            moveSpeed = Mathf.Lerp(moveSpeed, BaseMoveSpeed, 0.3f);
        }
        else
        {
            //speedPercent = Mathf.Clamp01(inputAxes.magnitude);
            speedPercent = Mathf.Lerp(speedPercent, 2 , 0.1f);
            moveSpeed = Mathf.Lerp(moveSpeed, SprintMoveSpeed, 0.3f);
        }
        anim.SetFloat("SpeedPercent", speedPercent);


    }

    private void FixedUpdate()
    {
        if (inputAxes.magnitude > 0)
        {
            locomotion = new Vector3(inputAxes.x, 0, inputAxes.y) * moveSpeed * Time.deltaTime;
            locomotion = cam.lookDirection * locomotion;
            transform.position += locomotion;
            Vector3 lookAt = new Vector3(inputAxes.x, 0, inputAxes.y);
            lookAt = cam.lookDirection * lookAt;
            Debug.DrawRay(transform.position, lookAt);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAt, Vector3.up), 0.25f); // 0.25 = turn speed
            Debug.DrawRay(transform.position + Vector3.up, transform.forward);
        }
    }



}
