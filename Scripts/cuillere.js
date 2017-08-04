//Auto-completion de la liste des ingrédients
$(document).ready(function () {
    $("#Ingredient_Name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '@Url.Action("GetIngredient", "RecetteDetails")',
                datatype: "json",
                data: {
                    term: request.term
                },
                success: function (data) {
                    response($.map(data, function (val, item) {
                        return {
                            label: val.Name,
                            value: val.Name,
                            IngredientId: val.ID
                        };
                    }));
                },
                error: function (response) {
                    alert(alert(response.responseText));
                }
            });
        },
        select: function (event, ui) {
            $("#Ingredient_Name").val(ui.item.IngredientId);
            $("#IngredientId").val(ui.item.IngredientId);
        }
    });
});

//Suppression des ingrédients de la liste de courses
$(document).ready(function () {
    $(".RemoveLink").click(function (e) {
        e.preventDefault();
        // Get the id from the link
        var recordToDelete = $(this).attr("data-id");
        if (recordToDelete !== '') {
            // Perform the ajax post
            $.post("/ShoppingList/RemoveFromCart", { "id": recordToDelete },
                function (data) {
                    // Successful requests get here
                    // Update the page elements
                    if (data.ItemCount === 0) {
                        $('#row-' + data.DeleteId).fadeOut('slow');
                    } else {
                        $('#item-count-' + data.DeleteId).text(data.ItemCount);
                    }
                    $('#cart-total').text(data.CartTotal);
                    $('#update-message').text(data.Message);
                    $('#cart-status').text('Cart (' + data.CartCount + ')');
                });
        }
    });
});

//Listes déroulantes en cascade (types pour les catégories Entrées et Viandes)
$(document).ready(function () {
    $('#listCat').change(function () {

        //alert('j\'entre dans la fonction!');
        $.ajax({
            type: "post",
            url: "/Recettes/GetTypes",
            data: { catId: $('#listCat').val() },
            datatype: "json",
            traditional: true,
            success: function (data) {
                var typeRequest = "<select class ='form-control' id='listType' name='TypeId'>";
                typeRequest = typeRequest + '<option value="">--Select--</option>';
                for (var i = 0; i < data.length; i++) {
                    typeRequest = typeRequest + '<option value=' + data[i].Value + '>' + data[i].Text + '</option>';
                }
                typeRequest = typeRequest + '</select><span class="field-valisation-valid text-danger" data-valmsg-for="TypeId" data-valmsg-replace="true"></span>';
                $('#Type').html(typeRequest);
            }
        });
    });
});