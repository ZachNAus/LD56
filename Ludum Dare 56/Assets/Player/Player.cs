using System;
using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player instance;

	public CharacterController controller;
	public GameObject playerCamera;
	public Transform model;

	[Tooltip("Move speed in units per second")]
	public float moveSpeed;
	public float jumpPower;
	public float modelRotateSpeed;
	// Velocity in units per second. Can be set to whatever.
	public Vector3 velocity;
	// Gravity in units per second.
	public Vector3 gravity;

	public Vector3 input;
	public bool doJump;

	private Holdable holdable;

	private void Awake()
	{
		instance = this;
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void Start()
	{
		// Temporary.
		SetHoldable(GetComponentInChildren<Holdable>());
	}

	private void Update()
	{
		input.x = Input.GetAxisRaw("Horizontal");
		input.z = Input.GetAxisRaw("Vertical");
		doJump |= Input.GetKeyDown(KeyCode.Space);

		if (holdable != null)
		{
			// Holding something.
			if (Input.GetMouseButtonDown(1))
			{
				DropHoldable();
			}
			// Try to use the holdable.
			else if (Input.GetMouseButtonDown(0))
			{
				holdable.OnUse(true);
			}
			else if (Input.GetMouseButtonUp(0))
			{
				holdable.OnUse(false);
			}
		}
		else
		{
			// Not holding anything.
			if (Input.GetMouseButtonDown(0))
			{
				// Look for a holdable pickup.
				// TODO: Better casting.
				var cast = Physics.SphereCastAll(transform.position, 2f, Vector3.down, 0.1f);
				foreach (var c in cast)
				{
					var pickup = c.transform.GetComponentInParent<Pickup>();
					var hold = pickup?.Pick();
					if (hold != null)
					{
						// Found something to hold.
						Destroy(pickup.gameObject);
						SetHoldable(hold);
						break;
					}
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.G))
		{
			PlayerStats.instance.TakeDamage(1);
		}
	}

	private void FixedUpdate()
	{
		// Apply lateral movement (side and forwards).
		var moveDir = new Vector3(input.x, 0, input.z);
		// Move relative to the camera.
		moveDir = playerCamera.transform.TransformDirection(moveDir);
		moveDir.y = 0;
		moveDir.Normalize();
		velocity.x = moveDir.x * moveSpeed;
		velocity.z = moveDir.z * moveSpeed;
		if (moveDir.sqrMagnitude != 0)
		{
			// Rotate player to direction of camera.
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), modelRotateSpeed * Time.deltaTime);
		}

		// Apply jumping.
		if (doJump && velocity.y <= 0f && IsGrounded())
		{
			velocity.y += jumpPower;
		}
		doJump = false;

		// Apply gravity.
		velocity += gravity;
		var collision = controller.Move(velocity * Time.deltaTime);
		// Collision detection.
		if (collision.HasFlag(CollisionFlags.Below))
		{
			velocity.y = 0;
		}
		if (collision.HasFlag(CollisionFlags.Sides))
		{
			velocity.x = velocity.y = 0;
		}
	}

	/// <summary>
	/// Returns true if the player is on the ground.
	/// </summary>
	public bool IsGrounded()
	{
		return controller.isGrounded;
	}

	public void DropHoldable()
	{
		SetHoldable(null);
	}

	public void SetHoldable(Holdable h)
	{
		if (holdable != null)
		{
			// Remove old holdable.
			holdable.OnExit();
			holdable.Drop();
			Destroy(holdable.gameObject);
		}
		if (h != null)
		{
			h.transform.SetParent(transform, false);
			h.player = this;
		}
		holdable = h;
		holdable?.OnEnter();
	}

	public static bool IsPlayer(GameObject collision)
	{
		return TryGetPlayer(collision, out var _);
	}

	public static bool TryGetPlayer(GameObject collision, out Player player)
	{
		player = collision.GetComponentInParent<Player>();
		return player != null;
	}
}