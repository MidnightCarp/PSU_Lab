open System

/// Считывает список строк с клавиатуры.
/// Ввод завершается пустой строкой.
let rec readStringList () =
    printf "Введите элемент списка (Enter для завершения): "
    let input = Console.ReadLine()
    if String.IsNullOrWhiteSpace(input) then
        []
    else
        input :: readStringList ()

/// Считывает вещественное число с клавиатуры с проверкой.
let rec readNumber () =
    let input = Console.ReadLine()
    match Double.TryParse input with
    | true, value -> value
    | false, _ ->
        printfn "Недопустимое значение. Введите число"
        readNumber ()

/// Считает, сколько раз элемент встречается в списке.
let countOccurrences target items =
    items
    |> List.fold (fun count element -> if element = target then count + 1 else count) 0

[<EntryPoint>]
let main _ =
    let items = readStringList ()

    printf "Введите число для поиска: "
    let comparisonValue = readNumber ()
    let searchString = string comparisonValue

    printfn "Список: %A" items

    let occurrences = countOccurrences searchString items

    printfn "Число %s найдено: %d раз" searchString occurrences
    0