using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public static partial class CoroutineTemplate
{
	public static AnimationCurve LinearCurve => AnimationCurve.Linear(0, 0, 1, 1);
	public static AnimationCurve EaseInOutCurve => AnimationCurve.EaseInOut(0, 0, 1, 1);

	public class Settings
	{
		public bool invokeOnceBeforeDelay;
		public float delay;

		public bool pingPong;
		public bool repeat;
		public float interval;
		public bool ignoreTimeScale;
		public bool useFixedDelta;

		public AnimationCurve curve;

		public Settings()
		{
			invokeOnceBeforeDelay = false;
			delay = 0;
			pingPong = false;
			repeat = false;
			interval = 0;
			curve = LinearCurve;
			ignoreTimeScale = false;
			useFixedDelta = false;
		}

		public Settings(bool invokeOnceBeforeDelay = false, float delay = 0, bool pingPong = false, bool repeat = false, float interval = 0, AnimationCurve curve = null, bool ignoreTimeScale = false, bool useFixedDelta = false)
		{
			this.invokeOnceBeforeDelay = invokeOnceBeforeDelay;
			this.delay = delay;
			this.pingPong = pingPong;
			this.repeat = repeat;
			this.interval = interval;
			this.curve = curve == null ? AnimationCurve.Linear(0, 0, 1, 1) : curve;
			this.ignoreTimeScale = ignoreTimeScale;
			this.useFixedDelta = useFixedDelta;
		}
	}

	#region Lerp
	#region Item

	public static CoroutineItem LerpCoroutine(this MonoBehaviour monoBehaviour, float time, Action<float> action, Action onEnd = null, Settings settings = null)
	{
		var routine = Base(time, action, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineItem LerpCoroutine(this MonoBehaviour monoBehaviour, float time, float from, float to, Action<float> action, Action onEnd = null, Settings settings = null)
	{
		var routine = Base(time, from, to, action, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineItem LerpCoroutine(this MonoBehaviour monoBehaviour, float time, Color from, Color to, Action<Color> action, Action onEnd = null, Settings settings = null)
	{
		var routine = Base(time, from, to, action, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineItem LerpCoroutine(this MonoBehaviour monoBehaviour, float time, Vector2 from, Vector2 to, Action<Vector2> action, Action onEnd = null, Settings settings = null)
	{
		var routine = Base(time, from, to, action, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineItem LerpCoroutine(this MonoBehaviour monoBehaviour, float time, Vector3 from, Vector3 to, Action<Vector3> action, Action onEnd = null, Settings settings = null)
	{
		var routine = Base(time, from, to, action, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineItem LerpCoroutine(this MonoBehaviour monoBehaviour, float time, Quaternion from, Quaternion to, Action<Quaternion> action, Action onEnd = null, Settings settings = null)
	{
		var routine = Base(time, from, to, action, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	#endregion

	#region Chain

	public static CoroutineChainItem LerpCoroutine(
		this MonoBehaviour monoBehaviour,
		bool chainHead,
		float time,
		Action<float> action,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var routine = Base(time, action, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineChainItem LerpCoroutine(this MonoBehaviour monoBehaviour, bool chainHead, float time, float from, float to, Action<float> action, Action onEnd = null, Settings settings = null)
	{
		var routine = Base(time, from, to, action, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineChainItem LerpCoroutine(this MonoBehaviour monoBehaviour, bool chainHead, float time, Color from, Color to, Action<Color> action, Action onEnd = null, Settings settings = null)
	{
		var routine = Base(time, from, to, action, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineChainItem LerpCoroutine(this MonoBehaviour monoBehaviour, bool chainHead, float time, Vector2 from, Vector2 to, Action<Vector2> action, Action onEnd = null, Settings settings = null)
	{
		var routine = Base(time, from, to, action, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineChainItem LerpCoroutine(this MonoBehaviour monoBehaviour, bool chainHead, float time, Vector3 from, Vector3 to, Action<Vector3> action, Action onEnd = null, Settings settings = null)
	{
		var routine = Base(time, from, to, action, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineChainItem LerpCoroutine(this MonoBehaviour monoBehaviour, bool chainHead, float time, Quaternion from, Quaternion to, Action<Quaternion> action, Action onEnd = null, Settings settings = null)
	{
		var routine = Base(time, from, to, action, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	#endregion

	#region Base

	public static IEnumerator Base(float time, Action<float> action, Action onEnd = null, Settings settings = null)
	{
		var coroutine = BaseCor(time, action, onEnd, settings);
		return coroutine;
	}

	public static IEnumerator Base(float time, float from, float to, Action<float> action, Action onEnd = null, Settings settings = null)
	{
		Func<float, float, float, float> func = (f, t, k) => Mathf.LerpUnclamped(f, t, k);
		var coroutine = BaseCor(time, from, to, func, action, onEnd, settings);
		return coroutine;
	}

	public static IEnumerator Base(float time, Color from, Color to, Action<Color> action, Action onEnd = null, Settings settings = null)
	{
		Func<Color, Color, float, Color> func = (f, t, k) => Color.LerpUnclamped(f, t, k);
		var coroutine = BaseCor(time, from, to, func, action, onEnd, settings);
		return coroutine;
	}

	public static IEnumerator Base(float time, Vector2 from, Vector2 to, Action<Vector2> action, Action onEnd = null, Settings settings = null)
	{
		Func<Vector2, Vector2, float, Vector2> func = (f, t, k) => Vector2.LerpUnclamped(f, t, k);
		var coroutine = BaseCor(time, from, to, func, action, onEnd, settings);
		return coroutine;
	}

	public static IEnumerator Base(float time, Vector3 from, Vector3 to, Action<Vector3> action, Action onEnd = null, Settings settings = null)
	{
		Func<Vector3, Vector3, float, Vector3> func = (f, t, k) => Vector3.LerpUnclamped(f, t, k);
		var coroutine = BaseCor(time, from, to, func, action, onEnd, settings);
		return coroutine;
	}

	public static IEnumerator Base(float time, Quaternion from, Quaternion to, Action<Quaternion> action, Action onEnd = null, Settings settings = null)
	{
		Func<Quaternion, Quaternion, float, Quaternion> func = (f, t, k) => Quaternion.LerpUnclamped(f, t, k);
		var coroutine = BaseCor(time, from, to, func, action, onEnd, settings);
		return coroutine;
	}

	private static IEnumerator BaseCor<T>(float time, T from, T to, Func<T, T, float, T> lerpFunc, Action<T> action, Action onEnd = null, Settings settings = null)
	{
		return BaseCor(time, k =>
		{
			var l = lerpFunc(from, to, k);
			action(l);
		}, onEnd, settings);
	}

	private static IEnumerator BaseCor(float time, Action<float> action, Action onEnd = null, Settings settings = null)
	{
		if (settings == null)
			settings = new Settings();

		if (settings.delay > 0)
		{
			if (settings.invokeOnceBeforeDelay)
				action?.Invoke(settings.curve.Evaluate(0));

			yield return new WaitForSeconds(settings.delay);
		}

		var invoke = true;

		while (invoke || settings.repeat)
		{
			for (var t = 0f; t < time; )
			{
				var k = settings.curve.Evaluate(t / time);
				action?.Invoke(k);

				if (settings.useFixedDelta == true)
					yield return new WaitForFixedUpdate();
				else
					yield return null;

				float deltaTime = settings.ignoreTimeScale ? Time.unscaledDeltaTime : settings.useFixedDelta ? Time.fixedDeltaTime : Time.deltaTime;
				t += deltaTime;
			}

			if (settings.pingPong)
			{
				for (var t = time; t > 0; )
				{
					var k = settings.curve.Evaluate(t / time);
					action?.Invoke(k);

					if (settings.useFixedDelta == true)
						yield return new WaitForFixedUpdate();
					else
						yield return null;

					float deltaTime = settings.ignoreTimeScale ? Time.unscaledDeltaTime : settings.useFixedDelta ? Time.fixedDeltaTime : Time.deltaTime;
					t -= deltaTime;
				}

				action?.Invoke(settings.curve.Evaluate(0));
			}
			else
			{
				action?.Invoke(settings.curve.Evaluate(1));
			}

			onEnd?.Invoke();

			yield return new WaitForSeconds(settings.interval);

			invoke = false;
		}
	}

	#endregion
	#endregion

	#region Wait And Do
	public static CoroutineItem WaitAndDoCoroutine(this MonoBehaviour monoBehaviour, float time, Action action)
	{
		var routine = WaitAndDo(time, action);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineChainItem WaitAndDoCoroutine(this MonoBehaviour monoBehaviour, float time, Action action, bool chianHead = false)
	{
		var routine = WaitAndDo(time, action);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chianHead);
		return coroutineItem;
	}

	public static IEnumerator WaitAndDo(float time, Action action)
	{
		yield return new WaitForSeconds(time);
		action();
	}
	#endregion

	#region Do After Next Frame
	public static CoroutineItem DoAfterNextFrameCoroutine(this MonoBehaviour monoBehaviour, Action action)
	{
		var routine = DoAfterNextFrame(action);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineChainItem DoAfterNextFrameCoroutine(this MonoBehaviour monoBehaviour, Action action, bool chianHead = false)
	{
		var routine = DoAfterNextFrame(action);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chianHead);
		return coroutineItem;
	}

	public static CoroutineItem DoAfterFewFramesCoroutine(this MonoBehaviour monoBehaviour, Action action, int frames)
	{
		var routine = DoAfterNextFrame(action, frames);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineChainItem DoAfterFewFramesCoroutine(this MonoBehaviour monoBehaviour, Action action, int frames, bool chianHead = false)
	{
		var routine = DoAfterNextFrame(action, frames);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chianHead);
		return coroutineItem;
	}

	public static IEnumerator DoAfterNextFrame(Action action, int frames = 1)
	{
		for(int i = 0; i < frames; i++)
			yield return null;

		action();
	}

	public static CoroutineItem DoAfterNextFixedFrameCoroutine(this MonoBehaviour monoBehaviour, Action action)
	{
		var routine = DoAfterNextFixedFrame(action);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineChainItem DoAfterNextFixedFrameCoroutine(this MonoBehaviour monoBehaviour, Action action, bool chianHead = false)
	{
		var routine = DoAfterNextFixedFrame(action);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chianHead);
		return coroutineItem;
	}

	public static CoroutineItem DoAfterFewFixedFramesCoroutine(this MonoBehaviour monoBehaviour, Action action, int frames)
	{
		var routine = DoAfterNextFixedFrame(action, frames);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineChainItem DoAfterFewFixedFramesCoroutine(this MonoBehaviour monoBehaviour, Action action, int frames, bool chianHead = false)
	{
		var routine = DoAfterNextFixedFrame(action, frames);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chianHead);
		return coroutineItem;
	}

	public static IEnumerator DoAfterNextFixedFrame(Action action, int frames = 1)
	{
		for (int i = 0; i < frames; i++)
			yield return new WaitForFixedUpdate();

		action();
	}

	#endregion

	#region Repeat
	#region Item
	public static CoroutineItem RepeatCoroutine(this MonoBehaviour monoBehaviour, int repetitions, float interval, Action<int> action, Action onEnd = null)
	{
		var routine = Repeat(repetitions, interval, action, onEnd);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineItem RepeatCoroutine(this MonoBehaviour monoBehaviour, int repetitions, Vector2 interval, Action<int> action, Action onEnd = null)
	{

		var routine = Repeat(repetitions, interval, action, onEnd);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineItem RepeatCoroutine(this MonoBehaviour monoBehaviour, float interval, Action<int> action)
	{
		var routine = Repeat(interval, action);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineItem RepeatCoroutine(this MonoBehaviour monoBehaviour, Vector2 interval, Action<int> action)
	{
		var routine = Repeat(interval, action);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}
	#endregion

	#region Chain
	public static CoroutineChainItem RepeatCoroutine(this MonoBehaviour monoBehaviour, bool chainHead, int repetitions, float interval, Action<int> action, Action onEnd = null)
	{
		var routine = Repeat(repetitions, interval, action, onEnd);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineChainItem RepeatCoroutine(this MonoBehaviour monoBehaviour, bool chainHead, int repetitions, Vector2 interval, Action<int> action, Action onEnd = null)
	{

		var routine = Repeat(repetitions, interval, action, onEnd);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineChainItem RepeatCoroutine(this MonoBehaviour monoBehaviour, bool chainHead, float interval, Action<int> action)
	{
		var routine = Repeat(interval, action);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineChainItem RepeatCoroutine(this MonoBehaviour monoBehaviour, bool chainHead, Vector2 interval, Action<int> action)
	{
		var routine = Repeat(interval, action);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}
	#endregion

	#region Base
	public static IEnumerator Repeat(int repetitions, float interval, Action<int> action, Action onEnd)
	{
		for (int i = 0; i < repetitions; i++)
		{
			yield return new WaitForSeconds(interval);
			action(i);
		}

		onEnd?.Invoke();
	}

	public static IEnumerator Repeat(int repetitions, Vector2 interval, Action<int> action, Action onEnd)
	{
		for (int i = 0; i < repetitions; i++)
		{
			action(i);
			yield return new WaitForSeconds(Random.Range(interval.x, interval.y));
		}

		onEnd?.Invoke();
	}

	public static IEnumerator Repeat(float interval, Action<int> action)
	{
		for (var i = 0; true; i++)
		{
			action(i);
			yield return new WaitForSeconds(interval);
		}
	}

	public static IEnumerator Repeat(Vector2 interval, Action<int> action)
	{
		for (var i = 0; true; i++)
		{
			action(i);
			yield return new WaitForSeconds(Random.Range(interval.x, interval.y));
		}
	}
	#endregion
	#endregion

	#region Check Internet Connection
	public static CoroutineItem CheckInternetConnection(this MonoBehaviour monoBehaviour, Action<bool> callback, int timeout = 5, string echoServer = "https://www.google.com")
	{
		var routine = CheckInternetConnection(callback, timeout, echoServer);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineChainItem CheckInternetConnection(this MonoBehaviour monoBehaviour, bool chainHead, Action<bool> callback, int timeout = 5, string echoServer = "https://www.google.com")
	{
		var routine = CheckInternetConnection(callback, timeout, echoServer);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static IEnumerator CheckInternetConnection(Action<bool> callback, int timeout, string echoServer)
	{
		var result = true;
		using(var request = UnityWebRequest.Head(echoServer))
		{
			request.timeout = timeout;
			yield return request.SendWebRequest();
			
			result &= request.result != UnityWebRequest.Result.ConnectionError;
			result &= request.result != UnityWebRequest.Result.ProtocolError;
			result &= request.responseCode == 200;
		}
		callback(result);
	}
	#endregion

	#region Change Color
	public static CoroutineItem ChangeColor(
		this MonoBehaviour monoBehaviour,
		float time,
		Graphic graphic,
		Color to,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var routine = ChangeColor(time, graphic, to, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineChainItem ChangeColor(
		this MonoBehaviour monoBehaviour,
		bool chainHead,
		float time,
		Graphic graphic,
		Color to,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var routine = ChangeColor(time, graphic, to, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineItem ChangeAlpha(
		this MonoBehaviour monoBehaviour,
		float time,
		Graphic graphic,
		float to,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var targetColor = graphic.color;
		targetColor.a = to;
		var routine = ChangeColor(time, graphic, targetColor, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineChainItem ChangeAlpha(
		this MonoBehaviour monoBehaviour,
		bool chainHead,
		float time,
		Graphic graphic,
		float to,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var targetColor = graphic.color;
		targetColor.a = to;
		var routine = ChangeColor(time, graphic, targetColor, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static IEnumerator ChangeColor(
		float time,
		Graphic graphic,
		Color to,
		Action onEnd,
		Settings settings
	)
	{
		var coroutine = Base(
			time: time,
			from: graphic.color,
			to: to,
			action: c => graphic.color = c,
			onEnd: onEnd,
			settings: settings
		);

		return coroutine;
	}
	#endregion

	public enum Axis { X, Y, Z, XY, XZ, YZ, ALL }

	#region MoveTo
	#region Item
	public static CoroutineItem MoveTo(
		this MonoBehaviour monoBehaviour,
		Vector3 target,
		float time,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var routine = MoveTo(monoBehaviour.transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineItem MoveTo(
		this MonoBehaviour monoBehaviour,
		Transform transform,
		Vector3 target,
		float time,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var routine = MoveTo(transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineItem MoveTo(
		this MonoBehaviour monoBehaviour,
		float time,
		float x = 0,
		float y = 0,
		float z = 0,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var target = ((space == Space.World) ? monoBehaviour.transform.eulerAngles : monoBehaviour.transform.localEulerAngles) + new Vector3(x, y, z);
		var routine = MoveTo(monoBehaviour.transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;

	}

	public static CoroutineItem MoveTo(
		this MonoBehaviour monoBehaviour,
		float time,
		Transform transform,
		float x = 0,
		float y = 0,
		float z = 0,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var target = ((space == Space.World) ? monoBehaviour.transform.eulerAngles : monoBehaviour.transform.localEulerAngles) + new Vector3(x, y, z);;
		var routine = MoveTo(transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}
	#endregion

	#region Chain
	public static CoroutineChainItem MoveTo(
		this MonoBehaviour monoBehaviour,
		bool chainHead,
		Vector3 target,
		float time,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var routine = MoveTo(monoBehaviour.transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineChainItem MoveTo(
		this MonoBehaviour monoBehaviour,
		bool chainHead,
		Transform transform,
		Vector3 target,
		float time,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var routine = MoveTo(transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineChainItem MoveTo(
		this MonoBehaviour monoBehaviour,
		bool chainHead,
		float time,
		float x = 0,
		float y = 0,
		float z = 0,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var target = ((space == Space.World) ? monoBehaviour.transform.eulerAngles : monoBehaviour.transform.localEulerAngles) + new Vector3(x, y, z);
		var routine = MoveTo(monoBehaviour.transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;

	}

	public static CoroutineChainItem MoveTo(
		this MonoBehaviour monoBehaviour,
		bool chainHead,
		float time,
		Transform transform,
		float x = 0,
		float y = 0,
		float z = 0,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var target = ((space == Space.World) ? monoBehaviour.transform.eulerAngles : monoBehaviour.transform.localEulerAngles) + new Vector3(x, y, z);;
		var routine = MoveTo(transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}
	#endregion

	#region Base
	public static IEnumerator MoveTo(Transform transform, Vector3 target, float time, Axis scaleAxes, Space space, Action onEnd, Settings settings)
	{
		var startPosition = (space == Space.World) ? transform.position : transform.localPosition;

		var coroutine = Base(
			time: time,
			from: startPosition,
			to: target,
			action: newPositions =>
			{
				var position = (space == Space.World) ? transform.position : transform.localPosition;

				switch (scaleAxes)
				{
					case Axis.ALL:
						position = newPositions;
						break;

					case Axis.X:
						position = new Vector3(newPositions.x, position.y, position.z);
						break;
					case Axis.Y:
						position = new Vector3(position.x, newPositions.y, position.z);
						break;
					case Axis.Z:
						position = new Vector3(position.x, position.y, newPositions.z);
						break;

					case Axis.XY:
						position = new Vector3(newPositions.x, newPositions.y, position.z);
						break;
					case Axis.XZ:
						position = new Vector3(newPositions.x, position.y, newPositions.z);
						break;
					case Axis.YZ:
						position = new Vector3(position.x, newPositions.y, newPositions.z);
						break;
				}

				if (space == Space.World)
					transform.position = position;
				else
					transform.localPosition = position;
			},
			onEnd : onEnd,
			settings : settings
		);

		return coroutine;
	}
	#endregion
	#endregion

	#region ScaleTo
	#region Item
	public static CoroutineItem ScaleTo(
		this MonoBehaviour monoBehaviour,
		Vector3 target,
		float time,
		Axis axis = Axis.ALL,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var routine = ScaleTo(monoBehaviour.transform, target, time, axis, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineItem ScaleTo(
		this MonoBehaviour monoBehaviour,
		Transform transform,
		Vector3 target,
		float time,
		Axis axis = Axis.ALL,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var routine = ScaleTo(transform, target, time, axis, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}
	#endregion

	#region Chain
	public static CoroutineChainItem ScaleTo(
		this MonoBehaviour monoBehaviour,
		bool chainHead,
		Vector3 target,
		float time,
		Axis axis = Axis.ALL,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var routine = ScaleTo(monoBehaviour.transform, target, time, axis, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineChainItem ScaleTo(
		this MonoBehaviour monoBehaviour,
		bool chainHead,
		Transform transform,
		Vector3 target,
		float time,
		Axis axis = Axis.ALL,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var routine = ScaleTo(transform, target, time, axis, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}
	#endregion

	#region Base
	public static IEnumerator ScaleTo(
		Transform transform,
		Vector3 target,
		float time,
		Axis scaleAxes,
		Action onEnd,
		Settings settings
	)
	{
		var coroutine = Base(
			time: time,
			from: transform.localScale,
			to: target,
			action: newScale =>
			{
				var scale = transform.localScale;

				switch (scaleAxes)
				{
					case Axis.ALL:
						scale = newScale;
						break;

					case Axis.X:
						scale = new Vector3(newScale.x, scale.y, scale.z);
						break;
					case Axis.Y:
						scale = new Vector3(scale.x, newScale.y, scale.z);
						break;
					case Axis.Z:
						scale = new Vector3(scale.x, scale.y, newScale.z);
						break;

					case Axis.XY:
						scale = new Vector3(newScale.x, newScale.y, scale.z);
						break;
					case Axis.XZ:
						scale = new Vector3(newScale.x, scale.y, newScale.z);
						break;
					case Axis.YZ:
						scale = new Vector3(scale.x, newScale.y, newScale.z);
						break;
				}

				transform.localScale = scale;
			},
			onEnd : onEnd,
			settings : settings
		);

		return coroutine;
	}
	#endregion
	#endregion

	#region RotateTo
	#region Item
	public static CoroutineItem RotateTo(
		this MonoBehaviour monoBehaviour,
		float time,
		Vector3 target,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var routine = Rotate(monoBehaviour.transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineItem RotateTo(
		this MonoBehaviour monoBehaviour,
		float time,
		Transform transform,
		Vector3 target,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var routine = Rotate(transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineItem Rotate(
		this MonoBehaviour monoBehaviour,
		float time,
		Vector3 value,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var target = ((space == Space.World) ? monoBehaviour.transform.eulerAngles : monoBehaviour.transform.localEulerAngles) + value;
		var routine = Rotate(monoBehaviour.transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineItem Rotate(
		this MonoBehaviour monoBehaviour,
		float time,
		Transform transform,
		Vector3 value,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var target = ((space == Space.World) ? monoBehaviour.transform.eulerAngles : monoBehaviour.transform.localEulerAngles) + value;
		var routine = Rotate(transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineItem Rotate(
		this MonoBehaviour monoBehaviour,
		float time,
		float x = 0,
		float y = 0,
		float z = 0,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var target = ((space == Space.World) ? monoBehaviour.transform.eulerAngles : monoBehaviour.transform.localEulerAngles) + new Vector3(x, y, z);
		var routine = Rotate(monoBehaviour.transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}

	public static CoroutineItem Rotate(
		this MonoBehaviour monoBehaviour,
		float time,
		Transform transform,
		float x = 0,
		float y = 0,
		float z = 0,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var target = ((space == Space.World) ? monoBehaviour.transform.eulerAngles : monoBehaviour.transform.localEulerAngles) + new Vector3(x, y, z);;
		var routine = Rotate(transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineItem(monoBehaviour, routine);
		return coroutineItem;
	}
	#endregion

	#region Chain
	public static CoroutineChainItem RotateTo(
		this MonoBehaviour monoBehaviour,
		bool chainHead,
		float time,
		Vector3 target,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var routine = Rotate(monoBehaviour.transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineChainItem RotateTo(
		this MonoBehaviour monoBehaviour,
		bool chainHead,
		float time,
		Transform transform,
		Vector3 target,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var routine = Rotate(transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineChainItem Rotate(
		this MonoBehaviour monoBehaviour,
		bool chainHead,
		float time,
		Vector3 value,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var target = ((space == Space.World) ? monoBehaviour.transform.eulerAngles : monoBehaviour.transform.localEulerAngles) + value;
		var routine = Rotate(monoBehaviour.transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineChainItem Rotate(
		this MonoBehaviour monoBehaviour,
		bool chainHead,
		float time,
		Transform transform,
		Vector3 value,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var target = ((space == Space.World) ? monoBehaviour.transform.eulerAngles : monoBehaviour.transform.localEulerAngles) + value;
		var routine = Rotate(transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineChainItem Rotate(
		this MonoBehaviour monoBehaviour,
		bool chainHead,
		float time,
		float x = 0,
		float y = 0,
		float z = 0,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var target = ((space == Space.World) ? monoBehaviour.transform.eulerAngles : monoBehaviour.transform.localEulerAngles) + new Vector3(x, y, z);
		var routine = Rotate(monoBehaviour.transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}

	public static CoroutineChainItem Rotate(
		this MonoBehaviour monoBehaviour,
		bool chainHead,
		float time,
		Transform transform,
		float x = 0,
		float y = 0,
		float z = 0,
		Axis axis = Axis.ALL,
		Space space = Space.World,
		Action onEnd = null,
		Settings settings = null
	)
	{
		var target = ((space == Space.World) ? monoBehaviour.transform.eulerAngles : monoBehaviour.transform.localEulerAngles) + new Vector3(x, y, z);;
		var routine = Rotate(transform, target, time, axis, space, onEnd, settings);
		var coroutineItem = new CoroutineChainItem(monoBehaviour, routine, chainHead);
		return coroutineItem;
	}
	#endregion

	#region Base
	public static IEnumerator Rotate(Transform transform, Vector3 target, float time, Axis scaleAxes, Space space, Action onEnd, Settings settings)
	{
		var startAngles = (space == Space.World) ? transform.eulerAngles : transform.localEulerAngles;

		var coroutine = Base(
			time: time,
			from: startAngles,
			to: target,
			action: newAngles =>
			{
				var angles = startAngles;

				switch (scaleAxes)
				{
					case Axis.ALL:
						angles = newAngles;
						break;

					case Axis.X:
						angles = new Vector3(newAngles.x, startAngles.y, startAngles.z);
						break;
					case Axis.Y:
						angles = new Vector3(startAngles.x, newAngles.y, startAngles.z);
						break;
					case Axis.Z:
						angles = new Vector3(startAngles.x, startAngles.y, newAngles.z);
						break;

					case Axis.XY:
						angles = new Vector3(newAngles.x, newAngles.y, startAngles.z);
						break;
					case Axis.XZ:
						angles = new Vector3(newAngles.x, startAngles.y, newAngles.z);
						break;
					case Axis.YZ:
						angles = new Vector3(startAngles.x, newAngles.y, newAngles.z);
						break;
				}

				if (space == Space.World)
					transform.eulerAngles = angles;
				else
					transform.localEulerAngles = angles;
			},
			onEnd : onEnd,
			settings : settings
		);

		return coroutine;
	}

	#endregion
	#endregion
}