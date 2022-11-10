# Шаблоны корутин

Расширение для *`MonoBehaviour`*

### Оглавление
- [Использование](#Использование)
- [Цепь корутин](#Цепь-корутин)
- [MoveTo](#Coroutine-MoveTo)
- [ScaleTo](#Coroutine-ScaleTo)
- [ChangeColor](#Coroutine-ChangeColor)
- [LerpCoroutine](#Coroutine-LerpCoroutine)
- [Settings](#CoroutineTemplate.Settings-Settings)
- [WaitAndDoCoroutine](#Coroutine-WaitAndDoCoroutine)
- [DoAfterNextFrameCoroutine](#Coroutine-DoAfterNextFrameCoroutine)
- [RepeatCoroutine](#Coroutine-RepeatCoroutine)
- [CheckInternetConnection](#Coroutine-CheckInternetConnection)

### Использование

У любого *`MonoBehaviour`* можно запустить корутину следующим способом:

```csharp
this.НазваниеКорутины(
	имяПараметра_1: значение_1,
	имяПараметра_2: значение_2,
	...
	имяПараметра_N: значение_N
);
```

#### Пример
```csharp
this.LerpCoroutine(
	// Время выполнения
	time: 1,

	// Начальное значение
	from: Vector3.zero,

	// Конечное значение
	to: Vector3.one,

	// Действие со значением
	action: s => transform.localScale = s
);
```

Все корутины возвращают тип *`CoroutineItem`*, поэтому позже их можно останавливать, например вот так:
#### Пример

```csharp
// Запускаем корутину
var myCoroutine = this.LerpCoroutine(
	time: 1,
	from: Vector3.zero,
	to: Vector3.one,
	action: s => transform.localScale = s
);

// Останавливаем корутину
StopCoroutine(myCoroutine);
```

## Цепь корутин

Можно создавать цепь из корутин, которые будут выполняться последовательно.

Для этого в первой корутине надо указать `chainHead:` **`true`**. После чего можно вызвать `.Then()` куда передать следующую корутину.

```csharp
this.ChangeColor(chainHead: true, 1, image, new Color(.5f, 0, 1))
.Then(this.MoveTo(1, image.transform, x: 150, space: Space.Self))
.Then(this.Rotate(1, image.transform, z: -90));
```

![](cube-chain.gif)

# Корутины

## *`CoroutineItem`* MoveTo

Перемещает объект к указанной точке

- *`Transform`* **`transform`** (default = `this.gameObject`): Transform для перемещения.
- *`Vector3`* **`target`**: Целевая позиция
- *`float`* **`time`**: Время перемещения (в секундах)
- *`Axis`* **`axis`** (default = `CoroutineTemplate.Axis.ALL`): По каким осям передвигаться
- *`Space`* **`space`** (default = `Space.World`): В каких координатах перемещать
- *`Action`* **`onEnd`** (опционально): Действие по завершению корутины
- *`Settings`* **`settings`** (опционально): [Дополнительные настройки](#CoroutineTemplate.Settings-Settings)


### Пример

Переместит объект из текущей позиции до *`Vector3.one`* за *`3`* секунды

```csharp
this.MoveTo(Vector3.one, 3);
```

Переместит объект *`otherGO`* из текущей позиции до *`Vector3.one`* за *`3`* секунды <b>только по X</b> координате <b>в локальных</b> координатах (localPosition).

```csharp
this.MoveTo(
	transform: otherGO.transform
	target: Vector3.one,
	time: 3,
	axis: CoroutineTemplate.Axis.X,
	space: Space.Self
);

```

## *`CoroutineItem`* ScaleTo

Масштабирует объект до указанного значения

- *`Transform`* **`transform`** (default = `this.gameObject`): Transform для перемещения
- *`Vector3`* **`target`**: Целевой масштаб
- *`float`* **`time`**: Время масштабирования (в секундах)
- *`Axis`* **`axis`** (default = `CoroutineTemplate.Axis.ALL`): По каким осям масштабировать (`X`, `Y`, `Z`, `XY`, `XZ`, `YZ` или `ALL`)
- *`Action`* **`onEnd`** (опционально): Действие по завершению корутины
- *`Settings`* **`settings`** (опционально): [Дополнительные настройки](#CoroutineTemplate.Settings-Settings)


### Пример

Масштабирует объект до *`Vector3.one`* за *`3`* секунды

```csharp
this.ScaleTo(Vector3.one, 3);
```

Масштабирует объект до *`Vector3.one`* за *`3`* секунды <b>только по Y</b> координате и обратно.

```csharp
this.ScaleTo(
	target: Vector3.one,
	time: 3,
	axis: CoroutineTemplate.Axis.Y,
	space: Space.Self,
	settings: new CoroutineTemplate.Settings(
		pingPong: true
	)
);
```

## *`CoroutineItem`* ChangeColor

Менят цвет

- *`Graphic`* **`graphic`**: У чего менять цвет
- *`Color`* **`to`**: Целевой цвет
- *`float`* **`time`**: Время изменения (в секундах)
- *`Action`* **`onEnd`** (опционально): Действие по завершению корутины
- *`Settings`* **`settings`** (опционально): [Дополнительные настройки](#CoroutineTemplate.Settings-Settings)


### Пример

Меняет цвет картинки на красный

```csharp
this.ChangeColor(
	graphic: myImage,
	to: Color.red,
	time: 2
);
```

## *`CoroutineItem`* LerpCoroutine

Основной шаблон для корутины. Удобно использовать для разных анимаций, замены цвета и т.п.

`T` — *`float`*, *`Color`*, *`Vector2`*, *`Vector3`* или *`Quaternion`*
- **`T`** **`time`**: Время выполнения
- **`T`** **`from`**: Начальное значение
- **`T`** **`to`**: Конечное значение
- *`Action<T>`* **`action`**: Действие для каждого шага корутины
- *`Action`* **`onEnd`** (опционально): Действие по завершению корутины
- *`CoroutineTemplate.Settings`* **`settings`** (опционально): [Дополнительные настройки](#CoroutineTemplate.Settings-Settings)

### Примеры
Таймер
```csharp
this.LerpCoroutine(
	time: 5,
	from: 5,
	to: 0,
	action: t => timeText.text = t.ToString("0"),
	onEnd: () => Debug.Log("Time end")
);
```

Замена цвета с красного на синий за 3.5 секунд
```csharp
this.LerpCoroutine(
	time: 3.5f,
	from: Color.red,
	to: Color.blue,
	action: c => ring.color = c,
	settings: new CoroutineTemplate.Settings(
		delay: 1,
		pingPong: true,
		RepeatCoroutine: true,
		interval: 1f
	)
);
```

Скейл от 0 до 1
```csharp
this.LerpCoroutine(
	time: 1,
	from: Vector3.zero,
	to: Vector3.one,
	action: s => transform.localScale = s
);
```

## *`CoroutineTemplate.Settings`* Settings

Дополнительные настройки для корутин выше

- *`float`* **`delay`** (default = `0`): Задержка перед стартом
- *`bool`* **`invokeOnceBeforeDelay`** (default = `false`): Выполнить один раз перед задержкой. (актуально при `delay` > 0)
- *`bool`* **`pingPong`** (default = `false`): Корутина туда и обратно. (`from` -> `to` -> `from`)
- *`bool`* **`RepeatCoroutine`** (default = `false`): Бесконечное повторение корутины
- *`float`* **`interval`** (default = `0`): Интервал между повторениями. (актуально при `RepeatCoroutine` = __true__)
- *`AnimationCurve`* **`curve`** (default = `AnimationCurve.EaseInOut(0, 0, 1, 1)`): Кривая по времени для неравномерной корутины.

## *`CoroutineItem`* WaitAndDoCoroutine

Выполнит действие через указанное время

- *`float`* **`time`**: Время задержки (в секундах)
- *`Action`* **`action`**: Действие после задержки

### Пример
Подождёт 1 секунду и выведет "Do something"
```csharp
this.WaitAndDoCoroutine(
	time: 1,
	action: () => Debug.Log("Do something")
);
```

## *`CoroutineItem`* DoAfterNextFrameCoroutine

Выполнит действие в следующем кадре

- *`Action`* **`action`**: Действие после задержки

### Пример
Подождёт 1 кадр и выведет "Do something"
```csharp
this.DoAfterNextFrameCoroutine(
	action: () => Debug.Log("Do something")
);
```

## *`CoroutineItem`* RepeatCoroutine

Повторение одинаковых действий

Бесконечные повторения
- *`float`* или *`Vector2`* **`interval`**: Интервал между повторениями. Если тип `Vector2` тогда каждый интервал случайно выбирается между `interval.x` и `interval.y`
- *`Action<int>`* **`action`**: Действие. В переменной возвращается номер текущего повторения.

Ограниченные повторения
- *`int`* **`repetitions`**: Количество повторений
- *`float`* или *`Vector2`* **`interval`**: Интервал между повторениями. Если тип `Vector2` тогда каждый интервал случайно выбирается между `interval.x` и `interval.y`
- *`Action<int>`* **`action`**: Действие. В переменной возвращается номер текущего повторения.
- *`Action`* **`onEnd`** (опционально): Действие по завершению корутины.

### Примеры

12 раз с интервалом в 1 секунду выведет "Boom" и в конце выведет "BOOOOOOM!"
```csharp
this.RepeatCoroutine(
	repetitions: 12,
	interval: 1,
	action: i => Debug.Log("Boom"),
	onEnd: () => Debug.Log("BOOOOOOM!")
);
```

Каждую секунду будет выводить "0", "1", "2" и т.д.
```csharp
this.RepeatCoroutine(
	interval: 1,
	action: i => Debug.Log(i.ToString())
);
```

## *`CoroutineItem`* CheckInternetConnection

Проверка интернет соединения

- *`Action<bool>`* **`callback`**: Действие после проверки. В переменной возвращается результат проверки.
- *`int`* **`timeout`** (default = `5`): Таймаут запроса
- *`string`* **`echoServer`** (default = `https://www.google.com`): URL для проверки

### Пример

Выводит в консоль статус подключения
```csharp
this.CheckInternetConnection(
	callback: isConnected => Debug.Log(isConnected ? "Connected" : "No Connections!")
);
```