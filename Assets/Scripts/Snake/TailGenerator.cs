using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailGenerator : MonoBehaviour
{
    [SerializeField] private Tail _tailTemplate;
    public List<Tail> Generate(int count)
    {
        List<Tail> tail = new List<Tail>();

        for (int i = 0; i < count; i++)
        {
            tail.Add(Instantiate(_tailTemplate, transform));
        }

        return tail;
    }
}
