document.addEventListener('DOMContentLoaded', function () {
    function resetForm(formId) {
        const form = document.getElementById(formId);
        const inputs = form.querySelectorAll('input[type="text"], input[type="tel"], input[type="email"], input[type="password"], input[type="checkbox"]');
        inputs.forEach(input => {
            if (input.type === 'checkbox') {
                input.checked = false;
            } else {
                input.value = '';
            }
        });
        const popup = document.getElementById('errorPopup');
        if (popup) {
            popup.style.display = 'none';
        }
    }

    document.getElementById('clientBtn').addEventListener('click', function () {
        document.getElementById('clientForm').style.display = 'block';
        document.getElementById('restaurantForm').style.display = 'none';
        this.classList.add('active');
        document.getElementById('restaurantBtn').classList.remove('active');
        resetForm('clientForm');
    });

    document.getElementById('restaurantBtn').addEventListener('click', function () {
        document.getElementById('restaurantForm').style.display = 'block';
        document.getElementById('clientForm').style.display = 'none';
        this.classList.add('active');
        document.getElementById('clientBtn').classList.remove('active');
        resetForm('restaurantForm');
    });
});