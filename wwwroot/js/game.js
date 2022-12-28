$(document).ready(function () {
    // Display game instructions
    displayGameInstructions();

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

    // Get JSON obj from html
    var gamedatastr = document.querySelector("#game span#gamedata").innerHTML;
    var gamedata = {};

    // Init gamedata JSON object if empty
    if (gamedatastr == "{}") {

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
    } else {
        gamedata = JSON.parse(gamedatastr);
    }

    // Create Board
    for (let i = 0; i < boardRows; i++) {
        // Create row
        let rowid = rowname + i;
        document.querySelector("#game .table tbody").innerHTML += `<tr id="${rowid}"></tr>`;

        // Create cells for that row
        for (let f = 0; f < boardColumns; f++) {
            let cellid = cellname + ((i * boardColumns) + f);
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
            HighlightArea(ev.target.id, true);
        });

        // dragleave
        cell.addEventListener('dragleave', function (ev) {
            HighlightArea(ev.target.id, false);
        });

        // ondrop
        /*---------------------------------------
        ---------WHEN CHOICE IS SELECTED---------
        ---------------------------------------*/
        cell.addEventListener('drop', function (ev) {
            ev.preventDefault();

            // get cell id
            let cellid = ev.target.id;
            HighlightArea(cellid, false)

            // get choice
            let data = ev.dataTransfer.getData("choiceid");
            data = data.replace(choicename, "");
            let choice = gamedata["choices"][parseInt(data)];

            if (choice == undefined) {
                alert("You are only allowed to drag items from the bottom right");
                throw Error;
            }

            // get cell position
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

                /*let formData = new FormData();
                formData.append("data", score);
                let request = new XMLHttpRequest();
                request.open("POST", "/Game/Finish");
                request.send(formData);*/
            }

            if (redoturn == false) {
                gamedata["turn"]++;
                fillBoard();
                createChoices();
                displayChoices();
            }
        });


    }


    //--------------Game Instructions--------------//
    $(".game-instructions").click(function (e) {
        e.preventDefault();
        displayGameInstructions();
    })
    
    //--------------FUNCTIONS--------------//

    // fill board with buildings
    function fillBoard() {
        for (let i = 0; i < (totalcells); i++) {
            if (building.includes(gamedata["layout"][i])) {
                let cellid = cellname + i;
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
        document.querySelector(".floating-container .building-option").innerHTML = "";
        for (let i = 0; i < gamedata["choices"].length; i++) {
            let choiceid = choicename + i;
            document.querySelector(".floating-container .building-option").innerHTML += `<div class="float-element mb-3 pulse animated infinite ${gamedata["choices"][i]}" id="${choiceid}" draggable="true"></div>`;
        }
        createOnDrag();
    }

    // check if the position the user chose is acceptable
    function checkPos(pos) {
        if (gamedata["turn"] == 1) {
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
        let formData = new FormData();
        formData.append("data", JSON.stringify(gamedata));
        let request = new XMLHttpRequest();
        request.open("POST", "/Game/SaveData");
        request.send(formData);
    }

    function createOnDrag() {
        var choicecontainer = document.querySelectorAll(".floating-container .building-option .float-element");
        for (var choice of choicecontainer) {
            choice.addEventListener('dragstart', function (ev) {
                ev.dataTransfer.setData("choiceid", ev.target.id);
            });
        }
    }

    function HighlightArea(id, isTrue) {
        isTrue == true ? document.getElementById(id).style.backgroundColor = "dodgerblue" :
            document.getElementById(id).style.backgroundColor = "";
    }

    // display game instruction modal
    function displayGameInstructions() {
        $("#instruction-modal").modal("show");
    }

})