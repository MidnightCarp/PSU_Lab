open System

/// Считывает последовательность вещественных чисел с клавиатуры.
/// Ввод завершается пустой строкой.
let rec readSequence () =
    seq {
        printf "Введите число (или Enter для завершения): "
        let input = Console.ReadLine()
        if not (String.IsNullOrWhiteSpace(input)) then
            match Double.TryParse input with
            | true, value -> yield value
            | false, _ -> printfn "Недопустимое значение. Повторите ввод"
            yield! readSequence ()
    }

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
    // Seq.cache запоминает введённые числа, чтобы не читать их повторно
    let oldSequence = readSequence () |> Seq.cache
    printfn "Исходная последовательность: %A" (oldSequence |> Seq.toList)

    let newSequence =
        oldSequence
        |> Seq.map int
        |> Seq.map firstDigit

    printfn "Новая последовательность: %A" (newSequence |> Seq.toList)
    0