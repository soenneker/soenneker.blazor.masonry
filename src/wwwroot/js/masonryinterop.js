export class MasonryInterop {
    static instances = new Map();

    static init(id, containerSelector, itemSelector, percentPosition = true, transitionDuration = 300) {
        try {
            if (this.instances.has(id)) {
                console.warn(`Masonry instance with id '${id}' already exists.`);
                return;
            }

            const masonry = new Masonry(containerSelector, {
                itemSelector: itemSelector,
                percentPosition: percentPosition,
                transitionDuration: transitionDuration
            });

            this.instances.set(id, masonry);
        } catch (error) {
            console.error(`Error initializing Masonry with id '${id}':`, error);
        }
    }

    static layout(id) {
        const masonry = this.instances.get(id);
        if (masonry) {
            masonry.layout();
        } else {
            console.warn(`Masonry instance with id '${id}' not found.`);
        }
    }

    static destroy(id) {
        const masonry = this.instances.get(id);
        if (masonry) {
            masonry.destroy();
            this.instances.delete(id);
        } else {
            console.warn(`Masonry instance with id '${id}' not found or already destroyed.`);
        }
    }
}

window.MasonryInterop = MasonryInterop;
