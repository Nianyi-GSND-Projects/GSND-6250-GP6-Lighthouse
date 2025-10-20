using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Game
{
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(PlayerInput))]
	public class Player : MonoBehaviour
	{
		#region Unity life cycle
		void OnEnable()
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
		void OnDisable()
		{
			Cursor.lockState = CursorLockMode.None;
		}

		void Update()
		{
			UpdateFocusedInteraction();
		}
		void LateUpdate()
		{
			UpdateInteractionIndicator();
		}
		void FixedUpdate()
		{
			float dt = Time.fixedDeltaTime;
			if(bufferMovementInput.sqrMagnitude > 0.1f)
			{
				Vector3 worldVelocity = transform.localToWorldMatrix.MultiplyVector(bufferMovementInput).normalized * moveSpeed;
				MoveContrained(transform.position + worldVelocity * dt);
			}
		}
		#endregion

		#region Component references
		[SerializeField] Transform eye;

		CharacterController controller;
		CharacterController Controller
		{
			get
			{
				if(controller == null)
					controller = GetComponent<CharacterController>();
				return controller;
			}
		}
		#endregion

		#region Control
		[SerializeField][Range(0, 10)] float moveSpeed = 3.0f;
		Vector3 bufferMovementInput = default;

		void MoveContrained(Vector3 targetPos)
		{
			// Limit the player's position on movable areas.
			if(!NavMesh.SamplePosition(targetPos, out var hit, 5f, 1))
				return;
			targetPos = hit.position;

			Controller.Move(targetPos - transform.position);
		}

		[SerializeField][Range(0, 1)] float orientSpeed = 1.0f;
		float Azimuth
		{
			get => transform.eulerAngles.y;
			set
			{
				var euler = transform.eulerAngles;
				euler.y = value;
				transform.eulerAngles = euler;
			}
		}
		float Zenith
		{
			get => eye.eulerAngles.x;
			set
			{
				var euler = eye.eulerAngles;
				euler.x = value;
				eye.eulerAngles = euler;
			}
		}

		protected void OnInputMoveDirection(InputValue value)
		{
			var raw = value.Get<Vector2>();
			bufferMovementInput = new Vector3(raw.x, 0, raw.y);
		}

		protected void OnInputOrientDelta(InputValue value)
		{
			var raw = value.Get<Vector2>();
			Azimuth = Azimuth + raw.x * orientSpeed;
			float zenith = Zenith + raw.y * orientSpeed;
			if(zenith < 0)
				zenith += 360;
			if(zenith < 180)
				zenith = Mathf.Clamp(zenith, 0, 90);
			else
				zenith = Mathf.Clamp(zenith, 270, 360);
			Zenith = zenith;
		}
		#endregion

		#region Interaction
		Interactable focusedInteraction = null;
		Interactable FocusedInteraction
		{
			get => focusedInteraction;
			set
			{
				if(value != null && !value.enabled)
					value = null;
				focusedInteraction = value;
			}
		}
		[SerializeField][Min(0)] float maxInteractDistance = 5.0f;
		[SerializeField] RectTransform interactionIndicator;
		[SerializeField] Text interactionIndicatorText;

		void UpdateFocusedInteraction()
		{
			if(!Physics.Raycast(eye.position, eye.forward, out var hit, maxInteractDistance))
			{
				FocusedInteraction = null;
				return;
			}
			FocusedInteraction = hit.transform.gameObject.GetComponentInParent<Interactable>();
		}
		void UpdateInteractionIndicator()
		{
			if(FocusedInteraction)
			{
				interactionIndicator.gameObject.SetActive(true);
				interactionIndicatorText.text = FocusedInteraction.text;
			}
			else
			{
				interactionIndicator.gameObject.SetActive(false);
				interactionIndicatorText.text = string.Empty;
			}
		}
		protected void OnInteract()
		{
			if(!FocusedInteraction)
				return;
			FocusedInteraction.Interact();
		}
		#endregion
	}
}
