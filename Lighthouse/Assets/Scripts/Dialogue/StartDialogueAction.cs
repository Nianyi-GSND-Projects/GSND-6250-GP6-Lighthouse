using UnityEngine;

namespace Game
{
    public class StartDialogueAction : MonoBehaviour
    {
        [SerializeField] private DialogueAsset dialogueToStart;
        
        [SerializeField] private DialogueEvent onStartDialogue;
        
        public void TriggerDialogue()
        {
            if (dialogueToStart == null)
            {
                Debug.LogWarning("Dialogue Asset is not assigned!", this.gameObject);
                return;
            }

            onStartDialogue?.Invoke(dialogueToStart);
        }
    }
}