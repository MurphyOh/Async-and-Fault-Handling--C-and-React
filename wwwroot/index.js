const serverUrl = "https://localhost:7151/api/mario";
const stepSize = (window.innerWidth * 5) / 100;
let marioPosition = 0;
let gameRunning = true; // Initialize gameRunning

document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("start-btn").addEventListener("click", startGame);
});

const getRandomAction = async () => {
    const actions = ["walk", "jump", "wait", "run"];
    const randomAction = actions[Math.floor(Math.random() * actions.length)];
    return await performAction(randomAction);
};

const startGame = async () => {
    document.getElementById("start-btn").disabled = true;

    while (gameRunning) {
        const result = await getRandomAction();
        await sleep(1000);
        if (checkGameOver()) {
            gameRunning = false;
            document.getElementById("message").textContent = "You Won!";
        }
    }
};

const performAction = async (action) => {
    const response = await fetch(serverUrl + "/" + action);
    const responseJson = await response.json();

    if (responseJson.message === "Mario died") {
        gameRunning = false;
        document.getElementById("message").textContent = JSON.stringify(responseJson.message);
        return;
    }

    document.getElementById("message").textContent = JSON.stringify(responseJson.message);
    moveMario(action);

};

const moveMario = (action) => {
    const mario = document.getElementById("mario");

    if (action === "jump") {
        marioPosition += stepSize;
    } else if (action === "run") {
        marioPosition += stepSize * 2;
    } else if (action === "walk") {
        marioPosition += stepSize;
    } else if (action === "wait") {
        sleep(1000);
    }
    mario.style.left = marioPosition + "px";

};

const sleep = (ms) => new Promise(resolve => setTimeout(resolve, ms));


const checkGameOver = () => {
    const mario = document.getElementById("mario");
    const flag = document.getElementById("flag");

    // Use offsetLeft to get the position relative to the parent container
    const marioLeft = mario.offsetLeft;
    const flagLeft = flag.offsetLeft;

    return marioLeft >= flagLeft;
};

