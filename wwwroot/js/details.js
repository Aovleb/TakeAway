document.addEventListener('DOMContentLoaded', function () {
    // Sauvegarder la position de scroll avant de cliquer sur "Add to Cart"
    const addToCartButtons = document.querySelectorAll('.add-to-cart');
    addToCartButtons.forEach(button => {
        button.addEventListener('click', function () {
            sessionStorage.setItem('scrollPosition', window.scrollY);
        });
    });

    // Restaurer la position de scroll après le chargement de la page
    const savedScrollPosition = sessionStorage.getItem('scrollPosition');
    if (savedScrollPosition) {
        window.scrollTo(0, parseInt(savedScrollPosition));
        sessionStorage.removeItem('scrollPosition');
    }
});