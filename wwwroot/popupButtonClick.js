window.popupButtonClick = (id) => {
    DotNet.invokeMethodAsync('PooLandApp', 'PopupButtonClick', id);
};