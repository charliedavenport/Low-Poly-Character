using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 5f;

    private Animator anim;
    private Vector2 inputAxes;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        inputAxes = Vector2.zero;
    }

    private void Update()
    {
        inputAxes.x = Input.GetAxis("Horizontal");
        inputAxes.y = Input.GetAxis("Vertical");
        anim.SetFloat("SpeedPercent", inputAxes.magnitude);
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


}
