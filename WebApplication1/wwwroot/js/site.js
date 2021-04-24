function LoadProducts() {
    var tr = '';
    $.ajax({
        url: '/Product/GetProducts',
        method: 'GET',
        success: (result) => {
            $.each(result, (k, v) => {
                tr += `<tr>
                            <td>${v.Name}</td>
                            <td>${v.Stock}</td>
                            <td>
                                <a href='/product/Edit?id=${v.Id}'>Edit</a> |
                                <a href='/product/Details?id=${v.Id}'>Details</a> |
                                <a href='/product/Delete?id=${v.Id}'>Delete</a> 
                            </td>
                          </tr>`
            });
            $("#tablebody").html(tr);
        },
        error: console.log
    })
}

$(() => {
    console.log("Hello")
    LoadProducts();
    var connection = new signalR.HubConnectionBuilder().withUrl("hub").build();
    connection.start();
    connection.on("ProductsChanged", LoadProducts);
    
})