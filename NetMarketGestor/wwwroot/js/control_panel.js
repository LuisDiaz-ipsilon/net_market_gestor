var orders;

function doRequest(method, resource, headers, body = null) {
    return new Promise((resolve, reject) => {
        let xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                try {
                    resolve(JSON.parse(this.responseText));
                } catch (e) {
                    console.error(e);
                    return this.responseText;
                }
                return this.response;
            } else if (this.readyState == 4 && this.status != 500) {
                resolve(JSON.parse(this.responseText));
            } else if (this.readyState == 4) {
                reject();
            }
        };
        xhr.open(method, resource);
        for (const prop in headers) {
            if (Object.hasOwnProperty.call(headers, prop)) {
                xhr.setRequestHeader(prop, headers[prop]);
            }
        }
        if (body)
        {
            xhr.send(body);
        } else {
            xhr.send();
        }
    }).then((res) => { return res; }, () => { return false; });
}

async function apiLogin() {
    await doRequest('POST', '/cuentas/registrar', { 'Content-Type': 'application/json' }, atob("eyAiZW1haWwiOiAidXN1YXJpb0BlamVtcGxvLmNvbSIsICJwYXNzd29yZCI6ICJjb250ckExMjMqIiB9"));
    let tokenReq = await doRequest('POST', '/cuentas/login', { 'Content-Type': 'application/json' }, atob("eyAiZW1haWwiOiAidXN1YXJpb0BlamVtcGxvLmNvbSIsICJwYXNzd29yZCI6ICJjb250ckExMjMqIiB9"));
    return tokenReq;
}

async function sendChangeStatusRequest(orderId) {
    let userCredentials = await apiLogin();
    for (var order of orders) {
        if (order.id == orderId) {
            if (order.estatus == 'en proceso') {
                order.estatus = 'en envio';
            } else if (order.estatus == 'en envio') {
                order.estatus = 'recibido';
            }
            delete order.id;
            delete order.user;
            delete order.userId;
            await doRequest('PUT', `/api/pedido/${orderId}`, { 'Content-Type': 'application/json', 'Authorization': `Bearer ${userCredentials.token}` }, JSON.stringify(order));
            loadOrders();
            return;
        }
    }
}

function changeStatus(elm)
{
    console.log(elm);
    let oC = elm;
    while (!oC.classList.contains('order-conainer'))
    {
        oC = oC.parentElement;
    }
    sendChangeStatusRequest(oC.children[0].querySelector('span').innerText);
}

function buildOrderView(order, ordersContainer) {
    let orderConainer = document.createElement('div');
    orderConainer.className = 'order-conainer';
    orderConainer.innerHTML = `<div class="order-id"><label>Pedido: </label><span>${order.id}</span></div><div class="order-address"><label>Domicilio:</label><span>${order.direccionEntrega}</span></div><div class="order-status"><label>Estatus:</label><div class="change-status-btn" onclick="changeStatus(this)"><span>${order.estatus}</span></div></div>`;
    ordersContainer.append(orderConainer);
}

function buildOrdersView(orders) {
    let ordersContainer = document.getElementById('orders');
    ordersContainer.innerHTML = '';
    for (const order of orders) {
        buildOrderView(order, ordersContainer);
    }
}

async function loadOrders() {
    let userCredentials = await apiLogin();
    orders = await doRequest('GET', '/api/pedidos', { 'Content-Type': 'application/json', 'Authorization': `Bearer ${userCredentials.token}` });
    buildOrdersView(orders);
}

function documentReady() {
    if (document.readyState == 'complete') {
        console.log("Document is ready");
        //setInterval(loadOrders, 10000);
        loadOrders();
    }
}

document.onreadystatechange = documentReady;