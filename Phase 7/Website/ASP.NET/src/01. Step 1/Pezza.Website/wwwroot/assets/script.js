$(document).ready(function() {
    //events
    $("#btnPizzaNext").on("click", function () {
        var total = 0;
        var products = '';
        $('.product-error').hide();
        $('#productDiv .qty').each(function (i, obj) {
            var qty = $(obj).val();
            if (qty > 0) {
                var product = $(obj).parent();
                var id = product.find("Id").val();
                var name = product.find("Name").val();
                var price = product.find("Price").val();

                total += (price * qty);
                products += id + ':' + price + ':' + qty + '|';
            }
        });
        $("#Products").val(products);
        $("#Total").val(total);
        console.log(products);
        if (total > 0) {
            $('.carousel').carousel('next');
        }
        else {
            $('.product-error').show();
            $('.product-error').html('Please choose your favourite pizza while it\'s hot!');
        }
    });


    
});
