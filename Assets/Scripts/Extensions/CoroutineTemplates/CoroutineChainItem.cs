using UnityEngine;
using System.Collections;

public class CoroutineChainItem : CoroutineItem
{
	private CoroutineChainItem nextCoroutine;
	private CoroutineChainItem parrentCoroutine;

	public CoroutineChainItem ParrentCoroutine => parrentCoroutine;

	public CoroutineChainItem(MonoBehaviour monoBehaviour, IEnumerator routine, bool head) : base(monoBehaviour, routine, head) {}
	public CoroutineChainItem(CoroutineItem coroutineItem) : base(coroutineItem) {}
	
	public override void Start()
	{
		coroutine = monoBehaviour.StartCoroutine(Enumerator());
	}

	public CoroutineChainItem Then(CoroutineItem nextCoroutine)
	{
		nextCoroutine.Stop();

		var nextChainCoroutine = new CoroutineChainItem(nextCoroutine);
		this.nextCoroutine = nextChainCoroutine;

		nextChainCoroutine.Inherit(this);

		return nextChainCoroutine;
	}

	private void Inherit(CoroutineChainItem parrent)
	{
		parrentCoroutine = parrent;
	}

	protected IEnumerator Enumerator()
	{
		yield return monoBehaviour.StartCoroutine(routine);
		nextCoroutine?.Start();
	}
}