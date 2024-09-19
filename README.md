Данный проект сделан на основе популярной мобильной игры Tap tap Dash.

![tappu](https://github.com/user-attachments/assets/1252d508-882c-4f66-bd3f-7223a2292262)

В проекте реализовано:
- Основной геймплейный цикл.
- Смена скинов в соответствующем разделе с индивидуальными звуковыми эффектами и анимациями.
- Настройка звука, вибрации и выбор языка.
- Показ рекламы после заданного числа проигрышей (Но не чаще 1 раза чем в заданный в конфиге промежуток)
- Сделана настраиваемая система подвижного фона с эффектом параллакса.
- Для анимаций использовался DoTween.
- Подгрузка ассетов с использованием Addresables.
- Общая архитектура базируется на инъекции зависимостей через VContainer и кастомную машину игровых состояний.
- Так же в проекте для самых разных задач, от анимаций интерфейса и до упорядоченной инициализации сущностей, активно используется UniTask.
- *** Реализован инструмент для удобного и быстрого создания новых уровней через отдельное окно в Editor через простой и понятный интерфейс.
  ...
