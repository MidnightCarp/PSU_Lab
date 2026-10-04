open System

let rec readSymbol () : char =
    let input = Console.ReadLine()
    match Char.TryParse input with
    | true, value -> value
    | false, _ ->
        printfn "Недопустимое значение. Повторите ввод"
        readSymbol ()

let rec readNumber () : int =
    let input = Console.ReadLine()
    match Int32.TryParse input with
    | true, value when value >= 0 -> value
    | true, _ ->
        printfn "Число не должно быть отрицательным"
        readNumber ()
    | false, _ ->
        printfn "Недопустимое значение. Повторите ввод"
        readNumber ()

printfn "Введите первый символ"
let char1 = readSymbol ()

printfn "Введите второй символ"
let char2 = readSymbol ()

printfn "Сколько чередований в списке?"
let n = readNumber ()

let alternationList =
    [ for i in 1 .. n * 2 -> if i % 2 = 0 then char2 else char1 ]

printfn "%A" alternationList