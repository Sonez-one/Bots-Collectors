using System.Collections;
using UnityEngine;

public class BotMover : MonoBehaviour
{
    private readonly float _movementSpeed = 10f;

    private ITargetable _target;
    private Coroutine _coroutine;

    public void SetTarget(ITargetable target)
    {
        _target = target;

        StopCurrentCoroutine();

        _coroutine = StartCoroutine(Move());
    }

    public void ClearTarget()
    {
        StopCurrentCoroutine();

        _target = null;
    }

    private IEnumerator Move()
    {
        while (enabled)
        {
            transform.LookAt(_target.Position);

            transform.position = Vector3.MoveTowards(transform.position, _target.Position, _movementSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private void StopCurrentCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }
}