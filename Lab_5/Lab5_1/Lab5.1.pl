:- use_module(library(readutil)).
:- initialization(main).

main :-
    loop_digits.

loop_digits :-
    repeat,
    write('Enter a natural number N or q to exit: '), flush_output,
    read_line_to_string(user_input, S),
    ( S == "q" ->
        writeln('Exiting program.'), !
    ; ( number_string(N, S), integer(N), N >= 0 ->
          number_codes(N, Codes),
          maplist(code_digit, Codes, Digits),
          format('List of digits: ~w~n', [Digits])
      ; format('Error: "~w" is not a natural number.~n', [S])
      ),
      fail
    ).

code_digit(Code, Digit) :-
    Digit is Code - 0'0.
