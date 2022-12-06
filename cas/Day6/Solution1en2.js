const fs = require('fs');

(async () => {
    const input = fs.readFileSync("C:\\Users\\cas.vangool\\adventofcode\\adventofcode2022\\cas\\Day6\\input.txt").toString();
    
    let keyarray = [];
    for(let i = 0; i < input.length; i++){
        if(!keyarray.includes(input[i])) {
            keyarray.push(input[i]);
        } else {
            const indexduplicate = keyarray.indexOf(input[i]);
            for(let b = 0; b <= indexduplicate; b++){
                keyarray.shift();
            }
            keyarray.push(input[i]);
        }

        if(keyarray.length === 14){
            console.log(i + 1);
            break;
        }

        if(keyarray.length > 14) {
            keyarray.shift();
        }
    }
})();