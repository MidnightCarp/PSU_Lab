open System

/// Считывает список вещественных чисел с клавиатуры.
/// Ввод завершается пустой строкой.
let rec readList () =
    printf "Введите число (или Enter для завершения): "
    let input = Console.ReadLine()
    if String.IsNullOrWhiteSpace(input) then
        []
    else
        match Double.TryParse input with
        | true, value -> value :: readList ()
        | false, _ ->
            printfn "Недопустимое значение. Введите число"
            readList ()

/// Возвращает первую цифру целого числа.
let firstDigit n =
    if n = 0 then
        0
    else
        // Модуль числа
        abs n
        |> string
        |> Seq.head
        |> string
        |> int

[<EntryPoint>]
let main _ =
    let oldList = readList ()
    printfn "Исходный список: %A" oldList

    let newList =
        oldList
        |> List.map int
        |> List.map firstDigit

    printfn "Новый список: %A" newList
    0