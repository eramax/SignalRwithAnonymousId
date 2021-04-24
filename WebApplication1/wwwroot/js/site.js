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
    console.log("Hello SignalR");
    LoadProducts();
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("hub")
        .withAutomaticReconnect([0, 0, 10000]).build();

    connection.on("ProductsChanged", LoadProducts);
    connection.onreconnecting(error => console.log(`Connection lost due to error "${error}". Reconnecting.`));
    connection.onreconnected(connectionId => console.log(`Connection reestablished. Connected with connectionId "${connectionId}".`));
    connection.start()
        .then(() => console.log('Connection started'))
        .catch(err => console.log('Error while starting connection: ' + err));   
})