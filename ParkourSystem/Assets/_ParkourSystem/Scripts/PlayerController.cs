using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private CameraController _cameraController;

    private Quaternion targetRotation;
    [SerializeField] private float playerRotationSpeed = 500f;

    [SerializeField]private Animator _animator;
    private void Awake()
    {
        if (Camera.main != null) _cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v= Input.GetAxisRaw("Vertical");
        Vector3 moveInput = new Vector3(h, 0, v).normalized;

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));
        
        var moveDir = _cameraController.PlanarRotation * moveInput;

        if (moveAmount > 0)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            
            //player face in move direction
            targetRotation = Quaternion.LookRotation(moveDir); 
        }
        
        //smoothly rotate player towards move direction
        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, targetRotation, playerRotationSpeed * Time.deltaTime);
        
        //blend tree value
        _animator.SetFloat("moveAmount",moveAmount, 0.2f, Time.deltaTime);
    }
}
