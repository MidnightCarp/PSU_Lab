:- use_module(library(lists)).
:- initialization(main).

main :-
    loop_count_min.

loop_count_min :-
    repeat,
      write('Enter a non-empty list of numbers [1,2,3,...] or q to exit: '), flush_output,
      read(Input),
      ( Input == q ->
          writeln('Exiting program.'), !
      ; ( is_list(Input), Input \= [] ->
            min_list(Input, Min),
            count_occurrences(Input, Min, Count),
            format('Minimum element ~w occurs ~w times.~n', [Min, Count])
        ; format('Error: enter a non-empty list or q.~n', [])
        ),
        fail
      ).

count_occurrences([], _, 0).
count_occurrences([H|T], E, C) :-
    H =:= E, !,
    count_occurrences(T, E, C1),
    C is C1 + 1.
count_occurrences([_|T], E, C) :-
    count_occurrences(T, E, C).
