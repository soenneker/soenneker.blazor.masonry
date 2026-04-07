const masonryInstances = new Map();
let masonryObserver = null;

export function init(id, containerSelector, itemSelector, columnWidthSelector, percentPosition = true, transitionDuration = 300) {
    try {
        if (masonryInstances.has(id)) {
            console.warn(`Masonry instance with id '${id}' already exists.`);
            return;
        }

        const masonry = new Masonry(containerSelector, {
            itemSelector: itemSelector,
            columnWidth: columnWidthSelector,
            percentPosition: percentPosition,
            transitionDuration: transitionDuration
        });

        masonryInstances.set(id, masonry);
    } catch (error) {
        console.error(`Error initializing Masonry with id '${id}':`, error);
    }
}

export function layout(id) {
    const masonry = masonryInstances.get(id);
    if (masonry) {
        masonry.layout();
    } else {
        console.warn(`Masonry instance with id '${id}' not found.`);
    }
}

export function destroy(id) {
    const masonry = masonryInstances.get(id);
    if (masonry) {
        masonry.destroy();
        masonryInstances.delete(id);
    } else {
        console.warn(`Masonry instance with id '${id}' not found or already destroyed.`);
    }
}

export function createObserver(elementId) {
    const target = document.getElementById(elementId);
    if (!target || !target.parentNode) {
        console.warn(`Element with id '${elementId}' not found or has no parent.`);
        return;
    }

    masonryObserver = new MutationObserver((mutations) => {
        const targetRemoved = mutations.some(mutation => Array.from(mutation.removedNodes).includes(target));

        if (targetRemoved) {
            destroy(elementId);

            masonryObserver.disconnect();
            masonryObserver = null;
        }
    });

    masonryObserver.observe(target.parentNode, { childList: true });
}
