window.initMasonry = (containerSelector, itemSelector, percentPosition, transitionDuration) => {
    var masonry = new Masonry(containerSelector, {
        percentPosition: percentPosition,
        transitionDuration: transitionDuration,
        itemSelector: itemSelector
    });
};