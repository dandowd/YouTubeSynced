
class Node<T> {
    public value: T;
    public next: Node<T>;

    constructor(elem: T) {
        this.value = elem;
    }
}

export class List<T> implements Iterable<T> {
    private head: Node<T>;
    private length = 0;

    constructor() {
        console.log("list constructed");
    }

    public GetLength() {
        return length;
    }

    public push(elem: T) {
        let current;
        let node = new Node(elem);

        if (this.head == undefined) {
            this.head = node;
            this.length++;
        }
        else {
            current = this.head;
            while (current.next) {
                current = current.next;
            }

            current.next = node;

            this.length++;
        }
    }

    public AddArray(elems: T[]) {

    }

    public GetElement(position: number) {
        let current: Node<T> = this.head;

        for (var i = 0; i < position; i++) {
            current = current.next;
        }

        return current.value;
    }

    public *[Symbol.iterator]() {
        console.log('iterator');
        for (var i = 0; i < this.length; i++) {
            yield this.GetElement(i);
        }
    }
}
