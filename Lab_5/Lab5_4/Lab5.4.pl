ivan_ok(R) :-
    ( member(spartak-1, R) -> T1 = true ; T1 = false ),
    ( member(cska-4,    R) -> T2 = true ; T2 = false ),
    T1 \= T2.

sergey_ok(R) :-
    ( member(spartak-P, R), P >= 3 -> T1 = true ; T1 = false ),
    ( member(cska-2,     R) -> T2 = true ; T2 = false ),
    T1 \= T2.

petr_ok(R) :-
    ( member(dinamo-1, R) -> T1 = true ; T1 = false ),
    ( member(cska-P,     R), P =< 3 -> T2 = true ; T2 = false ),
    T1 \= T2.

aleksey_ok(R) :-
    ( member(dinamo-2, R) -> T1 = true ; T1 = false ),
    ( member(rotor-4,    R) -> T2 = true ; T2 = false ),
    T1 \= T2.

valid_ranking(R) :-
    ivan_ok(R),
    sergey_ok(R),
    petr_ok(R),
    aleksey_ok(R).

permutation_places(Teams, Ranking) :-
    Places = [1,2,3,4],
    permutation(Places, PermPlaces),
    pair_teams_places(Teams, PermPlaces, Ranking).

pair_teams_places([], [], []).
pair_teams_places([Team|Teams], [Place|Places], [Team-Place|Rest]) :-
    pair_teams_places(Teams, Places, Rest).

all_rankings(Rankings) :-
    Teams = [spartak, cska, dinamo, rotor],
    findall(R, permutation_places(Teams, R), Rankings).


solve_and_show :-
    writeln('Solution to the logical problem about the football tournament:'),
    writeln(''),
    all_rankings(All),
    member(R, All),
    valid_ranking(R),
    writeln('Found solution:'),
    show_result(R),
    writeln(''),
    writeln('Program finished. ').

show_result(Ranking) :-
    sort(2, @=<, Ranking, Sorted),
    show_places(Sorted, 1).

show_places([], _).
show_places([Team-Place|Rest], Place) :-
    format('Place ~w - ~w~n', [Place, Team]),
    NextPlace is Place + 1,
    show_places(Rest, NextPlace).

wait_for_user :-
    get_char(_).

main :-
    catch(
        (solve_and_show, wait_for_user),
        Error,
        (writeln('An error occurred:'), writeln(Error))
    ).

:- initialization(main).
