const fs = require("fs");

(async () => {
    const input = fs.readFileSync("C:\\Users\\casva\\Documents\\adventofcode2022\\cas\\Day11\\input.txt").toString();
    const monkeyinput = input.split("\r\n\r\n");
    const monkeys = [];
    for(const monkey of monkeyinput) {
        const info = monkey.split("\r\n");
        const startingItems = info[1].replace('Starting items: ','').split(", ").map(i =>+i);
        const valueoperator = info[2].split(' ').pop();
        const operation = {
            operator: info[2].indexOf('*') > 1 ? 'TIMES' : 'ADD',
            value: valueoperator
        }
        const divisible = +info[3].split(' ').pop();
        const option1 = +info[4].split(' ').pop();
        const option2 = +info[5].split(' ').pop();
        monkeys.push({
            startingItems,
            operation,
            divisible,
            option1,
            option2,
            inspectedItems: 0,
        });
    }
    let modrator = monkeys.map(i => i.divisible).reduce((a,b) => a * b);
    for(let round = 1; round <= 10000; round++) {
        for(let monkey of monkeys) {
            for(let item of [...monkey.startingItems]) {
                let anxiety = monkey.operation.operator === 'TIMES' ? monkey.operation.value === 'old' ? item * item : item * +monkey.operation.value : item + +monkey.operation.value;             
                anxiety = anxiety % modrator;
                if((anxiety % monkey.divisible) === 0) {
                    monkeys[monkey.option1].startingItems.push(anxiety);
                } else {
                    monkeys[monkey.option2].startingItems.push(anxiety);
                }
                monkey.startingItems.shift();
                monkey.inspectedItems++;

            }
        }
    }

    const sortedmonkeys = monkeys.map(i => i.inspectedItems).sort((a,b) => b - a);
    console.log(sortedmonkeys[0] * sortedmonkeys[1]);

    
})();