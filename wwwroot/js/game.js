$(document).ready(function () {
    //--------------Prompt user before leaving page--------------//
    window.onbeforeunload = function () {
        return 'Are you sure you want to leave?';
    };

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
    $("#score").html(calculateScore());
    $("#totalCoin").html(coinsAvail());

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

            let posOk = checkPosition(position);

            //if position is suitable
            if (posOk) {
                // update gamedata (add building to layout, add usedCoin)
                gamedata["layout"][position] = choice;
                gamedata["coinUsed"]++;
                gamedata["turn"]++;
                placeBuilding(position);
                calculateTotalCoin(position);
                $("#score").html(calculateScore());
                $("#totalCoin").html(coinsAvail());
            }
            
            // Redirect Game when user is completed
            if (gamedata["turn"] >= totalcells || coinsAvail() <= 0) {
                // save score ==> send data to controller finish()

                let score = calculateScore();
                let formData = new FormData();
                formData.append("data", score);
                let request = new XMLHttpRequest();
                request.open("POST", "/Game/Finish");
                request.send(formData);
                request.onload = function () {
                    window.onbeforeunload = null;
                    if (request.status == 200) {
                        // Success!
                        // Redirect user to home
                        window.location.replace("/Home");
                    } else if (request.status == 201) {
                        // Redirect user to leaderboard if top 10
                        window.location.replace("/Game/Leaderboard");
                    } else {
                        // Error!
                        alert("Error. Something is wrong with the server! Try again later.");
                    }
                };
            }
            else if (posOk) {
                createChoices();
                displayChoices();
            }

        });


    }


    //--------------on click functions--------------//
    //Game Instructions
    $(".game-instructions").click(function (e) {
        e.preventDefault();
        displayGameInstructions();
    });

    //Save game
    $("#savegame").click(function (e) {
        e.preventDefault();
        saveData();
        document.querySelector(".nk-icon-close").click();

    });

    //--------------FUNCTIONS--------------//

    // place building in specific position
    function placeBuilding(position) {
        let cellid = cellname + position;
        var element = document.querySelector(`#game .table tbody td#${cellid}`);
        // add class if havent added
        if (!element.classList.contains(gamedata["layout"][position])) {
            element.classList.add(gamedata["layout"][position]);
        }
    }

    // fill board with buildings
    function fillBoard() {
        for (let i = 0; i < (totalcells); i++) {
            if (building.includes(gamedata["layout"][i])) {
                placeBuilding(i);
            }
        }
    }

    // calculate current score
    function calculateScore() {
        var score = 0;

        for (let i = 0; i < (totalcells); i++) {
            var neighborList = getNeighbor(i);
            var neighborNameList = neighborNames(neighborList);
            
            switch (gamedata["layout"][i]) {
                case "Residential":
                    if (neighborNameList.includes("Industry")) {
                        score += 1;
                    }
                    else {
                        for (var index in neighborNameList) {
                            let adjBuilding = neighborNameList[index];
                            if (adjBuilding == "Residential" || adjBuilding == "Commercial") {
                                score += 1;
                            } else if (adjBuilding == "Park") {
                                score += 2;
                            }
                        }
                    }
                    break;

                case "Industry":
                    score += 1;
                    break;

                case "Commercial":
                    for (var index in neighborNameList) {
                        let adjBuilding = neighborNameList[index];
                        if (adjBuilding == "Commercial") {
                            score += 1;
                        }
                    }
                    break;

                case "Park":
                    for (var index in neighborNameList) {
                        let adjBuilding = neighborNameList[index];
                        if (adjBuilding == "Park") {
                            score += 1;
                        }
                    }
                    break;

                case "Road":
                    if (neighborList.includes(epos(i)) && gamedata["layout"][epos(i)]  == "Road") {
                        score += 1;
                    }
                    if (neighborList.includes(wpos(i)) && gamedata["layout"][wpos(i)] == "Road") {
                        score += 1;
                    }
                    break;
            }
        }
        return score;
    }

    // convert neighboring positions to neighboring buildings names
    function neighborNames(neighborList) {
        var neighborNameList = [];
        for (var i in neighborList) {
            neighborNameList.push(gamedata["layout"][neighborList[i]]);
        }
        return neighborNameList;
    }

    // calculate Total coins
    function calculateTotalCoin(position) {
        // will be used every turn or when needed ???
        // not done just committing first
        var neighborList = getNeighbor(position)
        var neighborNameList = neighborNames(neighborList)

        switch (gamedata["layout"][position]) {
            
            case "Industry":

                for (var index in neighborNameList) {
                    let adjBuilding = neighborNameList[index];
                    if (adjBuilding == "Residential") {
                        gamedata["totalCoin"] += 1;
                    }
                }
                break;


            case "Commercial":
                for (var index in neighborNameList) {
                    let adjBuilding = neighborNameList[index];
                    if (adjBuilding == "Residential") {
                        gamedata["totalCoin"] += 1;
                    }
                }
                break;
        }
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
    function checkPosition(pos) {
        if (gamedata["turn"] == 1) {
            return true;
        }
        else {
            // check for building on pos --> have = return false
            if (building.includes(gamedata["layout"][pos])) {
                alert("This position is being used. Please try again.");
                return false;
            }

            // check for neighbors --> have = return true
            let neighbourlist = getNeighbor(pos);
            for (var i in neighbourlist) {
                if (building.includes(gamedata["layout"][neighbourlist[i]])) {
                    return true;
                }
            }
            alert("You must build next to an existing building.");
            return false;
        }
    }

    // return a list which contains the positions of neighbors
    function getNeighbor(pos) {
        let neighborList = [];
        //top left corner
        if (pos == 0) {
            neighborList.push(epos(pos));
            neighborList.push(spos(pos));
            return neighborList;
        }
        //top right corner
        else if (pos == (boardColumns - 1)) {
            neighborList.push(wpos(pos));
            neighborList.push(spos(pos));
            return neighborList;
        }
        //bottom left corner
        else if (pos == (boardColumns - 1)) {
            neighborList.push(epos(pos));
            neighborList.push(npos(pos));
            return neighborList;
        }
        //bottom right corner
        else if (pos == (boardColumns - 1)) {
            neighborList.push(wpos(pos));
            neighborList.push(npos(pos));
            return neighborList;
        }
        //top row
        else if (pos < boardColumns) {
            neighborList.push(epos(pos));
            neighborList.push(spos(pos));
            neighborList.push(wpos(pos));
            return neighborList;
        }
        //bottom row
        else if (pos >= (totalcells - boardColumns)) {
            neighborList.push(epos(pos));
            neighborList.push(npos(pos));
            neighborList.push(wpos(pos));
            return neighborList;
        }
        //left column
        else if ((pos % boardColumns) == 0) {
            neighborList.push(npos(pos));
            neighborList.push(epos(pos));
            neighborList.push(spos(pos));
            return neighborList;
        }
        //right column
        else if ((pos % boardColumns) == (boardColumns - 1)) {
            neighborList.push(npos(pos));
            neighborList.push(wpos(pos));
            neighborList.push(spos(pos));
            return neighborList;
        }
        //default
        else {
            neighborList.push(npos(pos));
            neighborList.push(epos(pos));
            neighborList.push(spos(pos));
            neighborList.push(wpos(pos));
            return neighborList;
        }

    }

    /*----------------START--------------------
    -----Simple functions for getNeighbor()--*/

    // The character before 'pos' stands for north, south, east and west of a position
    // e.g npos => north of position

    function npos(pos) {
        return (pos - boardColumns);
    }

    function spos(pos) {
        return (pos + boardColumns);
    }

    function epos(pos) {
        return (pos + 1);
    }

    function wpos(pos) {
        return (pos - 1);
    }

    /*----------------END--------------------
    -----Simple functions for getNeighbor()--*/

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
        request.onload = function () {
            if (request.status >= 200 && request.status < 300) {
                // Success!
                alert("Game is successfully saved");
            } else {
                // Error!
                alert("Error. Something is wrong with the server! Try again later.");
            }
        };
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