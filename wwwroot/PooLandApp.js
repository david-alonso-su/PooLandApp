window.popupButtonClick = (id) => {
    DotNet.invokeMethodAsync('PooLandApp', 'PopupButtonClick', id);
};

window.getWindowDimensions = function () {
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
};