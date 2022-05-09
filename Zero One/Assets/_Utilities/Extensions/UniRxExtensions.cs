using UniRx;
using System;
using System.Collections.Generic;

public static class UniRxExtensions
{
    public static IDisposable Subscribe(this IObservable<Unit> observable, Action action)
    {
        void onNext(Unit unit)
        {
            action?.Invoke();
        }

        return observable.Subscribe(onNext);
    }

    public static void AddTo(this IDisposable disposable, DisposableList disposableList)
    {
        disposableList.Add(disposable);
    }
}

public class DisposableList : IDisposable
{
    private List<IDisposable> _disposables = new List<IDisposable>();

    public void Dispose()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }

    public void Add(IDisposable disposable)
    {
        _disposables.Add(disposable);
    }
}
