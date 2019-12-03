$(document).ready(function () {
    var apiBaseUrl = "https://localhost:44367/";
    $('#btnGetData').click(function () {
        $.ajax({
            url: apiBaseUrl + 'api/TransactionDisplay',
            type: 'GET',
            dataType: 'json',
            success: function () {
               
                var $table = $('<table/>').addClass('dataTable table table-bordered table-striped');
                var $header = $('<thead/>').html('<tr><th>ID</th><th>Payment</th><th>Status</th></tr>');
                $table.append($header);
                $.each(function (i, val) {
                    var $row = $('<tr/>');
                    $row.append($('<td/>').html(val.TransactionIdentificator));
                    $row.append($('<td/>').html(val.Amount + val.CurrencyCode));
                    $row.append($('<td/>').html(val.Status));
                    $table.append($row);
                });
                $('#updatePanel').html($table);
            },
            error: function () {
                alert('Error!');
            }
        });
    });
});  