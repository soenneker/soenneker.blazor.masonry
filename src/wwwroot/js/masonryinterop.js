import '../../Soenneker.Blazor.Utils.ResourceLoader/js/resourceloader.js';

export class MasonryInitializer {
    static init(containerSelector, itemSelector, percentPosition, transitionDuration) {
        await ResourceLoader.loadScript('https://cdn.jsdelivr.net/npm/masonry-layout@4.2.2/dist/masonry.pkgd.min.js', "sha256-Nn1q/fx0H7SNLZMQ5Hw5JLaTRZp0yILA/FRexe19VdI=");
        await ResourceLoader.waitForVariable("Masonry");
       
        const masonry = new Masonry(containerSelector, {
            itemSelector: itemSelector,
            percentPosition: percentPosition,
            transitionDuration: transitionDuration
        });
    }
}