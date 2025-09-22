using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBJProcessor.DataWriters;

public class DeletionSynchronization
{
    private List<int> _vertexSubtrahends = new();
    private int _currectVertexSubtrahend = 0;

    public void IncrementCurrectVertexSubtrahend()
    {
        _currectVertexSubtrahend++;
    }

    public void AddVertexSubtrahend()
    {
        _vertexSubtrahends.Add(_currectVertexSubtrahend);
    }

    public int GetVertexSubtrahendOnIndex(int index)
    {
        return _vertexSubtrahends[index];
    }

    public List<int> VertexSubtrahends() {
        return _vertexSubtrahends;
    }

    public void Reset()
    {
        _vertexSubtrahends = new();
        _currectVertexSubtrahend = 0;
    }
}
