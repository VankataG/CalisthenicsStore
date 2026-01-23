(function () {

    function showAjaxAlert(message, isSuccess) {
        const host = document.getElementById('ajaxAlertHost');
        if (!host) return;

        const wrapper = document.createElement('div');
        wrapper.className = `alert ${isSuccess ? 'alert-success' : 'alert-danger'} shadow`;
        wrapper.textContent = message;

        host.appendChild(wrapper);

        setTimeout(() => wrapper.remove(), 2500);
    }


    async function loadInitialCartCount() {
        const badge = document.getElementById('cartCountBadge');
        const countSpan = document.getElementById('cartCount');
        if (!badge || !countSpan) return;

        try {
            const res = await fetch('/Cart/GetCartCount');
            const data = await res.json();

            const count = data.cartCount || 0;
            countSpan.textContent = count;

            if (count > 0) badge.classList.remove('d-none');
            else badge.classList.add('d-none');
        } catch (err) {
            console.error('Failed to load cart count', err);
        }
    }


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
                showAjaxAlert(data.message || 'Added to cart!', true);
            } else {
                console.error(data.message);
                showAjaxAlert(data.message || 'Failed adding to cart!', false);
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

        loadInitialCartCount();
    });

})();
