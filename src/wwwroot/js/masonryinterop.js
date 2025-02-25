export class MasonryInterop {
    static async init(containerSelector, itemSelector, percentPosition, transitionDuration) {       
        const masonry = new Masonry(containerSelector, {
            itemSelector: itemSelector,
            percentPosition: percentPosition,
            transitionDuration: transitionDuration
        });
    }
}

window.MasonryInterop = MasonryInterop;