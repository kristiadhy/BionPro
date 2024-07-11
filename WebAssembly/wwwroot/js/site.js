window.getScreenWidth = () => {
    return window.innerWidth;
};

function selectAllText(elementId) {
    var element = document.getElementById(elementId);
    if (element) {
        element.select();
    }
}