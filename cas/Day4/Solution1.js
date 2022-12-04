const fs = require('fs');

(async () => {
    const input = fs.readFileSync("C:\\Users\\casva\\Documents\\adventofcode2022\\cas\\Day4\\input.txt");
    const pairs = input.toString().split("\r\n");
    let duplicates = 0;
    for(const pair of pairs) {
        const assignments = pair.split(',').map(item => {
            const i = item.split('-');
            return {
                lowest: +i[0],
                highest: +i[1]
            };
        });
        if((assignments[0].lowest >= assignments[1].lowest && assignments[0].highest <= assignments[1].highest)
            || assignments[0].lowest <= assignments[1].lowest && assignments[0].highest >= assignments[1].highest){
            duplicates++;
        } 
    }
    console.log(duplicates);
})();