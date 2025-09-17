using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PasswordPuzzle : MonoBehaviour
{
    private TDAQueue<int> _queue;

    [SerializeField] private int _size;
    [SerializeField] private int[] _password;

    public UnityEvent OnWinCondition;
    public UnityEvent OnLoseCondition;

    void Start()
    {
        _queue = new TDAQueue<int>(_size); 
        //_password = new int[_size];
    }

    public void QueueValue(int x)
    {
        _queue.Enqueue(x);
        CheckPassword();
    }

    private void CheckPassword()
    {
        

        if (_queue.IsFull())
        {
            int check = 0;
            for (int i = 0; i< _size; i++)
            {
                if (_queue.First() == _password[i])
                {
                    check++;
                    Debug.Log($"Queue First {_queue.First()} es igual a {_password[i]}");
                }
                _queue.Dequeue();
            }
            Debug.Log(check);

            if (check == 5)
            {
                CallWinCondition();
            }
            else if (check >= 0 && check < 5)
            {
                OnLoseCondition?.Invoke();
            }
        }
    }

    public void CallWinCondition()
    {
        OnWinCondition?.Invoke();
    }
}
