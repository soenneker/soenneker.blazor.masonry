const masonryInstances = new Map();
const masonryObservers = new Map();

export function init(id, containerSelector, itemSelector, columnWidthSelector, percentPosition = true, transitionDuration = "0.2s") {
    const existing = masonryInstances.get(id);
    if (existing) {
        existing.destroy();
    }

    const masonry = new Masonry(containerSelector, {
        itemSelector,
        columnWidth: columnWidthSelector,
        percentPosition,
        transitionDuration
    });

    masonryInstances.set(id, masonry);
}

export function layout(id) {
    const masonry = masonryInstances.get(id);
    if (!masonry) {
        throw new Error(`Masonry instance '${id}' has not been initialized.`);
    }

    masonry.layout();
}

export function destroy(id) {
    const masonry = masonryInstances.get(id);
    if (masonry) {
        masonry.destroy();
        masonryInstances.delete(id);
    }

    const observer = masonryObservers.get(id);
    if (observer) {
        observer.disconnect();
        masonryObservers.delete(id);
    }
}

export function createObserver(elementId) {
    const target = document.getElementById(elementId);
    if (!target?.parentNode) {
        throw new Error(`Masonry element '${elementId}' was not found or has no parent.`);
    }

    masonryObservers.get(elementId)?.disconnect();

    const observer = new MutationObserver(mutations => {
        const targetRemoved = mutations.some(mutation => Array.from(mutation.removedNodes).includes(target));

        if (targetRemoved) {
            destroy(elementId);
        }
    });

    observer.observe(target.parentNode, { childList: true });
    masonryObservers.set(elementId, observer);
}
