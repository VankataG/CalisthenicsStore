(function () {

    function setCartCount(count) {
        const badge = document.getElementById('cartCountBadge');
        const countSpan = document.getElementById('cartCount');
        if (!badge || !countSpan) return;

        countSpan.textContent = count;
        if (count > 0) badge.classList.remove('d-none');
        else badge.classList.add('d-none');
    }

    async function handleAddToCartSubmit(e) {
        e.preventDefault();

        const form = e.currentTarget;
        const button = form.querySelector('button[type="submit"]');
        const oldText = button ? button.innerHTML : '';

        try {
            if (button) { button.disabled = true; button.innerHTML = 'Adding...'; }

            const res = await fetch(form.action, {
                method: 'POST',
                body: new FormData(form) // includes antiforgery token field automatically
            });

            const data = await res.json();

            if (data.success) {
                setCartCount(data.cartCount);
            } else {
                console.error(data.message);
            }
        } catch (err) {
            console.error(err);
        } finally {
            if (button) { button.disabled = false; button.innerHTML = oldText; }
        }
    }

    document.addEventListener('DOMContentLoaded', () => {
        document.querySelectorAll('.add-to-cart-ajax')
            .forEach(f => f.addEventListener('submit', handleAddToCartSubmit));
    });

})();
