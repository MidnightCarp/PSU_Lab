:- initialization(main).

main :-
    loop_symdiff.

loop_symdiff :-
    write('Enter the first set (list) or q to exit: '),
    read(A),
    ( A == q ->
        writeln('Exit.')
    ; write('Enter the second set or q to exit: '),
      read(B),
      ( (B == 'q' ; B == 'Q') ->
          writeln('Exit.')
      ; difference(A, B, D1),
        difference(B, A, D2),
        append(D1, D2, SymDiff),
        format('Symmetric difference: ~w~n', [SymDiff]),
        loop_symdiff   % recursive loop instead of repeat/fail
      )
    ).

difference([], _, []).
difference([H|T], B, [H|R]) :-
    \+ member(H, B),
    difference(T, B, R).
difference([_|T], B, R) :-
    difference(T, B, R).