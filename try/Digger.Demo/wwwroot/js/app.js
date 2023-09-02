
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
    e.preventDefault();
    DotNet.invokeMethodAsync('Digger.Demo', 'OnKeyDown', serializeEvent(e));
});

document.addEventListener('keyup', function (e) {
    e.preventDefault();
    DotNet.invokeMethodAsync('Digger.Demo', 'OnKeyUp', serializeEvent(e));
});
