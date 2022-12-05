const fs = require('fs');

(async () => {
    const input = fs.readFileSync("C:\\Users\\casva\\Documents\\adventofcode2022\\cas\\Day5\\input.txt").toString();
    let crates = input.split('\r\n\r\n')[0];
    let moves = input.split('\r\n\r\n')[1];
    crates = crates.replace(new RegExp('\\r\\n', 'gm'),"X");
    crates = crates.replace(new RegExp('X', 'gm'),"|");
    crates = crates.replace(new RegExp('     ', 'gm')," X ");
    crates = crates.replace(new RegExp('    ', 'gm'),"X ");
    crates = crates.replace(new RegExp('X\\s\\|', 'gm')," X|");
    crates = crates.replace(new RegExp('XX', 'gm'),"X X");
    crates = crates.replace(new RegExp('  ', 'gm')," ");
    crates = crates.replace(new RegExp('\\s\\|', 'gm'),"|");
    crates = crates.replace(new RegExp('\\[', 'gm'),"");
    crates = crates.replace(new RegExp('\\]', 'gm'),"");

    const cratelines = crates.split("|");
    const cratesetup = [];
    const amountofcratesperline = cratelines[0].split(' ').length;
    for(let i = 0; i < amountofcratesperline; i++){
        cratesetup.push([]);
    }
    console.log(cratesetup);
    for(const line of cratelines) {
        const cratesInline = line.split(" ");
        let counter = 0;
        for(const crate of cratesInline) {
            if(crate === "X"){
                counter++;
                continue;
            }
            cratesetup[counter].unshift(crate);
            counter++;
        }
    }

    moves = moves.replace(new RegExp("move\\s",'gm'),"");
    moves = moves.replace(new RegExp("\\sfrom\\s","gm"),"|");
    moves = moves.replace(new RegExp("\\sto\\s","gm"),"|");
    console.log(moves);
    for(const move of moves.split("\r\n")){
        const conf = move.split("|");
        for(let i = 0; i < +conf[0]; i++) {
            const movedcrate = cratesetup[+conf[1] - 1].splice(cratesetup[+conf[1] - 1].length - 1,1);
            cratesetup[+conf[2] - 1].push(movedcrate[0]);
        }
    }
    console.log(cratesetup);
    let resultstring = '';
    for(const array of cratesetup) {
        resultstring += array[array.length -1];
    }
    console.log(resultstring);
    
})();