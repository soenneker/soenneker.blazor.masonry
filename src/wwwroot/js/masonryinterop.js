export class MasonryInterop {
    constructor() {
        this.instances = new Map();
        this.observer = null;
    }

    init(id, containerSelector, itemSelector, percentPosition = true, transitionDuration = 300) {
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

    layout(id) {
        const masonry = this.instances.get(id);
        if (masonry) {
            masonry.layout();
        } else {
            console.warn(`Masonry instance with id '${id}' not found.`);
        }
    }

    destroy(id) {
        const masonry = this.instances.get(id);
        if (masonry) {
            masonry.destroy();
            this.instances.delete(id);
        } else {
            console.warn(`Masonry instance with id '${id}' not found or already destroyed.`);
        }
    }

    createObserver(elementId) {
        const target = document.getElementById(elementId);
        if (!target || !target.parentNode) {
            console.warn(`Element with id '${elementId}' not found or has no parent.`);
            return;
        }

        this.observer = new MutationObserver((mutations) => {
            const targetRemoved = mutations.some(mutation => Array.from(mutation.removedNodes).includes(target));

            if (targetRemoved) {
                this.destroy(elementId);

                this.observer.disconnect();
                this.observer = null;
            }
        });

        this.observer.observe(target.parentNode, { childList: true });
    }
}

window.MasonryInterop = new MasonryInterop();