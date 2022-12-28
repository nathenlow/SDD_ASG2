$(document).ready(function () {
    // Define variables
    // length & breadth of game board
    const boardColumns = 20;
    const boardRows = 20;
    const totalcells = boardColumns * boardRows;
    const defaultCoin = 16;
    const building = ["Residential", "Industry", "Commercial", "Park", "Road"];
    const cellname = "cell";
    const rowname = "row";
    const choicename = "choice";

    // Get JSONArray in string and convert it
    var gamedatastr = document.querySelector("#game span#gamedata").innerHTML;
    var gamedata = JSON.parse(gamedatastr);
    // Init gamedata JSON array if empty
    if (!$.isArray(gamedata) || !gamedata.length) {

        // Create Json array
        gamedata = {
            turn: 1,
            totalCoin: defaultCoin,
            coinUsed: 0,
            choices: [],
            layout: []
        }

        // fill layout with empty strings
        for (let i = 0; i < (totalcells); i++) {
            gamedata["layout"].push("");
        }

        createChoices();
    }

    // Create Board
    for (let i = 1; i <= boardRows; i++) {
        // Create row
        let rowid = rowname + i;
        document.querySelector("#game .table tbody").innerHTML += `<tr id="${rowid}"></tr>`;

        // Create cells for that row
        for (let f = 1; f <= boardColumns; f++) {
            let cellid = cellname + (((i - 1) * boardColumns) + f);
            document.querySelector(`#game .table tbody tr#${rowid}`).innerHTML += `<td id="${cellid}"></td>`;
        }
    }

    // Display data
    fillBoard();
    displayChoices();

    const cellgrp = document.querySelectorAll("#game .table tbody td");
    for (const cell of cellgrp) {
        // ondragover
        cell.addEventListener('dragover', function (ev) {
            ev.preventDefault();
        });

        // ondrop
        /*---------------------------------------
        ---------WHEN CHOICE IS SELECTED---------
        ---------------------------------------*/
        cell.addEventListener('drop', function (ev) {
            ev.preventDefault();
            // get choice
            let data = ev.dataTransfer.getData("choiceid");
            data = data.replace(choicename, "");
            let choice = gamedata["choices"][parseInt(data) - 1];
            if (choice == undefined) {
                alert("You are only allowed to drag items from the bottom right");
                throw Error;
            }

            // get cell
            let cellid = ev.target.id;
            let position = parseInt(cellid.replace(cellname, ""));

            console.log(choice);
            console.log(position);

            //show loading screen

            // define variables
            let redoturn = false;

            // check if position is suitable
            if (checkPos(position)) {
                // update gamedata (add building to layout, add usedCoin)


            }

            // Redirect Game when user is completed
            if (gamedata["turn"] >= totalcells && coinsAvail() <= 0) {
                // save score ==> send data to controller finish() (append to score, remove saved game data)
                //show how many points he have and his position
            }

            if (redoturn == false) {
                gamedata["turn"]++;
                fillBoard();
                createChoices();
                displayChoices();
            }
        });


    }

    //--------------FUNCTIONS--------------//

    // fill board with buildings
    function fillBoard() {
        for (let i = 0; i < (totalcells); i++) {
            if (building.includes(gamedata["layout"][i])) {
                let cellid = cellname + (i + 1);
                var element = document.querySelector(`#game .table tbody td#${cellid}`);
                // add class if havent added
                if (!element.classList.contains(gamedata["layout"][i])) {
                    element.classList.add(gamedata["layout"][i]);
                }
            }
        }
    }


    // calculate current score
    function calculateScore() {
        // will be used every turn or when needed ???
    }

    // calculate Total coins
    function calculateTotalCoin() {
        // will be used every turn or when needed ???

    }

    // Select 2 random buildings for user two choose from
    function createChoices() {
        // Shuffle array
        const shuffled = building.sort(() => 0.5 - Math.random());

        // Get sub-array of first 2 elements after shuffled
        let selected = shuffled.slice(0, 2);
        gamedata["choices"] = selected;
    }

    function displayChoices() {
        document.querySelector(".floating-container").innerHTML = "";
        for (let i = 0; i < gamedata["choices"].length; i++) {
            let choiceid = choicename + (i + 1);
            document.querySelector(".floating-container").innerHTML += `<div class="float-element mb-3 pulse animated infinite ${gamedata["choices"][i]}" id="${choiceid}" draggable="true"></div>`;
        }
        createOnDrag();
    }

    // check if the position the user chose is acceptable
    function checkPos(pos) {
        if (gamedata["turn"] == 0) {
            return true;
        }
        else {
            // check for building on pos --> have = return false
            // check for buildings in North, South, East, West --> have = return true

        }
    }

    function coinsAvail() {
        let availcoins = gamedata["totalCoin"] - gamedata["coinUsed"];
        return availcoins;
    }

    // create form and post data
    function saveData() {
        // save score ==> send data to controller savedata()

    }

    function createOnDrag() {
        var choicecontainer = document.querySelectorAll(".floating-container .float-element");
        for (var choice of choicecontainer) {
            choice.addEventListener('dragstart', function (ev) {
                ev.dataTransfer.setData("choiceid", ev.target.id);
            });
        }
    }


})