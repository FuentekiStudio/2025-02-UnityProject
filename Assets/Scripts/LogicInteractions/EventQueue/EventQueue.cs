using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EventQueue : MonoBehaviour
{
    private readonly Queue<IGameEvent> _queue = new Queue<IGameEvent>();
    private bool _processing;

    [Header("Timing")]
    [Tooltip("Time between events (0 to process everything in the same frame)")]
    public float delayBetweenEvents = 0.0f;

    public void AddEvent(IGameEvent gameEvent)
    {
        _queue.Enqueue(gameEvent);
        if (!_processing) StartCoroutine(ProcessEvents());
    }

    private IEnumerator ProcessEvents()
    {
        _processing = true;

        while (_queue.Count > 0) 
        {
            var e = _queue.Dequeue();
            e.Execute();

            if (delayBetweenEvents > 0f)
                yield return new WaitForSeconds(delayBetweenEvents);
            else
                yield return null; // next frame
        }

        _processing = false;
    }
}
