$(document).ready(function() {
    //events
    $("#btnPizza").on("click",function(){
       console.log('ddd'); 
    });


    //products 
    $.ajax({
        type: 'POST',
        dataType: 'json',
        data: JSON.stringify({}),
        contentType: 'application/json; charset=utf-8',
        url: 'http://localhost:58152/Product/Search'
    }).then(function(data) {
        console.log(data);
        if(data)
        {
            if(data.data)
            {
                for (i = 0; i < data.data.length; i++) {
                    var product = data.data[i];
                    var productCard = "<div class='col-lg-4 col-sm-12 px-2 py-2'><div class='card card-stats";
                    if (product.special === true){
                        productCard += " bg-default";
                    }
                    productCard += "'><div class='card-body pizza-card'><div class='row'><div class='col-12'><div class='numbers'><p class='card-title'>R"+product.price+"</p><p></p><p class='card-category'>"+product.name+"<br /><small>"+product.description+"</small></p></div></div></div></div></div>";
                    
                    
                    productCard += "<div class='input-group product-input-group'><input type='number' class='form-control' placeholder='0' aria-label='"+product.name+"' aria-describedby='"+product.name+"'><div class='input-group-append'><button class='btn btn-outline-secondary' type='button'>Add</button></div></div>";
                    productCard += "</div>";
                    $('#productDiv').append(productCard);
                }
            }
        }
    });

    //restaurants
    $.ajax({
        type: 'POST',
        dataType: 'json',
        data: JSON.stringify({}),
        contentType: 'application/json; charset=utf-8',
        url: 'http://localhost:58152/Restaurant/Search'
    }).then(function(data) {
        console.log(data);
        if(data)
        {
            if(data.data)
            {
                for (i = 0; i < data.data.length; i++) {
                    var restaurant = data.data[i];
                    var restCard = "<div class='col-lg-3'><div class='info'><h4 class='info-title'>"+restaurant.address.city+"</h4><hr class='line-primary-center' /><p>"+restaurant.name+"</p>";
                    restCard += "<iframe src='https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d7163.334432323665!2d28.04434017691293!3d-26.14239143386151!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x1e950cede23eb175%3A0x3cef7626ace4b244!2sMelrose%2C%20Johannesburg%2C%202196!5e0!3m2!1sen!2sza!4v1607889782854!5m2!1sen!2sza' width='200' height='150' frameborder='0' style='border:0;' allowfullscreen='' aria-hidden='false' tabindex='0'></iframe><br /><a class='btn btn-primary'>Choose Restaurant</a></div></div>";
                    $('#restaurantDiv').append(restCard);
                }
            }
        }
    });
});
