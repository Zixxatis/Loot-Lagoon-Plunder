using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    public class EnemyBrain : MonoBehaviour
    {
        [field: SerializeField] public PlayerDetector LongRangeDetector { get; private set; }
        [field: SerializeField] public PlayerDetector CloseRangeDetector { get; private set; }

        private DialogueAnimator dialogueAnimator;

        private Queue<ITask> defaultTasks;
        private Queue<ITask> longRangeTasks;
        private Queue<ITask> closeRangeTasks;

        private Queue<ITask> currentTaskQueue;
        private ITask currentTask;

        public void Initialize(DetectionConfig detectionConfig, Queue<ITask> defaultTasks, Queue<ITask> longRangeTasks, Queue<ITask> closeRangeTasks, DialogueAnimator dialogueAnimator)
        {
            PrepareDetectors(detectionConfig);

            this.dialogueAnimator = dialogueAnimator;
            this.defaultTasks = defaultTasks;
            this.longRangeTasks = longRangeTasks;
            this.closeRangeTasks = closeRangeTasks;

            currentTaskQueue = this.defaultTasks;
            SetNextTask();
        }

        private void Update()
        {
            currentTask.Update();

            if(currentTask.IsFinished())
                SetNextTask();
        }

        /// <summary> Sets next task and moves it to the end of the queue. </summary>
        private void SetNextTask()
        {
            ITask nextTask = currentTaskQueue.Dequeue();
            currentTaskQueue.Enqueue(nextTask);

            nextTask.StartTask();
            currentTask = nextTask;
        }

        /// <summary> Changes 'currentQueue' to copy of the given queue. </summary>
        /// <remarks> If the given queue is null or is the same as the current - won't change anything. </remarks>
        private void SwitchTaskQueue(Queue<ITask> nextTaskQueue)
        {
            if(nextTaskQueue == null || currentTaskQueue == nextTaskQueue)
                return;

            currentTaskQueue = new Queue<ITask>(nextTaskQueue);
            currentTask.ForceToAbortTask();
        }

        public void LoseAllTargets()
        {
            CloseRangeDetector.LoseTarget();
            LongRangeDetector.LoseTarget();
        }

        private void SwitchToLongRangeTasks()
        {
            SwitchTaskQueue(longRangeTasks); 
            
            if(dialogueAnimator != null)
                dialogueAnimator.PlayDialogueAnimation(DialogueEmotion.Exclamation);
        }
        
        private void ReturnToDefaultTask()
        {
            SwitchTaskQueue(defaultTasks); 
            
            if(dialogueAnimator != null)
                dialogueAnimator.PlayDialogueAnimation(DialogueEmotion.Question);
        }

        private void PrepareDetectors(DetectionConfig detectionConfig)
        {
            LongRangeDetector.Initialize(detectionConfig.LongDetectionRadius);
            LongRangeDetector.OnEnemyDetected += SwitchToLongRangeTasks;
            LongRangeDetector.OnEnemyLost += ReturnToDefaultTask;

            CloseRangeDetector.Initialize(detectionConfig.CloseDetectionRadius);
            CloseRangeDetector.OnEnemyDetected += () => SwitchTaskQueue(closeRangeTasks);
            CloseRangeDetector.OnEnemyLost += () => SwitchTaskQueue(longRangeTasks);
        }

        public void DisableDetectors()
        {
            LongRangeDetector.OnEnemyDetected -= SwitchToLongRangeTasks;
            LongRangeDetector.OnEnemyLost -= ReturnToDefaultTask;
            LongRangeDetector.DeactivateGameObject();

            CloseRangeDetector.OnEnemyDetected -= () => SwitchTaskQueue(closeRangeTasks);
            CloseRangeDetector.OnEnemyLost -= () => SwitchTaskQueue(longRangeTasks);
            CloseRangeDetector.DeactivateGameObject();
        }
    }
}