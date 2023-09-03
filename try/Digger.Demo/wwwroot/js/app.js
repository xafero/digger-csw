
function serializeEvent(e) {
    if (e) {
        var o = {
            altKey: e.altKey,
            charCode: e.charCode,
            code: e.code,
            composed: e.composed,
            ctrlKey: e.ctrlKey,
            metaKey: e.metaKey,
            key: e.key,
            keyCode: e.keyCode,
            repeat: e.repeat,
            shiftKey: e.shiftKey,
            timeStamp: e.timeStamp,
            type: e.type,
            which: e.which
        };
        return o;
    }
};

document.addEventListener('keydown', function (e) {
    DotNet.invokeMethodAsync('Digger.Demo', 'OnKeyDown', serializeEvent(e));
});

document.addEventListener('keyup', function (e) {
    DotNet.invokeMethodAsync('Digger.Demo', 'OnKeyUp', serializeEvent(e));
});

function doCanvasSetup() {
    const canvas = document.getElementById('webCanvas');
    const canvasDiv = document.getElementById('canvas-container');

    function resizeCanvas() {
        canvas.width = canvasDiv.clientWidth;
        canvas.height = canvasDiv.clientHeight;
    };

    window.addEventListener('resize', resizeCanvas);
    window.addEventListener('DOMContentLoaded', resizeCanvas);
    resizeCanvas();
}

let listenToJoy = false;
let lastJoyStates = [];

function getJoyStates(gamepad) {
    const array = [];
    for (const button of gamepad.buttons) {
        array.push(button.value);
    }
    return array;
}

function getArrayDiff(first, second) {
    const array = [];
    const max = Math.max(first.length, second.length);
    for (let i = 0; i < max; i++) {
        const firstI = first[i] ?? 0;
        const secondI = second[i] ?? 0;
        let res;
        if (firstI === secondI) res = null;
        else res = secondI - firstI;
        array.push(res);
    }
    return array;
}

function sendKey(btnVal, btnKey) {
    const evt = { Key: btnKey };
    if (btnVal === 1)
        DotNet.invokeMethodAsync('Digger.Demo', 'OnKeyDown', evt);
    else
        DotNet.invokeMethodAsync('Digger.Demo', 'OnKeyUp', evt);
}

function startJoyListen() {
    const gamepads = navigator.getGamepads();
    for (const gamepad of gamepads) {
        if (gamepad) {
            const myJoyStates = getJoyStates(gamepad);
            const myJoyDiff = getArrayDiff(lastJoyStates, myJoyStates);
            const btnFire = myJoyDiff[0];
            if (btnFire) sendKey(btnFire, "Control");
            const btnEsc = myJoyDiff[9];
            if (btnEsc) sendKey(btnEsc, "Escape");
            const btnMinus = myJoyDiff[4];
            if (btnMinus) sendKey(btnMinus, "-");
            const btnPlus = myJoyDiff[5];
            if (btnPlus) sendKey(btnPlus, "+");
            const btnLeft = myJoyDiff[14];
            if (btnLeft) sendKey(btnLeft, "ArrowLeft");
            const btnRight = myJoyDiff[15];
            if (btnRight) sendKey(btnRight, "ArrowRight");
            const btnUp = myJoyDiff[12];
            if (btnUp) sendKey(btnUp, "ArrowUp");
            const btnDown = myJoyDiff[13];
            if (btnDown) sendKey(btnDown, "ArrowDown");
            lastJoyStates = myJoyStates;
        }
    }
    if (!listenToJoy) return;
    requestAnimationFrame(startJoyListen);
}

window.addEventListener("gamepadconnected", (e) => {
    listenToJoy = true;
    requestAnimationFrame(startJoyListen);
});

window.addEventListener("gamepaddisconnected", (e) => {
    listenToJoy = false;
});
