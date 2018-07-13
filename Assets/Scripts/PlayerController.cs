using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float BaseMoveSpeed = 1f;
    public float SprintMoveSpeed = 2f;
    public bool sprinting;

    private Animator anim;
    private Vector2 inputAxes;
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
        inputAxes.x = Input.GetAxis("Horizontal");
        inputAxes.y = Input.GetAxis("Vertical");
        sprinting = Input.GetKey(KeyCode.LeftShift);
        if (!sprinting)
        {
            speedPercent = Mathf.Lerp(speedPercent, Mathf.Clamp01(inputAxes.magnitude), 0.1f);
            moveSpeed = Mathf.Lerp(moveSpeed, BaseMoveSpeed, 0.3f);
        }
        if (sprinting)
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
            transform.position += new Vector3(inputAxes.x * moveSpeed * Time.deltaTime, 0, inputAxes.y * moveSpeed * Time.deltaTime);
            Vector3 lookAt = new Vector3(inputAxes.x, 0, inputAxes.y);
            Debug.DrawRay(transform.position, lookAt);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAt, Vector3.up), 0.25f);
            Debug.DrawRay(transform.position + Vector3.up, transform.forward);
        }
    }

    IEnumerator doSprint()
    {
        for (float t=0; t<1; t += 0.1f)
        {
            speedPercent = Mathf.SmoothStep(speedPercent, speedPercent_Max, t);
            anim.SetFloat("SpeedPercent", speedPercent);
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }


}
