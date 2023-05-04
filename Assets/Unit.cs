using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _health;

    [SerializeField] private bool _cancel;


    private void Start()
    {
        RecieveHealing();

        CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
        CancellationToken _cancelToken = _cancelTokenSource.Token;

        Task task1 = new Task(() => Task1Async(_cancelToken));
        Task task2 = new Task(() => Task2Async(_cancelToken));

        task1.Start();
        task2.Start();

        if (_cancel)
        {
            _cancelTokenSource.Cancel();
            _cancelTokenSource.Dispose();
        }

        Debug.Log(WhatTaskFasterAsync(_cancelToken, task1, task2).Result);
    }
    private void RecieveHealing()
    {
        StartCoroutine(Healing(5, 0.5f));
    }
    private IEnumerator Healing(int value, float time)
    {
        for (; _health + value <= _maxHealth;)
        {
            _health += value;
            yield return new WaitForSeconds(time);
        }
    }
    async void Task1Async(CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
        {
            Debug.Log("Операция Task1Async прервана токеном.");
            return;
        }
        await Task.Delay(1000);
        Debug.Log("Task1 finishes.");
    }
    async void Task2Async(CancellationToken cancelToken)
    {
        for (int i = 0; i < 60; i++)
        {
            if (cancelToken.IsCancellationRequested)
            {
                Debug.Log("Операция Task2Async прервана токеном.");
                return;
            }
            await Task.Yield();
        }
        Debug.Log("Task2 finishes.");
    }

    public static Task<bool> WhatTaskFasterAsync(CancellationToken ct, Task task1, Task task2)
    {
        bool returner = false;

        using (CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct))
        {
            CancellationToken linkedCt = linkedCts.Token;
            var result = Task.WhenAny(task1, task2);
            if (result == task1 && result.IsCompleted == true)
                returner = true;
            else
                returner = false;
            linkedCts.Cancel();
        }

        Task<bool> t = Task.Run(() =>
        {
            return returner;
        });

        return t;
    }
}
