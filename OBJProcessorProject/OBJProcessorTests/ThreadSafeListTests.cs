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
    public void EmptyCollection()
    {
        var emptyList = new ThreadSafeList<int> {};
        var enumerator = emptyList.GetEnumerator();

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
    public void MultipleIterations()
    {
        var threadSafeList = new ThreadSafeList<int> { 1, 2, 3, 4 };
        var results = new ConcurrentBag<int>();
        int numberOfIterations = 300;

        Parallel.For(0, numberOfIterations, _ =>
        {
            foreach (var item in threadSafeList)
            {
                results.Add(item);
            }
        });

        Assert.Equal(threadSafeList.Count * numberOfIterations, results.Count);
    }

    [Fact]
    public void ThrowsIfCollectionModified()
    {
        var list = new ThreadSafeList<int> { 1, 2, 3, 4 };
        var enumerator = list.GetEnumerator();

        enumerator.MoveNext();
        list.Add(4);

        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }
}

// TODO: More tests