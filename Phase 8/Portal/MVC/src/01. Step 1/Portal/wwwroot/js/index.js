(function () {
    'use strict'
    const form = document.getElementById("pezza-form");
    form.addEventListener("submit", onFormSubmit);

    function onFormSubmit(event) {
        event.preventDefault();
        var success = document.querySelector('.alert-success');
        var danger = document.querySelector('.alert-danger');
        success.style.display = 'none';
        danger.style.display = 'none';

        var checkboxes = document.querySelectorAll("input[name='pizza-selection']:checked");
        if(checkboxes.length == 0){
            danger.style.display = 'block';
            return;
        }
        const pizzas = Array.from(checkboxes).map(checkbox => checkbox.value);

        const dataObject = Object.fromEntries(new FormData(event.target).entries());
        delete dataObject["pizza-selection"];
        dataObject.pizzas = pizzas;        
        form.reset();
        success.style.display = 'block';
    }
})()