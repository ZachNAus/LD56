using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerCamera : MonoBehaviour
{
	public Transform player;
	public Vector3 mouseDelta;
	public float rotateSpeed;
	public float verticalMoveSpeed;

	private void Awake()
	{
		transform.SetParent(null);
	}

	private void OnEnable()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnDisable()
	{
		Cursor.lockState = CursorLockMode.None;
	}

	private void Update()
	{
		mouseDelta.x += Input.GetAxisRaw("Mouse X");
		mouseDelta.y += Input.GetAxisRaw("Mouse Y");
	}

	private void FixedUpdate()
	{
		transform.position = player.transform.position;
		transform.Rotate(Vector3.up, mouseDelta.x * rotateSpeed);
		
		//transform.Rotate(Vector3.right, -mouseDelta.y * rotateSpeed);

		// Odd way to do camera.
		// var child = transform.GetChild(0);
		// Vector3 childPos = child.transform.localPosition;
		// childPos.y += mouseDelta.y * verticalMoveSpeed;
		// childPos.y = Mathf.Clamp(childPos.y, 0f, 10f);
		// child.transform.localPosition = childPos;
		mouseDelta = Vector3.zero;
	}
}