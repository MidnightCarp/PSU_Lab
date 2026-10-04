open System

/// Проверка вводимого вещественного числа
let rec readFloat prompt =
    printf "%s" prompt
    match Console.ReadLine() with
    | s when String.IsNullOrWhiteSpace(s) ->
        printfn "Ошибка: введите число."
        readFloat prompt
    | s ->
        match Double.TryParse(s) with
        | true, value -> value
        | _ ->
            printfn "Ошибка: некорректный формат числа."
            readFloat prompt

/// Проверка вводимого целого числа
let rec readInt prompt =
    printf "%s" prompt
    match Console.ReadLine() with
    | s when String.IsNullOrWhiteSpace(s) ->
        printfn "Ошибка: введите целое число."
        readInt prompt
    | s ->
        match Int32.TryParse(s) with
        | true, value -> value
        | _ ->
            printfn "Ошибка: некорректный формат целого числа."
            readInt prompt

/// Тип для представления комплексного числа
type ComplexNumber =
    { Real: float
      Imaginary: float }

/// Создание комплексного числа
let createComplex real imaginary =
    { Real = real; Imaginary = imaginary }

/// Сложение комплексных чисел
let add z1 z2 =
    { Real = z1.Real + z2.Real
      Imaginary = z1.Imaginary + z2.Imaginary }

/// Вычитание комплексных чисел
let subtract z1 z2 =
    { Real = z1.Real - z2.Real
      Imaginary = z1.Imaginary - z2.Imaginary }

/// Умножение комплексных чисел
let multiply z1 z2 =
    { Real = z1.Real * z2.Real - z1.Imaginary * z2.Imaginary
      Imaginary = z1.Real * z2.Imaginary + z1.Imaginary * z2.Real }

/// Деление комплексных чисел
let divide z1 z2 =
    let denominator = z2.Real * z2.Real + z2.Imaginary * z2.Imaginary
    if denominator = 0.0 then
        invalidArg "z2" "Деление на ноль невозможно."
    { Real = (z1.Real * z2.Real + z1.Imaginary * z2.Imaginary) / denominator
      Imaginary = (z1.Imaginary * z2.Real - z1.Real * z2.Imaginary) / denominator }

/// Модуль комплексного числа
let modulus z =
    sqrt (z.Real * z.Real + z.Imaginary * z.Imaginary)

/// Аргумент комплексного числа
let argument z =
    atan2 z.Imaginary z.Real

/// Возведение в степень по формуле Муавра
let moivre z n =
    if modulus z = 0.0 then
        { Real = 0.0; Imaginary = 0.0 }
    else
        let r = modulus z
        let theta = argument z
        let rN = r ** float n
        let tN = float n * theta
        { Real = rN * cos tN
          Imaginary = rN * sin tN }

/// Сопряжённое комплексное число
let conjugate z =
    { z with Imaginary = -z.Imaginary }

/// Преобразование комплексного числа в строку
let toString z =
    match z.Real, z.Imaginary with
    | r, 0.0 -> sprintf "%.2f" r
    | 0.0, i when i = 1.0 -> "i"
    | 0.0, i when i = -1.0 -> "-i"
    | 0.0, i -> sprintf "%.2fi" i
    | r, i when i > 0.0 -> sprintf "%.2f + %.2fi" r i
    | r, i -> sprintf "%.2f - %.2fi" r (abs i)

[<EntryPoint>]
let main _ =
    let real1 = readFloat "Введите действительную часть z1: "
    let imaginary1 = readFloat "Введите мнимую часть z1: "
    let real2 = readFloat "Введите действительную часть z2: "
    let imaginary2 = readFloat "Введите мнимую часть z2: "
    let power = readInt "Введите целую степень для z1: "

    let z1 = createComplex real1 imaginary1
    let z2 = createComplex real2 imaginary2

    printfn ""
    printfn "Результаты операций над комплексными числами:"
    printfn "z1 = %s" (toString z1)
    printfn "z2 = %s" (toString z2)
    printfn "z1 + z2 = %s" (toString (add z1 z2))
    printfn "z1 - z2 = %s" (toString (subtract z1 z2))
    printfn "z1 * z2 = %s" (toString (multiply z1 z2))
    printfn "z1 / z2 = %s" (toString (divide z1 z2))
    printfn "z1^%d = %s" power (toString (moivre z1 power))
    printfn "Модуль z1 = %.2f" (modulus z1)
    printfn "Аргумент z1 = %.2f рад" (argument z1)
    printfn "Сопряжённое z1 = %s" (toString (conjugate z1))
    0