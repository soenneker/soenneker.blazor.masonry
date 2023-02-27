window.initMasonry = (selector, percentPosition, transitionDuration) => {
    var masonry = new Masonry(selector, {
        percentPosition: percentPosition,
        transitionDuration: transitionDuration
    });
};