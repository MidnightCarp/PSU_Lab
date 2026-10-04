open System

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

let sumOfDigits (num: int) : int =
    let rec sum acc x =
        match x with
        | 0 -> acc
        | _ -> sum (acc + x % 10) (x / 10)
    sum 0 num

printfn "Введите натуральное число"
let n = readNumber ()

let result = sumOfDigits n

printfn "Сумма цифр: %d" result