using OBJProcessor;
using System.Collections.Concurrent;

namespace OBJProcessorTests;

public class ThreadSafeListTests
{
    static void RunTest(int numberOfTasks, ThreadSafeList<int> list, Action<ThreadSafeList<int>> action)
    {
        var tasks = new Task[numberOfTasks];

        for (int i = 0; i < numberOfTasks; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                action(list);
            });
        }

        Task.WaitAll(tasks);
    }

    [Fact]
    public void SimpleAddTest()
    {
        int numberOfTasks = 100;
        int numberOfIterations = 1000;

        var list = new ThreadSafeList<int>();
        RunTest(numberOfTasks, list, (ThreadSafeList<int> list) =>
        {
            for (int i = 0; i < numberOfIterations; i++)
            {
                list.Add(i);
            }
        });

        Assert.Equal(numberOfTasks * numberOfIterations, list.Count);
    }
}
public class ThreadSafeEnumeratorTests
{
    [Fact]
    public void EmptyCollection()
    {
        var emptyList = new ThreadSafeList<int> { };
        var enumerator = emptyList.GetEnumerator();

        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void ReturnsElementsInCorrectOrder()
    {
        var list = new ThreadSafeList<int> { 1, 2, 3 };
        var enumerator = list.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(1, enumerator.Current);

        Assert.True(enumerator.MoveNext());
        Assert.Equal(2, enumerator.Current);

        Assert.True(enumerator.MoveNext());
        Assert.Equal(3, enumerator.Current);

        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void Reset_StartsFromBeginning()
    {
        var list = new ThreadSafeList<int> { 1, 2 };
        var enumerator = list.GetEnumerator();

        enumerator.MoveNext();
        enumerator.Reset();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(1, enumerator.Current);
    }

    [Fact]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        var list = new ThreadSafeList<int> { 1 };
        var enumerator = list.GetEnumerator();

        enumerator.Dispose();
        enumerator.Dispose();
        Assert.Throws<ObjectDisposedException>(() => enumerator.MoveNext());
    }

    [Fact]
    public void ThrowsIfCollectionModified_Add()
    {
        var list = new ThreadSafeList<int> { 1, 2, 3, 4 };
        var enumerator = list.GetEnumerator();

        enumerator.MoveNext();
        list.Add(4);

        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }

    [Fact]
    public void ThrowsIfCollectionModified_Remove()
    {
        var list = new ThreadSafeList<int> { 1, 2, 3 };
        var enumerator = list.GetEnumerator();
        enumerator.MoveNext();

        list.Remove(2);

        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }

    [Fact]
    public void ThrowsIfCollectionModified_Clear()
    {
        var list = new ThreadSafeList<int> { 1, 2, 3 };
        var enumerator = list.GetEnumerator();
        enumerator.MoveNext();

        list.Clear();

        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }

    [Fact]
    public void TwoIndependentEnumerators()
    {
        var list = new ThreadSafeList<int> { 1, 2, 3, 4, 5 };
        var enumerator1 = list.GetEnumerator();
        var enumerator2 = list.GetEnumerator();

        enumerator1.MoveNext();
        var en1Current = enumerator1.Current;

        enumerator2.MoveNext();
        enumerator2.MoveNext();
        var en2Current = enumerator2.Current;

        Assert.Equal(1, en1Current);
        Assert.Equal(2, en2Current);
    }

    //[Fact]
    //public void Concurrent_Enumeration_While_Adding()
    //{
    //    var list = new ThreadSafeList<int>();
    //    int itemsToAdd = 100;
    //    var allEnumeratedCounts = new ConcurrentBag<int>();

    //    var writerTask = Task.Run(() =>
    //    {
    //        for (int i = 0; i < itemsToAdd; i++)
    //        {
    //            list.Add(i);
    //            Thread.Sleep(1);
    //        }
    //    });

    //    var readerTask = Task.Run(() =>
    //    {
    //        while (writerTask.IsCompleted == false)
    //        {
    //            int countDuringEnumeration = 0;
    //            foreach (var item in list)
    //            {
    //                countDuringEnumeration++;
    //                Thread.Sleep(1);
    //            }
    //            allEnumeratedCounts.Add(countDuringEnumeration);
    //        }
    //    });

    //    Task.WaitAll(writerTask, readerTask);
    //    Assert.True(allEnumeratedCounts.All(count => count >= 0));
    //    Assert.True(allEnumeratedCounts.Any(), "No enumerations were performed.");

    //    Console.WriteLine($"Enumeration counts ranged from {allEnumeratedCounts.Min()} to {allEnumeratedCounts.Max()}");
    //}

    [Fact]
    public void MultipleIterations()
    {
        int numberOfElements = 1000;
        var threadSafeList = new ThreadSafeList<int>();
        for (int i = 0; i < numberOfElements; i++)
            threadSafeList.Add(i);

        var results = new ConcurrentBag<int>();
        int numberOfIterations = 1000;

        Parallel.For(0, numberOfIterations, _ =>
        {
            foreach (var item in threadSafeList)
            {
                results.Add(item);
            }
        });

        Assert.Equal(threadSafeList.Count * numberOfIterations, results.Count);
    }
}
