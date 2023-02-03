window.initMasonry = (selector, percentPosition, transitionDuration) => {
    document.querySelector(selector).masonry({
        percentPosition: percentPosition,
        transitionDuration: transitionDuration
    });
};