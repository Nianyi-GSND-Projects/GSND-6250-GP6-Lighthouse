using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic; // 引入队列

namespace Game
{
    public class SubtitleManager : MonoBehaviour
    {
        public static SubtitleManager Instance { get; private set; }

        [SerializeField] private GameObject subtitlePanel;
        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private float typingSpeed = 0.05f;
        
        private Queue<DialogueAsset.Line> dialogueQueue;
        private Coroutine displayCoroutine;
        
        public bool IsBusy { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                dialogueQueue = new Queue<DialogueAsset.Line>();
            }
        }
        
        public void StartDialogue(DialogueAsset dialogueAsset)
        {

            if (IsBusy) return;
            
            dialogueQueue.Clear();
            foreach (var line in dialogueAsset.lines)
            {
                dialogueQueue.Enqueue(line);
            }
            
            if (displayCoroutine == null)
            {
                displayCoroutine = StartCoroutine(DisplayNextLine());
            }
        }

        private IEnumerator DisplayNextLine()
        {
            IsBusy = true;
            subtitlePanel.SetActive(true);
            
            DialogueAsset.Line currentLine = dialogueQueue.Dequeue();
            
            // subtitleText.text = "";
            // float typingStartTime = Time.time;
            // foreach (char letter in currentLine.text.ToCharArray())
            // {
            //     subtitleText.text += letter;
            //     yield return new WaitForSeconds(typingSpeed);
            // }
            // float typingEndTime = Time.time;
            //
            // float typingDuration = typingEndTime - typingStartTime;
            // float waitTime = currentLine.duration - typingDuration;
            // if (waitTime > 0)
            // {
            //     yield return new WaitForSeconds(waitTime);
            // }
            
            subtitleText.text = currentLine.text;
            yield return new WaitForSeconds(currentLine.duration);
            
            if (dialogueQueue.Count > 0)
            {
                displayCoroutine = StartCoroutine(DisplayNextLine());
            }
            else
            {
                subtitlePanel.SetActive(false);
                IsBusy = false;
                displayCoroutine = null;
            }
        }
    }
}