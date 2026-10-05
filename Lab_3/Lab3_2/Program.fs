open System

/// Считывает последовательность строк с клавиатуры.
/// Ввод завершается пустой строкой.
let rec readStringSequence () =
    seq {
        printf "Введите элемент последовательности (или Enter для завершения): "
        let input = Console.ReadLine()
        if not (String.IsNullOrWhiteSpace(input)) then
            yield input
            yield! readStringSequence ()
    }

/// Считывает вещественное число с клавиатуры с проверкой.
let rec readNumber () =
    let input = Console.ReadLine()
    match Double.TryParse input with
    | true, value -> value
    | false, _ ->
        printfn "Недопустимое значение. Введите число"
        readNumber ()

/// Считает, сколько раз значение встречается в последовательности.
let countOccurrences target items =
    items
    |> Seq.fold (fun count element -> if element = target then count + 1 else count) 0

[<EntryPoint>]
let main _ =
    let items = readStringSequence () |> Seq.cache

    printfn "Последовательность: %A" (items |> Seq.toList)

    printf "Введите число для поиска: "
    let comparisonValue = readNumber ()
    let searchString = string comparisonValue

    let occurrences = countOccurrences searchString items

    printfn "Число %s найдено: %d раз" searchString occurrences
    0