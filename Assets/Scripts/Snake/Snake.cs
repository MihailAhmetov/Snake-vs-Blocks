using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SnakeInput))]
[RequireComponent(typeof(TailGenerator))]
public class Snake : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _tailSpringiness;
    [SerializeField] private SnakeHead _snakeHead;
    [SerializeField] private int _tailSize;

    private SnakeInput _snakeInput;
    private List<Tail> _tail;
    private TailGenerator _tailGenerator;

    public event UnityAction<int> SizeUpdated;

    private void Awake()
    {
        _snakeInput = GetComponent<SnakeInput>();
        _tailGenerator = GetComponent<TailGenerator>();

        _tail = _tailGenerator.Generate(_tailSize);
        SizeUpdated?.Invoke(_tail.Count);
    }

    private void OnEnable()
    {
        _snakeHead.BlockCollided += OnBlockCollided;
        _snakeHead.BonusCollected += OnBonusCollected;
    }

    private void OnDisable()
    {
        _snakeHead.BlockCollided -= OnBlockCollided;
        _snakeHead.BonusCollected -= OnBonusCollected;
    }
    private void FixedUpdate()
    {
        Move(_snakeHead.transform.position + _snakeHead.transform.up * _speed * Time.fixedDeltaTime);

        _snakeHead.transform.up = _snakeInput.GetDirectionToClick(_snakeHead.transform.position);
    }

    private void Move(Vector3 nextPosition)
    {
        Vector3 previousPosition = _snakeHead.transform.position;

        foreach (var segment in _tail)
        {
            Vector3 tempPosition = segment.transform.position;
            segment.transform.position = Vector2.Lerp(segment.transform.position, previousPosition, _tailSpringiness * Time.deltaTime);
            previousPosition = tempPosition;
        }

        _snakeHead.Move(nextPosition);
    }

    private void OnBlockCollided()
    {
        Tail deletedSegment = _tail[_tail.Count - 1];
        _tail.Remove(deletedSegment);
        Destroy(deletedSegment.gameObject);

        SizeUpdated?.Invoke(_tail.Count);
    }

    private void OnBonusCollected(int bonusSize)
    {
        _tail.AddRange(_tailGenerator.Generate(bonusSize));
        SizeUpdated?.Invoke(_tail.Count);
    }
}
