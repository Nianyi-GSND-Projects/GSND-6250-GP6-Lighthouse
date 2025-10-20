using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Game
{
	public class Interactable : MonoBehaviour
	{
		public string text;
		[SerializeField] bool oneTime;
		[SerializeField][Min(0)] float delay = 0;
		[SerializeField] UnityEvent onInteract;

		public void Interact()
		{
			if(oneTime)
				enabled = false;
			StartCoroutine(nameof(InteractCoroutine));
		}

		IEnumerator InteractCoroutine()
		{
			yield return new WaitForSeconds(delay);
			onInteract?.Invoke();
		}
	}
}
