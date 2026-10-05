% min_list(+List, -Min)
% Находит минимальный элемент списка.
min_list([X], X).                          % База: список из одного элемента.
min_list([H|T], Min) :-                    % Рекурсия: список из головы и хвоста.
    min_list(T, TailMin),                  % Находим минимум хвоста.
    (H < TailMin -> Min = H ; Min = TailMin). % Сравниваем голову с минимумом хвоста.

% count_occurrences(+List, +Element, -Count)
% Считает, сколько раз Element встречается в List.
count_occurrences([], _, 0).               % База: пустой список — 0 вхождений.
count_occurrences([H|T], E, Count) :-      % Рекурсия.
    count_occurrences(T, E, Count1),       % Считаем в хвосте.
    (H =:= E -> Count is Count1 + 1 ; Count = Count1). % Если голова равна E, увеличиваем счётчик.

% min_count(+List, -Count)
% Основной предикат: определяет, сколько раз встречается минимальный элемент.
min_count(List, Count) :-
    min_list(List, Min),                   % 1. Находим минимум.
    count_occurrences(List, Min, Count).   % 2. Считаем его вхождения.