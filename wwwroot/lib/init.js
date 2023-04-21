
window.GetValueByID = function (id) {
    console.log(id);
    window.Cache = document.getElementById(id).value.split("\n");
    return window.Cache.length;
}

window.PopLines = function () {
    var length = window.Cache.length;
    if (length > 100) {
        var poped = window.Cache.slice(0, 100);
        window.Cache = window.Cache.slice(100, length);
        return poped;
    }
    else {
        return window.Cache;
    }
}