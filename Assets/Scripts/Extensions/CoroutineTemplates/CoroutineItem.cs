using UnityEngine;
using System.Collections;

public class CoroutineItem
{
	protected MonoBehaviour monoBehaviour;
	protected Coroutine coroutine;
	protected IEnumerator routine;

	public Coroutine Coroutine => coroutine;

	public CoroutineItem(MonoBehaviour monoBehaviour, IEnumerator routine, bool start = true)
	{
		this.monoBehaviour = monoBehaviour;
		this.routine = routine;

		if (start)
			Start();
	}
	
	public CoroutineItem(CoroutineItem coroutineItem)
	{
		this.monoBehaviour = coroutineItem.monoBehaviour;
		this.routine = coroutineItem.routine;
	}

	public virtual void Start()
	{
		coroutine = monoBehaviour.StartCoroutine(routine);
	}

	public void Stop()
	{
		if (coroutine != null)
			monoBehaviour.StopCoroutine(coroutine);
	}
}