const fs = require("fs");

const Rock = "Rock";
const Paper = "Paper";
const Sciccors = "Sciccors"
const opponentMap = {
    "A": Rock,
    "B": Paper,
    "C": Sciccors
};

const awnserMap = {
    "X": Rock,
    "Y": Paper,
    "Z": Sciccors
};



(async () => {
    const input = fs.readFileSync("C:\\Users\\casva\\Documents\\adventofcode2022\\cas\\Day2\\puzzleinput.txt");
    const lines = input.toString().split("\r\n");
    const games = lines.map(item => {
        const items = item.split(" ");
        return {
            opponent: opponentMap[items[0]],
            awnser: awnserMap[items[1]]
        };
    });
    let totalscore = 0;
    for(let game of games) {
        totalscore += calculatePointsFromOptions(game.awnser);
        totalscore += checkWinLoseDraw(game.opponent, game.awnser);
    }
    console.log(totalscore)
})();

function calculatePointsFromOptions(awnser){
    if(awnser === Rock) {
        return 1;
    } else if (awnser === Paper){
        return 2;
    }
    return 3;
}

function checkWinLoseDraw(opponent, awnser) {
    if(opponent === awnser) {
        return 3;
    }

    if((awnser === Rock && opponent === Sciccors) || (awnser === Paper && opponent === Rock) || (awnser === Sciccors && opponent === Paper) ){
        return 6;
    } 

    return 0;
}