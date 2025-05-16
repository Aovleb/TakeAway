document.addEventListener('DOMContentLoaded', function () {
    // Gestion des messages flash
    const flashMessages = document.querySelectorAll('.flash-message');
    flashMessages.forEach(message => {
        setTimeout(() => {
            message.style.transform = 'translateY(-100%)';
            message.style.opacity = '0';
            setTimeout(() => {
                message.remove();
            }, 300);
        }, 2000);
    });
});