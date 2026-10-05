open System

/// Двоичное дерево.
type BinaryTree<'T> =
    | Empty
    | Node of 'T * BinaryTree<'T> * BinaryTree<'T>

/// Печатает дерево в виде псевдографики.
let printTree tree =
    let rec printNode depth prefix isLeft node =
        match node with
        | Empty -> ()
        | Node (value, left, right) ->
            let branch =
                if depth > 0 then
                    if isLeft then "├─ " else "└─ "
                else
                    ""

            printfn "%s%s%A" prefix branch value

            let childPrefix =
                if depth = 0 then
                    ""
                else
                    prefix + (if isLeft then "│  " else "   ")

            printNode (depth + 1) childPrefix true left
            printNode (depth + 1) childPrefix false right

    printNode 0 "" true tree

/// Строит случайное дерево заданной глубины.
let rec buildTree (random: Random) depth maxDepth =
    if depth >= maxDepth then
        Empty
    else
        let value = random.Next(-10, 11)
        let left = buildTree random (depth + 1) maxDepth
        let right = buildTree random (depth + 1) maxDepth
        Node (value, left, right)

/// Применяет функцию к каждому значению дерева.
let rec mapTree f tree =
    match tree with
    | Empty -> Empty
    | Node (value, left, right) ->
        Node (f value, mapTree f left, mapTree f right)

[<EntryPoint>]
let main _ =
    let random = Random ()
    let maxDepth = 4
    let tree = buildTree random 0 maxDepth

    printfn "Дерево:"
    printTree tree

    printf "Введите элемент для добавления: "
    let input = Console.ReadLine ()

    let treeString = mapTree string tree
    let newTree = mapTree (fun s -> s + input) treeString

    printfn "Дерево с добавлением:"
    printTree newTree
    0