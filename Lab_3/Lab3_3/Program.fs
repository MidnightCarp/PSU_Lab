open System
open System.IO

/// Рекурсивно возвращает последовательность путей
/// ко всем *.txt-файлам в указанном каталоге и его подкаталогах.
let rec allTextFiles (directory: string) : seq<string> =
    seq {
        // Все *.txt-файлы в текущем каталоге
        yield! Directory.EnumerateFiles(directory, "*.txt", SearchOption.TopDirectoryOnly)

        // Рекурсивно спускаемся в каждый подкаталог
        for subdir in Directory.EnumerateDirectories directory do
            yield! allTextFiles subdir
    }

/// Запрашивает у пользователя путь к существующему каталогу.
/// Повторяет ввод, пока путь не будет корректным.
let rec readDirectoryPath () =
    let input = Console.ReadLine().Trim()
    if not (Directory.Exists input) then
        printfn "Указанный каталог не найден."
        printf "Введите путь к каталогу: "
        readDirectoryPath ()
    else
        input

/// Печатает все элементы последовательности.
/// Если последовательность пуста, выводит сообщение.
let printSequence sequence =
    if Seq.isEmpty sequence then
        printfn "В каталоге и подкаталогах нет файлов с расширением .txt"
    else
        sequence |> Seq.iter (printfn "%s")

[<EntryPoint>]
let main _ =
    printf "Введите путь к каталогу: "
    let rootDir = readDirectoryPath ()
    let result = allTextFiles rootDir
    printSequence result
    0