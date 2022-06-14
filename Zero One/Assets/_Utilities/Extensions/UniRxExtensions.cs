using UniRx;
using System;
using System.Collections.Generic;

public static class UniRxExtensions
{
    public static void OnNext(this Subject<Unit> subject)
    {
        subject.OnNext(Unit.Default);
    }

    public static IDisposable Subscribe(this IObservable<Unit> observable, Action action)
    {
        void onNext(Unit unit)
        {
            action?.Invoke();
        }

        return observable.Subscribe(onNext);
    }
}
