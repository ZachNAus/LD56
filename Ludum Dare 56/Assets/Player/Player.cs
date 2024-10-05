using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player instance;

	[Header("Dependencies")]
	public CharacterController controller;
	public GameObject playerCamera;
	public Transform model;
	public Animator animator;
	public GameObject shieldBack; // Shield that appears in our back when we're not blocking.
	public GameObject shieldHeld; // Shield that appears when we are blocking.

	[Header("Tweakable values")]
	[Tooltip("Move speed in units per second.")]
	public float moveSpeed;
	public float jumpPower;
	public float modelRotateSpeed;
	[Tooltip("Velocity in units per second.")]
	public Vector3 velocity;
	[Tooltip("Gravity in units per second.")]
	public Vector3 gravity;
	public KeyCode blockKey;

	// Buffered input.
	private Vector3 input;
	private bool doJump;
	private bool mouse1Down, mouse1Up;
	private bool mouse2Down;
	private bool keyGDown;

	// State.
	public bool isDead; // True if we are dead.
	public bool isBlocking; // True if we are in the blocking state, i.e. our shield is out.

	// Other.
	private Holdable holdable;

	// Layer with a mask for the waist up, so we can do sword swing animations etc without affecting running animation.
	public int AnimatorLayerTorso() => animator.GetLayerIndex("Torso");
	public Transform ModelRightHand() => RecursiveFindChild(model, "mixamorig:RightHand");
	public void PlayTorso(string animationName)
	{
		animator.SetLayerWeight(AnimatorLayerTorso(), 1);
		animator.Play(animationName, AnimatorLayerTorso());
	}

	private void Awake()
	{
		instance = this;
		shieldHeld.transform.localScale = Vector3.zero;
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
		mouse1Down |= Input.GetMouseButtonDown(0);
		mouse1Up |= Input.GetMouseButtonUp(0);
		mouse2Down |= Input.GetMouseButtonDown(1);
		keyGDown |= Input.GetKeyDown(KeyCode.G);
	}

	private void FixedUpdate()
	{
		// Debug hotkey.
		if (keyGDown)
		{
			PlayerStats.instance.TakeDamage(1);
		}

		if (isDead)
		{
			// Dead state.
		}
		else if (isBlocking)
		{
			// Blocking state.
			if (!Input.GetKey(blockKey))
			{
				// TODO: Go back to normal.
				isBlocking = false;
				animator.Play("BlockEnd");
				// TODO: Move to some on state enter/exit. Do this when going to dead state too.
				shieldBack.SetActive(true);
				shieldHeld.transform.DOKill();
				shieldHeld.transform.DOScale(0f, 0.25f);
			}
		}
		else
		{
			// Normal/regular state.
			if (holdable != null)
			{
				// Holding something.
				if (mouse2Down)
				{
					DropHoldable();
				}
				// Try to use the holdable.
				else if (mouse1Down)
				{
					holdable.OnUse(true);
				}
				else if (mouse1Up)
				{
					holdable.OnUse(false);
				}
			}
			else
			{
				// Not holding anything.
				if (mouse1Down)
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
			if (IsGrounded())
			{
				if (doJump && velocity.y <= 0f)
				{
					velocity.y += jumpPower;
					animator.Play("Jump");
				}
				else if (Input.GetKey(blockKey))
				{
					// TODO: Have some way for holdables to prevent transitioning to block state?
					isBlocking = true;
					animator.SetLayerWeight(AnimatorLayerTorso(), 0);
					velocity.x = velocity.z = 0;
					shieldBack.gameObject.SetActive(false);
					shieldHeld.transform.DOKill();
					shieldHeld.transform.DOScale(1.3f, 0.25f);
					// TODO: Hide holdable?
					animator.Play("Block");
				}
			}
		}

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

		// Update animator.
		animator.SetFloat("xVelocity", velocity.x);
		animator.SetFloat("zVelocity", velocity.z);

		// Clear buffered input.
		input = Vector3.zero;
		doJump = false;
		mouse1Down = false;
		mouse1Up = false;
		mouse2Down = false;
		keyGDown = false;
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

	/// <summary>
	/// Only PlayerStats should call this.
	/// </summary>
	public void Die()
	{
		if (!isDead)
		{
			// Switch to dead state.
			isDead = true;
			velocity.x = velocity.z = 0;
			DropHoldable();
			animator.SetLayerWeight(AnimatorLayerTorso(), 0);
			animator.Play("Die");
		}
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

	private static Transform RecursiveFindChild(Transform parent, string childName)
	{
		foreach (Transform child in parent)
		{
			if(child.name == childName)
			{
				return child;
			}
			else
			{
				Transform found = RecursiveFindChild(child, childName);
				if (found != null)
				{
					return found;
				}
			}
		}
		return null;
	}
}